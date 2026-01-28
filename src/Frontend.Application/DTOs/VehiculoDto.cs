namespace Frontend.Application.DTOs;

public class VehiculoDto
{
    public int IdVehiculo { get; set; }
    public int IdCategoria { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Costo { get; set; }
    public string? CategoriaDescripcion { get; set; }
    public string? CategoriaCodigo { get; set; }
}
