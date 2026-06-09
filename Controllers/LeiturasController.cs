using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de leituras de sensor. REST: /api/leituras</summary>
[ApiController]
[Route("api/leituras")]
[Produces("application/json")]
public class LeiturasController : ControllerBase
{
    private readonly AppDbContext _db;
    public LeiturasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeituraSensor>>> GetAll()
        => Ok(await _db.LeiturasSensor.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<LeituraSensor>> GetById(long id)
    {
        var leitura = await _db.LeiturasSensor.FindAsync(id);
        return leitura is null ? NotFound() : Ok(leitura);
    }

    [HttpPost]
    public async Task<ActionResult<LeituraSensor>> Create(LeituraSensor leitura)
    {
        if (await _db.Sensores.CountAsync(s => s.Id == leitura.IdSensor) == 0)
            return BadRequest($"Sensor {leitura.IdSensor} nao existe.");

        leitura.Id = 0;
        leitura.DataHora ??= DateTime.UtcNow;
        _db.LeiturasSensor.Add(leitura);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = leitura.Id }, leitura);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, LeituraSensor input)
    {
        var leitura = await _db.LeiturasSensor.FindAsync(id);
        if (leitura is null) return NotFound();

        leitura.IdSensor = input.IdSensor;
        leitura.UmidadeSolo = input.UmidadeSolo;
        leitura.Temperatura = input.Temperatura;
        leitura.UmidadeAr = input.UmidadeAr;
        leitura.Luminosidade = input.Luminosidade;
        leitura.DataHora = input.DataHora;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var leitura = await _db.LeiturasSensor.FindAsync(id);
        if (leitura is null) return NotFound();
        _db.LeiturasSensor.Remove(leitura);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
