using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyWear.Api.Data;
using OnlyWear.Api.DTOs;
using OnlyWear.Api.Entities;

namespace OnlyWear.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DisenosPersonalizadosController : ControllerBase
{
    private readonly OnlyWearDbContext _context;

    public DisenosPersonalizadosController(OnlyWearDbContext context)
    {
        _context = context;
    }

    [HttpGet("/api/usuarios/{usuarioId}/disenos")]
    public async Task<ActionResult<IEnumerable<DisenoPersonalizadoResponse>>> GetDisenosDelUsuario(int usuarioId)
    {
        var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
        if (!existeUsuario)
        {
            return NotFound();
        }

        var disenos = await _context.DisenosPersonalizados
            .Where(d => d.UsuarioId == usuarioId)
            .ToListAsync();

        return Ok(disenos.Select(ToResponse));
    }

    [HttpPost("/api/disenos")]
    public async Task<ActionResult<DisenoPersonalizadoResponse>> PostDiseno(CrearDisenoRequest request)
    {
        var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Id == request.UsuarioId);
        if (!existeUsuario)
        {
            return NotFound();
        }

        var existeProductoBase = await _context.Productos.AnyAsync(p => p.Id == request.ProductoBaseId);
        if (!existeProductoBase)
        {
            return BadRequest($"No existe el producto base {request.ProductoBaseId}.");
        }

        var diseno = new DisenoPersonalizado
        {
            UsuarioId = request.UsuarioId,
            ProductoBaseId = request.ProductoBaseId,
            ImagenClienteUrl = request.ImagenClienteUrl,
            PosicionX = request.PosicionX,
            PosicionY = request.PosicionY,
            Escala = request.Escala,
            TalleElegido = request.TalleElegido
        };

        _context.DisenosPersonalizados.Add(diseno);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDiseno), new { id = diseno.Id }, ToResponse(diseno));
    }

    [HttpGet("/api/disenos/{id}")]
    public async Task<ActionResult<DisenoPersonalizadoResponse>> GetDiseno(int id)
    {
        var diseno = await _context.DisenosPersonalizados.FindAsync(id);

        if (diseno == null)
        {
            return NotFound();
        }

        return Ok(ToResponse(diseno));
    }

    private static DisenoPersonalizadoResponse ToResponse(DisenoPersonalizado diseno)
    {
        return new DisenoPersonalizadoResponse
        {
            Id = diseno.Id,
            UsuarioId = diseno.UsuarioId,
            ProductoBaseId = diseno.ProductoBaseId,
            ImagenClienteUrl = diseno.ImagenClienteUrl,
            PosicionX = diseno.PosicionX,
            PosicionY = diseno.PosicionY,
            Escala = diseno.Escala,
            TalleElegido = diseno.TalleElegido
        };
    }
}