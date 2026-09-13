namespace OnlyWear.Api.DTOs;

public class PedidoResponse
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;
    public string MetodoEntrega { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<DetallePedidoResponse> Detalles { get; set; } = new();
}