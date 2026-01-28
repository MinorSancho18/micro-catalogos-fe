using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IClientesApiService
{
    Task<IEnumerable<ClienteDto>> ListarAsync();
    Task<ClienteDto?> ObtenerAsync(int id);
    Task<int> CrearAsync(ClienteDto dto);
    Task<bool> ActualizarAsync(int id, ClienteDto dto);
    Task<bool> EliminarAsync(int id);
}
