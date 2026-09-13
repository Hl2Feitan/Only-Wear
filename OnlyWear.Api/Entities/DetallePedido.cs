namespace OnlyWear.Api.Entities;

public class DetallePedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public Pedido Pedido { get; set; } = null!;
    public int? ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int? DisenoPersonalizadoId { get; set; }
    public DisenoPersonalizado? DisenoPersonalizado { get; set; }
    public string Talle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}