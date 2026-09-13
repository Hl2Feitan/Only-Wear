namespace OnlyWear.Api.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = "Cliente";
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    public ICollection<DisenoPersonalizado> Disenos { get; set; } = new List<DisenoPersonalizado>();
}