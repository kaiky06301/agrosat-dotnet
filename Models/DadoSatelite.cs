using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Dado de satelite (NDVI, umidade estimada, chuva) por talhao. Tabela DADO_SATELITE.
/// </summary>
[Table("DADO_SATELITE")]
public class DadoSatelite
{
    [Key]
    [Column("ID_DADO_SAT")]
    public long Id { get; set; }

    [Required]
    [Column("ID_TALHAO")]
    public long IdTalhao { get; set; }

    [Range(-1, 1)]
    [Column("NDVI", TypeName = "NUMBER(4,3)")]
    public decimal? Ndvi { get; set; }

    [Column("UMIDADE_ESTIMADA", TypeName = "NUMBER(5,2)")]
    public decimal? UmidadeEstimada { get; set; }

    [Column("INDICE_CHUVA_MM", TypeName = "NUMBER(6,2)")]
    public decimal? IndiceChuvaMm { get; set; }

    [Column("DATA_REFERENCIA")]
    public DateTime? DataReferencia { get; set; }

    // Navegacao N:1 — DadoSatelite -> Talhao
    [ForeignKey(nameof(IdTalhao))]
    [JsonIgnore]
    public Talhao? Talhao { get; set; }
}
