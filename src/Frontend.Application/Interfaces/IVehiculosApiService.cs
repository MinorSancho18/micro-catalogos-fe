using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IVehiculosApiService
{
    Task<IEnumerable<VehiculoDto>> ListarAsync();
    Task<VehiculoDto?> ObtenerAsync(int id);
    Task<int> CrearAsync(VehiculoDto dto);
    Task<bool> ActualizarAsync(int id, VehiculoDto dto);
    Task<bool> EliminarAsync(int id);
}
