using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de recomendacoes. REST: /api/recomendacoes</summary>
[ApiController]
[Route("api/recomendacoes")]
[Produces("application/json")]
public class RecomendacoesController : ControllerBase
{
    private readonly AppDbContext _db;
    public RecomendacoesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recomendacao>>> GetAll()
        => Ok(await _db.Recomendacoes.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Recomendacao>> GetById(long id)
    {
        var rec = await _db.Recomendacoes.FindAsync(id);
        return rec is null ? NotFound() : Ok(rec);
    }

    [HttpPost]
    public async Task<ActionResult<Recomendacao>> Create(Recomendacao rec)
    {
        if (await _db.Talhoes.CountAsync(t => t.Id == rec.IdTalhao) == 0)
            return BadRequest($"Talhao {rec.IdTalhao} nao existe.");
        if (rec.IdAlerta is not null && await _db.Alertas.CountAsync(a => a.Id == rec.IdAlerta) == 0)
            return BadRequest($"Alerta {rec.IdAlerta} nao existe.");

        rec.Id = 0;
        rec.DataHora ??= DateTime.UtcNow;
        _db.Recomendacoes.Add(rec);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = rec.Id }, rec);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Recomendacao input)
    {
        var rec = await _db.Recomendacoes.FindAsync(id);
        if (rec is null) return NotFound();

        rec.IdTalhao = input.IdTalhao;
        rec.IdAlerta = input.IdAlerta;
        rec.Tipo = input.Tipo;
        rec.Texto = input.Texto;
        rec.DataHora = input.DataHora;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var rec = await _db.Recomendacoes.FindAsync(id);
        if (rec is null) return NotFound();
        _db.Recomendacoes.Remove(rec);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
