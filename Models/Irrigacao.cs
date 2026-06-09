using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Evento de irrigacao de um talhao. Tabela IRRIGACAO.
/// modo: AUTO | MANUAL
/// </summary>
[Table("IRRIGACAO")]
public class Irrigacao
{
    [Key]
    [Column("ID_IRRIGACAO")]
    public long Id { get; set; }

    [Required]
    [Column("ID_TALHAO")]
    public long IdTalhao { get; set; }

    [Column("INICIO")]
    public DateTime? Inicio { get; set; }

    [Column("FIM")]
    public DateTime? Fim { get; set; }

    [Column("VOLUME_LITROS", TypeName = "NUMBER(10,2)")]
    public decimal? VolumeLitros { get; set; }

    [MaxLength(10)]
    [Column("MODO")]
    public string? Modo { get; set; } = "AUTO";

    // Navegacao N:1 — Irrigacao -> Talhao
    [ForeignKey(nameof(IdTalhao))]
    [JsonIgnore]
    public Talhao? Talhao { get; set; }
}
