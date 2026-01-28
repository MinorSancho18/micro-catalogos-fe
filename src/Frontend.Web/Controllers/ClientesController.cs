using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Web.Controllers;

public class ClientesController : Controller
{
    private readonly IClientesApiService _clientesService;

    public ClientesController(IClientesApiService clientesService)
    {
        _clientesService = clientesService;
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
            var clientes = await _clientesService.ListarAsync();
            return Json(clientes);
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
            var cliente = await _clientesService.ObtenerAsync(id);
            if (cliente == null)
                return NotFound();
            return Json(cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _clientesService.CrearAsync(dto);
            return Ok(new { id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ClienteDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _clientesService.ActualizarAsync(id, dto);
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
            var result = await _clientesService.EliminarAsync(id);
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
