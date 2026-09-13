namespace OnlyWear.Api.DTOs;

public class DetallePedidoResponse
{
    public int Id { get; set; }
    public int? ProductoId { get; set; }
    public int? DisenoPersonalizadoId { get; set; }
    public string Talle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}