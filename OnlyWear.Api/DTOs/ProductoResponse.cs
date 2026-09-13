namespace OnlyWear.Api.DTOs;

public class ProductoResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public bool Activo { get; set; }

    public List<PrecioPorTalleResponse> Precios { get; set; } = new();
}