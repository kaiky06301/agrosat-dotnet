using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Cultura agricola (soja, milho, etc.) com faixas ideais. Tabela CULTURA.
/// </summary>
[Table("CULTURA")]
public class Cultura
{
    [Key]
    [Column("ID_CULTURA")]
    public long Id { get; set; }

    [Required]
    [MaxLength(80)]
    [Column("NOME")]
    public string Nome { get; set; } = null!;

    [Column("UMIDADE_IDEAL_MIN", TypeName = "NUMBER(5,2)")]
    public decimal? UmidadeIdealMin { get; set; }

    [Column("UMIDADE_IDEAL_MAX", TypeName = "NUMBER(5,2)")]
    public decimal? UmidadeIdealMax { get; set; }

    [Column("TEMP_IDEAL_MIN", TypeName = "NUMBER(5,2)")]
    public decimal? TempIdealMin { get; set; }

    [Column("TEMP_IDEAL_MAX", TypeName = "NUMBER(5,2)")]
    public decimal? TempIdealMax { get; set; }

    [Column("CICLO_DIAS")]
    public int? CicloDias { get; set; }

    // Navegacao 1:N — Cultura -> Talhao
    [JsonIgnore]
    public ICollection<Talhao> Talhoes { get; set; } = new List<Talhao>();
}
