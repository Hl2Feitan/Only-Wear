namespace OnlyWear.Api.DTOs;

public class ProductoUpdateRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public bool Activo { get; set; }
}