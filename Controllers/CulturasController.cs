using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de culturas. REST: /api/culturas</summary>
[ApiController]
[Route("api/culturas")]
[Produces("application/json")]
public class CulturasController : ControllerBase
{
    private readonly AppDbContext _db;
    public CulturasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cultura>>> GetAll()
        => Ok(await _db.Culturas.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Cultura>> GetById(long id)
    {
        var cultura = await _db.Culturas.FindAsync(id);
        return cultura is null ? NotFound() : Ok(cultura);
    }

    [HttpPost]
    public async Task<ActionResult<Cultura>> Create(Cultura cultura)
    {
        cultura.Id = 0;
        _db.Culturas.Add(cultura);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = cultura.Id }, cultura);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, Cultura input)
    {
        var cultura = await _db.Culturas.FindAsync(id);
        if (cultura is null) return NotFound();

        cultura.Nome = input.Nome;
        cultura.UmidadeIdealMin = input.UmidadeIdealMin;
        cultura.UmidadeIdealMax = input.UmidadeIdealMax;
        cultura.TempIdealMin = input.TempIdealMin;
        cultura.TempIdealMax = input.TempIdealMax;
        cultura.CicloDias = input.CicloDias;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var cultura = await _db.Culturas.FindAsync(id);
        if (cultura is null) return NotFound();
        _db.Culturas.Remove(cultura);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
