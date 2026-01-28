using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Web.Controllers;

public class VehiculosController : Controller
{
    private readonly IVehiculosApiService _vehiculosService;
    private readonly ICategoriasApiService _categoriasService;

    public VehiculosController(IVehiculosApiService vehiculosService, ICategoriasApiService categoriasService)
    {
        _vehiculosService = vehiculosService;
        _categoriasService = categoriasService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            var vehiculos = await _vehiculosService.ListarAsync();
            return Json(vehiculos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(int id)
    {
        try
        {
            var vehiculo = await _vehiculosService.ObtenerAsync(id);
            if (vehiculo == null)
                return NotFound();
            return Json(vehiculo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerCategorias()
    {
        try
        {
            var categorias = await _categoriasService.ListarAsync();
            return Json(categorias);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] VehiculoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _vehiculosService.CrearAsync(dto);
            return Ok(new { id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int id, [FromBody] VehiculoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _vehiculosService.ActualizarAsync(id, dto);
            if (!result)
                return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var result = await _vehiculosService.EliminarAsync(id);
            if (!result)
                return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
