namespace OnlyWear.Api.Entities;

public class DisenoPersonalizado
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public string ImagenClienteUrl { get; set; } = string.Empty;
    public float PosicionX { get; set; }
    public float PosicionY { get; set; }
    public float Escala { get; set; }
    public string TalleElegido { get; set; } = string.Empty;

    public ICollection<DetallePedido> DetallesPedido { get; set; } = new List<DetallePedido>();
}