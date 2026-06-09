using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Sensor fisico (ESP32) instalado em um talhao. Tabela SENSOR.
/// status: ATIVO | INATIVO
/// </summary>
[Table("SENSOR")]
public class Sensor
{
    [Key]
    [Column("ID_SENSOR")]
    public long Id { get; set; }

    [Required]
    [MaxLength(40)]
    [Column("CODIGO")]
    public string Codigo { get; set; } = null!;

    [MaxLength(40)]
    [Column("TIPO")]
    public string? Tipo { get; set; }

    [MaxLength(20)]
    [Column("STATUS")]
    public string? Status { get; set; } = "ATIVO";

    [Column("DATA_INSTALACAO")]
    public DateTime? DataInstalacao { get; set; }

    [Required]
    [Column("ID_TALHAO")]
    public long IdTalhao { get; set; }

    // Navegacao N:1 — Sensor -> Talhao
    [ForeignKey(nameof(IdTalhao))]
    [JsonIgnore]
    public Talhao? Talhao { get; set; }

    // Navegacao 1:N — Sensor -> LeituraSensor
    [JsonIgnore]
    public ICollection<LeituraSensor> Leituras { get; set; } = new List<LeituraSensor>();
}
