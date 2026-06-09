using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AgroSat.Api.Models;

/// <summary>
/// Leitura de telemetria enviada por um sensor. Tabela LEITURA_SENSOR.
/// </summary>
[Table("LEITURA_SENSOR")]
public class LeituraSensor
{
    [Key]
    [Column("ID_LEITURA")]
    public long Id { get; set; }

    [Required]
    [Column("ID_SENSOR")]
    public long IdSensor { get; set; }

    [Column("UMIDADE_SOLO", TypeName = "NUMBER(5,2)")]
    public decimal? UmidadeSolo { get; set; }

    [Column("TEMPERATURA", TypeName = "NUMBER(5,2)")]
    public decimal? Temperatura { get; set; }

    [Column("UMIDADE_AR", TypeName = "NUMBER(5,2)")]
    public decimal? UmidadeAr { get; set; }

    [Column("LUMINOSIDADE", TypeName = "NUMBER(7,2)")]
    public decimal? Luminosidade { get; set; }

    [Column("DATA_HORA")]
    public DateTime? DataHora { get; set; }

    // Navegacao N:1 — LeituraSensor -> Sensor
    [ForeignKey(nameof(IdSensor))]
    [JsonIgnore]
    public Sensor? Sensor { get; set; }
}
