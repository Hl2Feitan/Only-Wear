using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlyWear.Api.Data;
using OnlyWear.Api.DTOs;
using OnlyWear.Api.Entities;

namespace OnlyWear.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly OnlyWearDbContext _context;

    public UsuariosController(OnlyWearDbContext context)
    {
        _context = context;
    }

    [HttpPost("registro")]
    public async Task<ActionResult<UsuarioResponse>> Registrar(RegistroUsuarioRequest request)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, ToResponse(usuario));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioResponse>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return Ok(ToResponse(usuario));
    }

    private static UsuarioResponse ToResponse(Usuario usuario)
    {
        return new UsuarioResponse
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol,
            FechaRegistro = usuario.FechaRegistro
        };
    }
}