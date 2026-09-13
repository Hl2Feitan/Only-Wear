namespace OnlyWear.Api.Entities;

public class Pedido
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "pagado";
    public string MetodoPago { get; set; } = string.Empty;
    public string MetodoEntrega { get; set; } = string.Empty;
    public decimal Total { get; set; }

    public ICollection<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
}