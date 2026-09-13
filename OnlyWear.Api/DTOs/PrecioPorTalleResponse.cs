namespace OnlyWear.Api.DTOs;

public class PrecioPorTalleResponse
{
    public int Id { get; set; }
    public string Talle { get; set; } = string.Empty;
    public decimal Precio { get; set; }
}