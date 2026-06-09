using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de talhoes. REST: /api/talhoes</summary>
[ApiController]
[Route("api/talhoes")]
[Produces("application/json")]
public class TalhoesController : ControllerBase
{
    private readonly AppDbContext _db;
    public TalhoesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Talhao>>> GetAll()
        => Ok(await _db.Talhoes.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Talhao>> GetById(long id)
    {
        var talhao = await _db.Talhoes.FindAsync(id);
        return talhao is null ? NotFound() : Ok(talhao);
    }

    [HttpPost]
    public async Task<ActionResult<Talhao>> Create(Talhao talhao)
    {
        if (await _db.Propriedades.CountAsync(p => p.Id == talhao.IdPropriedade) == 0)
            return BadRequest($"Propriedade {talhao.IdPropriedade} nao existe.");
        if (talhao.IdCultura is not null && await _db.Culturas.CountAsync(c => c.Id == talhao.IdCultura) == 0)
            return BadRequest($"Cultura {talhao.IdCultura} nao existe.");

        talhao.Id = 0;
        _db.Talhoes.Add(talhao);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = talhao.Id }, talhao);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Talhao input)
    {
        var talhao = await _db.Talhoes.FindAsync(id);
        if (talhao is null) return NotFound();

        talhao.Nome = input.Nome;
        talhao.AreaHa = input.AreaHa;
        talhao.IdPropriedade = input.IdPropriedade;
        talhao.IdCultura = input.IdCultura;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var talhao = await _db.Talhoes.FindAsync(id);
        if (talhao is null) return NotFound();
        _db.Talhoes.Remove(talhao);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
