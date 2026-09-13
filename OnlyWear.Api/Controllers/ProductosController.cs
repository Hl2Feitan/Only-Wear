using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyWear.Api.Data;
using OnlyWear.Api.DTOs;
using OnlyWear.Api.Entities;

namespace OnlyWear.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly OnlyWearDbContext _context;

    public ProductosController(OnlyWearDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductos()
    {
        var productos = await _context.Productos
            .Where(p => p.Activo)
            .Include(p => p.Precios)
            .ToListAsync();

        return Ok(productos.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoResponse>> GetProducto(int id)
    {
        var producto = await _context.Productos
            .Include(p => p.Precios)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto == null)
        {
            return NotFound();
        }

        return Ok(ToResponse(producto));
    }

    [HttpPost]
    public async Task<ActionResult<ProductoResponse>> PostProducto(ProductoRequest request)
    {
        var producto = new Producto
        {
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            ImagenUrl = request.ImagenUrl,
            Activo = true
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, ToResponse(producto));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProducto(int id, ProductoUpdateRequest request)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        producto.Nombre = request.Nombre;
        producto.Descripcion = request.Descripcion;
        producto.ImagenUrl = request.ImagenUrl;
        producto.Activo = request.Activo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            return NotFound();
        }

        producto.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static ProductoResponse ToResponse(Producto producto)
    {
        return new ProductoResponse
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            ImagenUrl = producto.ImagenUrl,
            Activo = producto.Activo,
            Precios = producto.Precios.Select(p => new PrecioPorTalleResponse
            {
                Id = p.Id,
                Talle = p.Talle,
                Precio = p.Precio
            }).ToList()
        };
    }
}