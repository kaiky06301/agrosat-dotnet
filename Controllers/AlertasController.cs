using AgroSat.Api.Data;
using AgroSat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Controllers;

/// <summary>CRUD de alertas agricolas. REST: /api/alertas</summary>
[ApiController]
[Route("api/alertas")]
[Produces("application/json")]
public class AlertasController : ControllerBase
{
    private readonly AppDbContext _db;
    public AlertasController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertaAgricola>>> GetAll()
        => Ok(await _db.Alertas.AsNoTracking().ToListAsync());

    [HttpGet("{id:long}")]
    public async Task<ActionResult<AlertaAgricola>> GetById(long id)
    {
        var alerta = await _db.Alertas.FindAsync(id);
        return alerta is null ? NotFound() : Ok(alerta);
    }

    [HttpPost]
    public async Task<ActionResult<AlertaAgricola>> Create(AlertaAgricola alerta)
    {
        if (await _db.Talhoes.CountAsync(t => t.Id == alerta.IdTalhao) == 0)
            return BadRequest($"Talhao {alerta.IdTalhao} nao existe.");

        alerta.Id = 0;
        alerta.DataHora ??= DateTime.UtcNow;
        alerta.Resolvido ??= "N";
        _db.Alertas.Add(alerta);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = alerta.Id }, alerta);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, AlertaAgricola input)
    {
        var alerta = await _db.Alertas.FindAsync(id);
        if (alerta is null) return NotFound();

        alerta.IdTalhao = input.IdTalhao;
        alerta.Tipo = input.Tipo;
        alerta.Severidade = input.Severidade;
        alerta.Mensagem = input.Mensagem;
        alerta.DataHora = input.DataHora;
        alerta.Resolvido = input.Resolvido;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var alerta = await _db.Alertas.FindAsync(id);
        if (alerta is null) return NotFound();
        _db.Alertas.Remove(alerta);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
