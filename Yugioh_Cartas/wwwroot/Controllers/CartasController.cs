using Microsoft.AspNetCore.Mvc;
using Yugioh_Cartas.Services;

namespace Yugioh_Cartas.wwwroot.Controllers
{      

    [ApiController]
    [Route("api/[controller]")]
    public class CartasController : ControllerBase
    {
        private readonly ICartaService _service;

        public CartasController(ICartaService service)
        {
            _service = service; 
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetTodasCartas()
        {
            var cartas = await _service.ObterCartas();
            return Ok(cartas); 
        }
    }
}

