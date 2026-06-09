using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de irrigacoes. REST: /api/irrigacoes</summary>
[ApiController]
[Route("api/irrigacoes")]
[Produces("application/json")]
public class IrrigacoesController : ControllerBase
{
    private readonly AppDbContext _db;
    public IrrigacoesController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Irrigacao>>> GetAll()
        => Ok(await _db.Irrigacoes.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Irrigacao>> GetById(long id)
    {
        var irrig = await _db.Irrigacoes.FindAsync(id);
        return irrig is null ? NotFound() : Ok(irrig);
    }

    [HttpPost]
    public async Task<ActionResult<Irrigacao>> Create(Irrigacao irrig)
    {
        if (await _db.Talhoes.CountAsync(t => t.Id == irrig.IdTalhao) == 0)
            return BadRequest($"Talhao {irrig.IdTalhao} nao existe.");

        irrig.Id = 0;
        irrig.Modo ??= "AUTO";
        _db.Irrigacoes.Add(irrig);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = irrig.Id }, irrig);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Irrigacao input)
    {
        var irrig = await _db.Irrigacoes.FindAsync(id);
        if (irrig is null) return NotFound();

        irrig.IdTalhao = input.IdTalhao;
        irrig.Inicio = input.Inicio;
        irrig.Fim = input.Fim;
        irrig.VolumeLitros = input.VolumeLitros;
        irrig.Modo = input.Modo;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var irrig = await _db.Irrigacoes.FindAsync(id);
        if (irrig is null) return NotFound();
        _db.Irrigacoes.Remove(irrig);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
