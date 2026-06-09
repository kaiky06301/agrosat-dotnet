using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de sensores. REST: /api/sensores</summary>
[ApiController]
[Route("api/sensores")]
[Produces("application/json")]
public class SensoresController : ControllerBase
{
    private readonly AppDbContext _db;
    public SensoresController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sensor>>> GetAll()
        => Ok(await _db.Sensores.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Sensor>> GetById(long id)
    {
        var sensor = await _db.Sensores.FindAsync(id);
        return sensor is null ? NotFound() : Ok(sensor);
    }

    [HttpPost]
    public async Task<ActionResult<Sensor>> Create(Sensor sensor)
    {
        if (await _db.Talhoes.CountAsync(t => t.Id == sensor.IdTalhao) == 0)
            return BadRequest($"Talhao {sensor.IdTalhao} nao existe.");

        sensor.Id = 0;
        sensor.Status ??= "ATIVO";
        sensor.DataInstalacao ??= DateTime.UtcNow;
        _db.Sensores.Add(sensor);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = sensor.Id }, sensor);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Sensor input)
    {
        var sensor = await _db.Sensores.FindAsync(id);
        if (sensor is null) return NotFound();

        sensor.Codigo = input.Codigo;
        sensor.Tipo = input.Tipo;
        sensor.Status = input.Status;
        sensor.DataInstalacao = input.DataInstalacao;
        sensor.IdTalhao = input.IdTalhao;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var sensor = await _db.Sensores.FindAsync(id);
        if (sensor is null) return NotFound();
        _db.Sensores.Remove(sensor);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
