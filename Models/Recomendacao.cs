using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Recomendacao agronomica (pode estar ligada a um alerta). Tabela RECOMENDACAO.
/// tipo: IRRIGAR | AGUARDAR | PROTEGER_GEADA | DRENAR
/// </summary>
[Table("RECOMENDACAO")]
public class Recomendacao
{
    [Key]
    [Column("ID_RECOMENDACAO")]
    public long Id { get; set; }

    [Required]
    [Column("ID_TALHAO")]
    public long IdTalhao { get; set; }

    [Column("ID_ALERTA")]
    public long? IdAlerta { get; set; }

    [MaxLength(40)]
    [Column("TIPO")]
    public string? Tipo { get; set; }

    [MaxLength(255)]
    [Column("TEXTO")]
    public string? Texto { get; set; }

    [Column("DATA_HORA")]
    public DateTime? DataHora { get; set; }

    // Navegacao N:1
    [ForeignKey(nameof(IdTalhao))]
    [JsonIgnore]
    public Talhao? Talhao { get; set; }

    [ForeignKey(nameof(IdAlerta))]
    [JsonIgnore]
    public AlertaAgricola? Alerta { get; set; }
}
