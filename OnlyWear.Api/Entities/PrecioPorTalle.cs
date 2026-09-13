namespace OnlyWear.Api.Entities;

public class PrecioPorTalle
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public Producto Producto { get; set; } = null!;
    public string Talle { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}