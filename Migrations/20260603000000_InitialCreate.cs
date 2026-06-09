using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSat.Api.Migrations;

/// <summary>
/// Migration inicial do AgroSat (provider Oracle). Cria as 10 tabelas do
/// contrato de dominio com sequences para auto-incremento das PKs.
///
/// IMPORTANTE: Esta migration foi escrita manualmente como EXEMPLO/PONTO DE
/// PARTIDA, espelhando database/01_ddl.sql. O ideal e o Kaiky regenerar com:
///   dotnet ef migrations remove
///   dotnet ef migrations add InitialCreate
/// apos restaurar os pacotes, para o ModelSnapshot ficar 100% sincronizado
/// com o provider Oracle instalado na maquina dele.
/// </summary>
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // -------- Sequences (uma por tabela, como no DDL) --------
        migrationBuilder.CreateSequence<long>(name: "SEQ_USUARIO", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_CULTURA", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_PROPRIEDADE", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_TALHAO", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_SENSOR", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_LEITURA", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_DADO_SAT", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_ALERTA", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_RECOMENDACAO", startValue: 1L);
        migrationBuilder.CreateSequence<long>(name: "SEQ_IRRIGACAO", startValue: 1L);

        // -------- USUARIO --------
        migrationBuilder.CreateTable(
            name: "USUARIO",
            columns: table => new
            {
                ID_USUARIO = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_USUARIO.NEXTVAL"),
                NOME = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                CPF = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                EMAIL = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                SENHA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                TELEFONE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                DATA_CADASTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_USUARIO", x => x.ID_USUARIO));

        // -------- CULTURA --------
        migrationBuilder.CreateTable(
            name: "CULTURA",
            columns: table => new
            {
                ID_CULTURA = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_CULTURA.NEXTVAL"),
                NOME = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                UMIDADE_IDEAL_MIN = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                UMIDADE_IDEAL_MAX = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                TEMP_IDEAL_MIN = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                TEMP_IDEAL_MAX = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                CICLO_DIAS = table.Column<int>(type: "NUMBER(10)", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_CULTURA", x => x.ID_CULTURA));

        // -------- PROPRIEDADE --------
        migrationBuilder.CreateTable(
            name: "PROPRIEDADE",
            columns: table => new
            {
                ID_PROPRIEDADE = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_PROPRIEDADE.NEXTVAL"),
                NOME = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                LATITUDE = table.Column<decimal>(type: "NUMBER(9,6)", nullable: true),
                LONGITUDE = table.Column<decimal>(type: "NUMBER(9,6)", nullable: true),
                AREA_TOTAL_HA = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                MUNICIPIO = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: true),
                UF = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: true),
                ID_USUARIO = table.Column<long>(type: "NUMBER(19)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PROPRIEDADE", x => x.ID_PROPRIEDADE);
                table.ForeignKey("FK_PROPRIEDADE_USUARIO_ID_USUARIO", x => x.ID_USUARIO,
                    "USUARIO", "ID_USUARIO", onDelete: ReferentialAction.Restrict);
            });

        // -------- TALHAO --------
        migrationBuilder.CreateTable(
            name: "TALHAO",
            columns: table => new
            {
                ID_TALHAO = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_TALHAO.NEXTVAL"),
                NOME = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                AREA_HA = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                ID_PROPRIEDADE = table.Column<long>(type: "NUMBER(19)", nullable: false),
                ID_CULTURA = table.Column<long>(type: "NUMBER(19)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TALHAO", x => x.ID_TALHAO);
                table.ForeignKey("FK_TALHAO_PROPRIEDADE_ID_PROPRIEDADE", x => x.ID_PROPRIEDADE,
                    "PROPRIEDADE", "ID_PROPRIEDADE", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_TALHAO_CULTURA_ID_CULTURA", x => x.ID_CULTURA,
                    "CULTURA", "ID_CULTURA", onDelete: ReferentialAction.SetNull);
            });

        // -------- SENSOR --------
        migrationBuilder.CreateTable(
            name: "SENSOR",
            columns: table => new
            {
                ID_SENSOR = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_SENSOR.NEXTVAL"),
                CODIGO = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: false),
                TIPO = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: true),
                STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                DATA_INSTALACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                ID_TALHAO = table.Column<long>(type: "NUMBER(19)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SENSOR", x => x.ID_SENSOR);
                table.ForeignKey("FK_SENSOR_TALHAO_ID_TALHAO", x => x.ID_TALHAO,
                    "TALHAO", "ID_TALHAO", onDelete: ReferentialAction.Restrict);
            });

        // -------- LEITURA_SENSOR --------
        migrationBuilder.CreateTable(
            name: "LEITURA_SENSOR",
            columns: table => new
            {
                ID_LEITURA = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_LEITURA.NEXTVAL"),
                ID_SENSOR = table.Column<long>(type: "NUMBER(19)", nullable: false),
                UMIDADE_SOLO = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                TEMPERATURA = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                UMIDADE_AR = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                LUMINOSIDADE = table.Column<decimal>(type: "NUMBER(7,2)", nullable: true),
                DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_LEITURA_SENSOR", x => x.ID_LEITURA);
                table.ForeignKey("FK_LEITURA_SENSOR_SENSOR_ID_SENSOR", x => x.ID_SENSOR,
                    "SENSOR", "ID_SENSOR", onDelete: ReferentialAction.Restrict);
            });

        // -------- DADO_SATELITE --------
        migrationBuilder.CreateTable(
            name: "DADO_SATELITE",
            columns: table => new
            {
                ID_DADO_SAT = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_DADO_SAT.NEXTVAL"),
                ID_TALHAO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                NDVI = table.Column<decimal>(type: "NUMBER(4,3)", nullable: true),
                UMIDADE_ESTIMADA = table.Column<decimal>(type: "NUMBER(5,2)", nullable: true),
                INDICE_CHUVA_MM = table.Column<decimal>(type: "NUMBER(6,2)", nullable: true),
                DATA_REFERENCIA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DADO_SATELITE", x => x.ID_DADO_SAT);
                table.ForeignKey("FK_DADO_SATELITE_TALHAO_ID_TALHAO", x => x.ID_TALHAO,
                    "TALHAO", "ID_TALHAO", onDelete: ReferentialAction.Restrict);
            });

        // -------- ALERTA_AGRICOLA --------
        migrationBuilder.CreateTable(
            name: "ALERTA_AGRICOLA",
            columns: table => new
            {
                ID_ALERTA = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_ALERTA.NEXTVAL"),
                ID_TALHAO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                TIPO = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: true),
                SEVERIDADE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                MENSAGEM = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                RESOLVIDO = table.Column<string>(type: "NVARCHAR2(1)", maxLength: 1, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ALERTA_AGRICOLA", x => x.ID_ALERTA);
                table.ForeignKey("FK_ALERTA_AGRICOLA_TALHAO_ID_TALHAO", x => x.ID_TALHAO,
                    "TALHAO", "ID_TALHAO", onDelete: ReferentialAction.Restrict);
            });

        // -------- RECOMENDACAO --------
        migrationBuilder.CreateTable(
            name: "RECOMENDACAO",
            columns: table => new
            {
                ID_RECOMENDACAO = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_RECOMENDACAO.NEXTVAL"),
                ID_TALHAO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                ID_ALERTA = table.Column<long>(type: "NUMBER(19)", nullable: true),
                TIPO = table.Column<string>(type: "NVARCHAR2(40)", maxLength: 40, nullable: true),
                TEXTO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                DATA_HORA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RECOMENDACAO", x => x.ID_RECOMENDACAO);
                table.ForeignKey("FK_RECOMENDACAO_TALHAO_ID_TALHAO", x => x.ID_TALHAO,
                    "TALHAO", "ID_TALHAO", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_RECOMENDACAO_ALERTA_AGRICOLA_ID_ALERTA", x => x.ID_ALERTA,
                    "ALERTA_AGRICOLA", "ID_ALERTA", onDelete: ReferentialAction.SetNull);
            });

        // -------- IRRIGACAO --------
        migrationBuilder.CreateTable(
            name: "IRRIGACAO",
            columns: table => new
            {
                ID_IRRIGACAO = table.Column<long>(type: "NUMBER(19)", nullable: false,
                    defaultValueSql: "SEQ_IRRIGACAO.NEXTVAL"),
                ID_TALHAO = table.Column<long>(type: "NUMBER(19)", nullable: false),
                INICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                FIM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                VOLUME_LITROS = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                MODO = table.Column<string>(type: "NVARCHAR2(10)", maxLength: 10, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_IRRIGACAO", x => x.ID_IRRIGACAO);
                table.ForeignKey("FK_IRRIGACAO_TALHAO_ID_TALHAO", x => x.ID_TALHAO,
                    "TALHAO", "ID_TALHAO", onDelete: ReferentialAction.Restrict);
            });

        // -------- Indices unicos / FKs --------
        migrationBuilder.CreateIndex("IX_USUARIO_CPF", "USUARIO", "CPF", unique: true);
        migrationBuilder.CreateIndex("IX_USUARIO_EMAIL", "USUARIO", "EMAIL", unique: true);
        migrationBuilder.CreateIndex("IX_PROPRIEDADE_ID_USUARIO", "PROPRIEDADE", "ID_USUARIO");
        migrationBuilder.CreateIndex("IX_TALHAO_ID_PROPRIEDADE", "TALHAO", "ID_PROPRIEDADE");
        migrationBuilder.CreateIndex("IX_TALHAO_ID_CULTURA", "TALHAO", "ID_CULTURA");
        migrationBuilder.CreateIndex("IX_SENSOR_CODIGO", "SENSOR", "CODIGO", unique: true);
        migrationBuilder.CreateIndex("IX_SENSOR_ID_TALHAO", "SENSOR", "ID_TALHAO");
        migrationBuilder.CreateIndex("IX_LEITURA_SENSOR_ID_SENSOR", "LEITURA_SENSOR", "ID_SENSOR");
        migrationBuilder.CreateIndex("IX_DADO_SATELITE_ID_TALHAO", "DADO_SATELITE", "ID_TALHAO");
        migrationBuilder.CreateIndex("IX_ALERTA_AGRICOLA_ID_TALHAO", "ALERTA_AGRICOLA", "ID_TALHAO");
        migrationBuilder.CreateIndex("IX_RECOMENDACAO_ID_TALHAO", "RECOMENDACAO", "ID_TALHAO");
        migrationBuilder.CreateIndex("IX_RECOMENDACAO_ID_ALERTA", "RECOMENDACAO", "ID_ALERTA");
        migrationBuilder.CreateIndex("IX_IRRIGACAO_ID_TALHAO", "IRRIGACAO", "ID_TALHAO");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("IRRIGACAO");
        migrationBuilder.DropTable("RECOMENDACAO");
        migrationBuilder.DropTable("ALERTA_AGRICOLA");
        migrationBuilder.DropTable("DADO_SATELITE");
        migrationBuilder.DropTable("LEITURA_SENSOR");
        migrationBuilder.DropTable("SENSOR");
        migrationBuilder.DropTable("TALHAO");
        migrationBuilder.DropTable("PROPRIEDADE");
        migrationBuilder.DropTable("CULTURA");
        migrationBuilder.DropTable("USUARIO");

        migrationBuilder.DropSequence("SEQ_USUARIO");
        migrationBuilder.DropSequence("SEQ_CULTURA");
        migrationBuilder.DropSequence("SEQ_PROPRIEDADE");
        migrationBuilder.DropSequence("SEQ_TALHAO");
        migrationBuilder.DropSequence("SEQ_SENSOR");
        migrationBuilder.DropSequence("SEQ_LEITURA");
        migrationBuilder.DropSequence("SEQ_DADO_SAT");
        migrationBuilder.DropSequence("SEQ_ALERTA");
        migrationBuilder.DropSequence("SEQ_RECOMENDACAO");
        migrationBuilder.DropSequence("SEQ_IRRIGACAO");
    }
}
