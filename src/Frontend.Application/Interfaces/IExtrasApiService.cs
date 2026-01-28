using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IExtrasApiService
{
    Task<IEnumerable<ExtraDto>> ListarAsync();
    Task<ExtraDto?> ObtenerAsync(int id);
    Task<int> CrearAsync(ExtraDto dto);
    Task<bool> ActualizarAsync(int id, ExtraDto dto);
    Task<bool> EliminarAsync(int id);
}
