using Yugioh_Cartas.Models;

namespace Yugioh_Cartas.Services
{
    public interface ICartaService
    {

        Task<List<Carta>> ObterCartas();
    }
}
