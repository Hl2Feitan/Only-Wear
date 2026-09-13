using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyWear.Api.Data;
using OnlyWear.Api.DTOs;
using OnlyWear.Api.Entities;

namespace OnlyWear.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreciosPorTalleController : ControllerBase
{
    private readonly OnlyWearDbContext _context;

    public PreciosPorTalleController(OnlyWearDbContext context)
    {
        _context = context;
    }

    [HttpGet("/api/productos/{productoId}/precios")]
    public async Task<ActionResult<IEnumerable<PrecioPorTalleResponse>>> GetPreciosDelProducto(int productoId)
    {
        var existeProducto = await _context.Productos.AnyAsync(p => p.Id == productoId);
        if (!existeProducto)
        {
            return NotFound();
        }

        var precios = await _context.PreciosPorTalle
            .Where(p => p.ProductoId == productoId)
            .ToListAsync();

        return Ok(precios.Select(ToResponse));
    }

    [HttpPost("/api/productos/{productoId}/precios")]
    public async Task<ActionResult<PrecioPorTalleResponse>> PostPrecio(int productoId, PrecioRequest request)
    {
        var existeProducto = await _context.Productos.AnyAsync(p => p.Id == productoId);
        if (!existeProducto)
        {
            return NotFound();
        }

        var precio = new PrecioPorTalle
        {
            ProductoId = productoId,
            Talle = request.Talle,
            Precio = request.Precio
        };

        _context.PreciosPorTalle.Add(precio);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPreciosDelProducto), new { productoId }, ToResponse(precio));
    }

    [HttpPut("/api/precios/{id}")]
    public async Task<IActionResult> PutPrecio(int id, PrecioRequest request)
    {
        var precio = await _context.PreciosPorTalle.FindAsync(id);

        if (precio == null)
        {
            return NotFound();
        }

        precio.Talle = request.Talle;
        precio.Precio = request.Precio;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("/api/precios/{id}")]
    public async Task<IActionResult> DeletePrecio(int id)
    {
        var precio = await _context.PreciosPorTalle.FindAsync(id);

        if (precio == null)
        {
            return NotFound();
        }

        _context.PreciosPorTalle.Remove(precio);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static PrecioPorTalleResponse ToResponse(PrecioPorTalle precio)
    {
        return new PrecioPorTalleResponse
        {
            Id = precio.Id,
            Talle = precio.Talle,
            Precio = precio.Precio
        };
    }
}