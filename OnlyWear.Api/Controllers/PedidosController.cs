using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyWear.Api.Data;
using OnlyWear.Api.DTOs;
using OnlyWear.Api.Entities;

namespace OnlyWear.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private const string EstadoPagado = "pagado";
    private const string EstadoPendiente = "pendiente";
    private const string EstadoEnProduccion = "en producción";
    private const string EstadoEnviado = "enviado";

    private readonly OnlyWearDbContext _context;

    public PedidosController(OnlyWearDbContext context)
    {
        _context = context;
    }

    [HttpGet("/api/usuarios/{usuarioId}/pedidos")]
    public async Task<ActionResult<IEnumerable<PedidoResponse>>> GetPedidosDelUsuario(int usuarioId)
    {
        var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
        if (!existeUsuario)
        {
            return NotFound();
        }

        var pedidos = await _context.Pedidos
            .Where(p => p.UsuarioId == usuarioId)
            .Include(p => p.Detalles)
            .ToListAsync();

        return Ok(pedidos.Select(ToResponse));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoResponse>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Detalles)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
        {
            return NotFound();
        }

        return Ok(ToResponse(pedido));
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponse>> PostPedido(CrearPedidoRequest request)
    {
        var existeUsuario = await _context.Usuarios.AnyAsync(u => u.Id == request.UsuarioId);
        if (!existeUsuario)
        {
            return NotFound();
        }

        if (request.Detalles == null || request.Detalles.Count == 0)
        {
            return BadRequest("El pedido debe incluir al menos un detalle.");
        }

        var pedido = new Pedido
        {
            UsuarioId = request.UsuarioId,
            MetodoPago = request.MetodoPago,
            MetodoEntrega = request.MetodoEntrega,
            Estado = string.IsNullOrWhiteSpace(request.MetodoPago) ? EstadoPendiente : EstadoPagado
        };

        decimal total = 0;

        foreach (var detalleRequest in request.Detalles)
        {
            bool tieneProducto = detalleRequest.ProductoId.HasValue;
            bool tieneDiseno = detalleRequest.DisenoPersonalizadoId.HasValue;

            if (tieneProducto == tieneDiseno)
            {
                return BadRequest("Cada detalle debe tener exactamente uno entre productoId y disenoPersonalizadoId.");
            }

            if (detalleRequest.Cantidad <= 0)
            {
                return BadRequest("La cantidad de cada detalle debe ser mayor a cero.");
            }

            decimal precioUnitario;

            if (tieneProducto)
            {
                var precio = await _context.PreciosPorTalle
                    .FirstOrDefaultAsync(p => p.ProductoId == detalleRequest.ProductoId && p.Talle == detalleRequest.Talle);

                if (precio == null)
                {
                    return BadRequest($"No existe un precio para el talle '{detalleRequest.Talle}' del producto {detalleRequest.ProductoId}.");
                }

                precioUnitario = precio.Precio;
            }
            else
            {
                var diseno = await _context.DisenosPersonalizados.FindAsync(detalleRequest.DisenoPersonalizadoId);
                if (diseno == null)
                {
                    return NotFound($"No existe el diseño {detalleRequest.DisenoPersonalizadoId}.");
                }

                var precio = await _context.PreciosPorTalle
                    .FirstOrDefaultAsync(p => p.ProductoId == diseno.ProductoBaseId && p.Talle == detalleRequest.Talle);

                if (precio == null)
                {
                    return BadRequest($"No existe un precio para el talle '{detalleRequest.Talle}' del producto base {diseno.ProductoBaseId} del diseño {diseno.Id}.");
                }

                precioUnitario = precio.Precio;
            }

            var detalle = new DetallePedido
            {
                ProductoId = detalleRequest.ProductoId,
                DisenoPersonalizadoId = detalleRequest.DisenoPersonalizadoId,
                Talle = detalleRequest.Talle,
                Cantidad = detalleRequest.Cantidad,
                PrecioUnitario = precioUnitario
            };

            pedido.Detalles.Add(detalle);
            total += precioUnitario * detalleRequest.Cantidad;
        }

        pedido.Total = total;
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, ToResponse(pedido));
    }

    [HttpPut("{id}/estado")]
    public async Task<IActionResult> PutEstado(int id, CambiarEstadoRequest request)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
        {
            return NotFound();
        }

        if (string.IsNullOrWhiteSpace(request.Estado))
        {
            return BadRequest("El campo estado es obligatorio.");
        }

        if (request.Estado.Equals(EstadoEnviado, StringComparison.OrdinalIgnoreCase) &&
            !pedido.Estado.Equals(EstadoPagado, StringComparison.OrdinalIgnoreCase) &&
            !pedido.Estado.Equals(EstadoEnProduccion, StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Un pedido no puede pasar a 'enviado' sin haber estado antes en 'pagado' o 'en producción'.");
        }

        pedido.Estado = request.Estado;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static PedidoResponse ToResponse(Pedido pedido)
    {
        return new PedidoResponse
        {
            Id = pedido.Id,
            UsuarioId = pedido.UsuarioId,
            Fecha = pedido.Fecha,
            Estado = pedido.Estado,
            MetodoPago = pedido.MetodoPago,
            MetodoEntrega = pedido.MetodoEntrega,
            Total = pedido.Total,
            Detalles = pedido.Detalles.Select(d => new DetallePedidoResponse
            {
                Id = d.Id,
                ProductoId = d.ProductoId,
                DisenoPersonalizadoId = d.DisenoPersonalizadoId,
                Talle = d.Talle,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario
            }).ToList()
        };
    }
}