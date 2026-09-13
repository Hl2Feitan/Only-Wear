namespace OnlyWear.Api.DTOs;

public class CrearDisenoRequest
{
    public int UsuarioId { get; set; }
    public int ProductoBaseId { get; set; }
    public string ImagenClienteUrl { get; set; } = string.Empty;
    public float PosicionX { get; set; }
    public float PosicionY { get; set; }
    public float Escala { get; set; }
    public string TalleElegido { get; set; } = string.Empty;
}