using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Alerta agricola gerado para um talhao. Tabela ALERTA_AGRICOLA.
/// tipo: SECA | GEADA | EXCESSO_UMIDADE | RISCO_CLIMATICO
/// severidade: BAIXA | MEDIA | ALTA
/// resolvido: S | N
/// </summary>
[Table("ALERTA_AGRICOLA")]
public class AlertaAgricola
{
    [Key]
    [Column("ID_ALERTA")]
    public long Id { get; set; }

    [Required]
    [Column("ID_TALHAO")]
    public long IdTalhao { get; set; }

    [MaxLength(30)]
    [Column("TIPO")]
    public string? Tipo { get; set; }

    [MaxLength(20)]
    [Column("SEVERIDADE")]
    public string? Severidade { get; set; }

    [MaxLength(255)]
    [Column("MENSAGEM")]
    public string? Mensagem { get; set; }

    [Column("DATA_HORA")]
    public DateTime? DataHora { get; set; }

    [MaxLength(1)]
    [Column("RESOLVIDO")]
    public string? Resolvido { get; set; } = "N";

    // Navegacao N:1 — AlertaAgricola -> Talhao
    [ForeignKey(nameof(IdTalhao))]
    [JsonIgnore]
    public Talhao? Talhao { get; set; }

    // Navegacao 1:N — AlertaAgricola -> Recomendacao
    [JsonIgnore]
    public ICollection<Recomendacao> Recomendacoes { get; set; } = new List<Recomendacao>();
}
