using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Yugioh_Cartas.Models;

namespace Yugioh_Cartas.Models
{
    public class YugiohApiResponse
    {
        public List<Carta> data { get; set; } = new List<Carta>();
    }

    public class Carta
    {
        [JsonPropertyName("name")]
        public string Nome { get; set; }

        [JsonPropertyName("type")]
        public string Tipo { get; set; }

        [JsonPropertyName("attribute")]
        public string Atributo { get; set; }

        [JsonPropertyName("race")]
        public string Raca { get; set; }

        [JsonPropertyName("card_images")]
        public List<CartaImg> CardImages { get; set; } = new List<CartaImg>();
    }

    public class CartaImg
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }
}








}
