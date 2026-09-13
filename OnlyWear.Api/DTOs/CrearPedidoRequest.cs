namespace OnlyWear.Api.DTOs;

public class CrearPedidoRequest
{
    public int UsuarioId { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string MetodoEntrega { get; set; } = string.Empty;
    public List<DetallePedidoRequest> Detalles { get; set; } = new();
}