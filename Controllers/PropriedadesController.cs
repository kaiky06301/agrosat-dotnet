using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de propriedades. REST: /api/propriedades</summary>
[ApiController]
[Route("api/propriedades")]
[Produces("application/json")]
public class PropriedadesController : ControllerBase
{
    private readonly AppDbContext _db;
    public PropriedadesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Propriedade>>> GetAll()
        => Ok(await _db.Propriedades.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Propriedade>> GetById(long id)
    {
        var prop = await _db.Propriedades.FindAsync(id);
        return prop is null ? NotFound() : Ok(prop);
    }

    [HttpPost]
    public async Task<ActionResult<Propriedade>> Create(Propriedade propriedade)
    {
        if (await _db.Usuarios.CountAsync(u => u.Id == propriedade.IdUsuario) == 0)
            return BadRequest($"Usuario {propriedade.IdUsuario} nao existe.");

        propriedade.Id = 0;
        _db.Propriedades.Add(propriedade);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = propriedade.Id }, propriedade);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Propriedade input)
    {
        var prop = await _db.Propriedades.FindAsync(id);
        if (prop is null) return NotFound();

        prop.Nome = input.Nome;
        prop.Latitude = input.Latitude;
        prop.Longitude = input.Longitude;
        prop.AreaTotalHa = input.AreaTotalHa;
        prop.Municipio = input.Municipio;
        prop.Uf = input.Uf;
        prop.IdUsuario = input.IdUsuario;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var prop = await _db.Propriedades.FindAsync(id);
        if (prop is null) return NotFound();
        _db.Propriedades.Remove(prop);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
