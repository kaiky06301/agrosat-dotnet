using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Propriedade rural de um usuario. Tabela PROPRIEDADE.
/// </summary>
[Table("PROPRIEDADE")]
public class Propriedade
{
    [Key]
    [Column("ID_PROPRIEDADE")]
    public long Id { get; set; }

    [Required]
    [MaxLength(120)]
    [Column("NOME")]
    public string Nome { get; set; } = null!;

    [Column("LATITUDE", TypeName = "NUMBER(9,6)")]
    public decimal? Latitude { get; set; }

    [Column("LONGITUDE", TypeName = "NUMBER(9,6)")]
    public decimal? Longitude { get; set; }

    [Column("AREA_TOTAL_HA", TypeName = "NUMBER(10,2)")]
    public decimal? AreaTotalHa { get; set; }

    [MaxLength(80)]
    [Column("MUNICIPIO")]
    public string? Municipio { get; set; }

    [MaxLength(2)]
    [Column("UF")]
    public string? Uf { get; set; }

    [Required]
    [Column("ID_USUARIO")]
    public long IdUsuario { get; set; }

    // Navegacao N:1 — Propriedade -> Usuario
    [ForeignKey(nameof(IdUsuario))]
    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    // Navegacao 1:N — Propriedade -> Talhao
    [JsonIgnore]
    public ICollection<Talhao> Talhoes { get; set; } = new List<Talhao>();
}
