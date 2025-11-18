using System.Net.WebSockets;
using Yugioh_Cartas.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICartaService, CartasApiService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseWebSockets();
app.UseRouting();
app.UseAuthorization();

app.Map("/ws/cartas", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = 400;
        return;
    }

    var ws = await context.WebSockets.AcceptWebSocketAsync();
    var service = context.RequestServices.GetRequiredService<ICartaService>();
    var cartas = await service.ObterCartas();

    foreach (var c in cartas)
    {
        var imagem = (c.CardImages != null && c.CardImages.Any())
            ? c.CardImages[0].ImageUrl
            : "https://via.placeholder.com/150?text=Sem+Imagem";

        var dto = new
        {
            Nome = c.Nome,
            Tipo = c.Tipo,
            Raca = c.Raca,
            Atributo = c.Atributo,
            ImagemUrl = imagem
        };

        var json = System.Text.Json.JsonSerializer.Serialize(dto);
        var buffer = System.Text.Encoding.UTF8.GetBytes(json);
        await ws.SendAsync(buffer, WebSocketMessageType.Text, true, default);
    
        await Task.Delay(20); 
    }

    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Fim das cartas", default);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
