using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Talhao (subdivisao de uma propriedade, com uma cultura plantada). Tabela TALHAO.
/// </summary>
[Table("TALHAO")]
public class Talhao
{
    [Key]
    [Column("ID_TALHAO")]
    public long Id { get; set; }

    [Required]
    [MaxLength(80)]
    [Column("NOME")]
    public string Nome { get; set; } = null!;

    [Column("AREA_HA", TypeName = "NUMBER(10,2)")]
    public decimal? AreaHa { get; set; }

    [Required]
    [Column("ID_PROPRIEDADE")]
    public long IdPropriedade { get; set; }

    [Column("ID_CULTURA")]
    public long? IdCultura { get; set; }

    // Navegacao N:1
    [ForeignKey(nameof(IdPropriedade))]
    [JsonIgnore]
    public Propriedade? Propriedade { get; set; }

    [ForeignKey(nameof(IdCultura))]
    [JsonIgnore]
    public Cultura? Cultura { get; set; }

    // Navegacao 1:N
    [JsonIgnore]
    public ICollection<Sensor> Sensores { get; set; } = new List<Sensor>();

    [JsonIgnore]
    public ICollection<DadoSatelite> DadosSatelite { get; set; } = new List<DadoSatelite>();

    [JsonIgnore]
    public ICollection<AlertaAgricola> Alertas { get; set; } = new List<AlertaAgricola>();

    [JsonIgnore]
    public ICollection<Recomendacao> Recomendacoes { get; set; } = new List<Recomendacao>();

    [JsonIgnore]
    public ICollection<Irrigacao> Irrigacoes { get; set; } = new List<Irrigacao>();
}
