namespace OnlyWear.Api.Entities;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public ICollection<PrecioPorTalle> Precios { get; set; } = new List<PrecioPorTalle>();
    public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
}