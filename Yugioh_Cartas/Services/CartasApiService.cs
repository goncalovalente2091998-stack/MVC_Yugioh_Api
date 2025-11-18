using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Text.Json;
using Yugioh_Cartas.Models;
using static System.Net.WebRequestMethods;

namespace Yugioh_Cartas.Services
{

    public class CartasApiService : ICartaService
    {

        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        public CartasApiService(IHttpClientFactory httpFactory, IMemoryCache cache)
        {
            _http = httpFactory.CreateClient();
            _http.Timeout = TimeSpan.FromMinutes(20);
            _cache = cache;
        }

        public async Task<List<Carta>> ObterCartas()
        {


            if (_cache.TryGetValue("TodasCartas", out List<Carta> cartas))
            {
                return cartas; // se existir, retorna imediatamente
            }


            string url = "https://db.ygoprodeck.com/api/v7/cardinfo.php";

            
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

         
            using var stream = await response.Content.ReadAsStreamAsync();

         
            var doc = await JsonSerializer.DeserializeAsync<YugiohApiResponse>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            });

          
            cartas = doc?.data ?? new List<Carta>();
            _cache.Set("TodasCartas", cartas, TimeSpan.FromHours(1));



            return cartas;


        }

        public class YugiohApiResponse
        {   
            public List<Carta> data { get; set; }
        }
    }
}

    

