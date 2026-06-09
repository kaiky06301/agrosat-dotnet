using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de dados de satelite. REST: /api/dados-satelite</summary>
[ApiController]
[Route("api/dados-satelite")]
[Produces("application/json")]
public class DadosSateliteController : ControllerBase
{
    private readonly AppDbContext _db;
    public DadosSateliteController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DadoSatelite>>> GetAll()
        => Ok(await _db.DadosSatelite.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<DadoSatelite>> GetById(long id)
    {
        var dado = await _db.DadosSatelite.FindAsync(id);
        return dado is null ? NotFound() : Ok(dado);
    }

    [HttpPost]
    public async Task<ActionResult<DadoSatelite>> Create(DadoSatelite dado)
    {
        if (await _db.Talhoes.CountAsync(t => t.Id == dado.IdTalhao) == 0)
            return BadRequest($"Talhao {dado.IdTalhao} nao existe.");

        dado.Id = 0;
        _db.DadosSatelite.Add(dado);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dado.Id }, dado);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, DadoSatelite input)
    {
        var dado = await _db.DadosSatelite.FindAsync(id);
        if (dado is null) return NotFound();

        dado.IdTalhao = input.IdTalhao;
        dado.Ndvi = input.Ndvi;
        dado.UmidadeEstimada = input.UmidadeEstimada;
        dado.IndiceChuvaMm = input.IndiceChuvaMm;
        dado.DataReferencia = input.DataReferencia;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var dado = await _db.DadosSatelite.FindAsync(id);
        if (dado is null) return NotFound();
        _db.DadosSatelite.Remove(dado);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
