using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Produtor rural dono de propriedades. Tabela USUARIO.
/// </summary>
[Table("USUARIO")]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    public long Id { get; set; }

    [Required]
    [MaxLength(120)]
    [Column("NOME")]
    public string Nome { get; set; } = null!;

    [Required]
    [MaxLength(11)]
    [Column("CPF")]
    public string Cpf { get; set; } = null!;

    [Required]
    [MaxLength(120)]
    [Column("EMAIL")]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("SENHA")]
    public string Senha { get; set; } = null!;

    [MaxLength(20)]
    [Column("TELEFONE")]
    public string? Telefone { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime? DataCadastro { get; set; }

    // Navegacao 1:N — Usuario -> Propriedade
    [JsonIgnore]
    public ICollection<Propriedade> Propriedades { get; set; } = new List<Propriedade>();
}
