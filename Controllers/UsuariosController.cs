using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de usuarios (produtores). REST: /api/usuarios</summary>
[ApiController]
[Route("api/usuarios")]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _db;
    public UsuariosController(AppDbContext db) => _db = db;

    /// <summary>Lista todos os usuarios.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        => Ok(await _db.Usuarios.AsNoTracking().ToListAsync());

    /// <summary>Busca um usuario por id.</summary>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<Usuario>> GetById(long id)
    {
        var usuario = await _db.Usuarios.FindAsync(id);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>Cria um usuario.</summary>
    [HttpPost]
    public async Task<ActionResult<Usuario>> Create(Usuario usuario)
    {
        usuario.Id = 0;
        usuario.DataCadastro ??= DateTime.UtcNow;
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    /// <summary>Atualiza um usuario.</summary>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Usuario input)
    {
        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();

        usuario.Nome = input.Nome;
        usuario.Cpf = input.Cpf;
        usuario.Email = input.Email;
        usuario.Senha = input.Senha;
        usuario.Telefone = input.Telefone;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Remove um usuario.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();
        _db.Usuarios.Remove(usuario);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
