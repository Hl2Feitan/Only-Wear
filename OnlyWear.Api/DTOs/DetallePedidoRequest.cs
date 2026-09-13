namespace OnlyWear.Api.DTOs;

public class DetallePedidoRequest
{
    public int? ProductoId { get; set; }
    public int? DisenoPersonalizadoId { get; set; }
    public string Talle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}