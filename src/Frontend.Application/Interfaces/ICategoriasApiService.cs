using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface ICategoriasApiService
{
    Task<IEnumerable<CategoriaVehiculoDto>> ListarAsync();
}
