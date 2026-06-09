using AgroSat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroSat.Api.Data;

/// <summary>
/// DbContext do AgroSat. Mapeia as 10 entidades do contrato de dominio para o
/// esquema Oracle (database/01_ddl.sql) e configura os relacionamentos 1:N.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cultura> Culturas => Set<Cultura>();
    public DbSet<Propriedade> Propriedades => Set<Propriedade>();
    public DbSet<Talhao> Talhoes => Set<Talhao>();
    public DbSet<Sensor> Sensores => Set<Sensor>();
    public DbSet<LeituraSensor> LeiturasSensor => Set<LeituraSensor>();
    public DbSet<DadoSatelite> DadosSatelite => Set<DadoSatelite>();
    public DbSet<AlertaAgricola> Alertas => Set<AlertaAgricola>();
    public DbSet<Recomendacao> Recomendacoes => Set<Recomendacao>();
    public DbSet<Irrigacao> Irrigacoes => Set<Irrigacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -------------------------------------------------------------
        // Sequences Oracle (espelham o DDL). Para o Oracle os IDs sao
        // gerados por sequence; cada PK usa defaultValueSql "SEQ_x.NEXTVAL".
        // Os nomes batem com a migration InitialCreate e com o 01_ddl.sql.
        // -------------------------------------------------------------
        modelBuilder.HasSequence<long>("SEQ_USUARIO").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_CULTURA").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_PROPRIEDADE").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_TALHAO").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_SENSOR").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_LEITURA").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_DADO_SAT").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_ALERTA").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_RECOMENDACAO").StartsAt(1).IncrementsBy(1);
        modelBuilder.HasSequence<long>("SEQ_IRRIGACAO").StartsAt(1).IncrementsBy(1);

        // USUARIO
        modelBuilder.Entity<Usuario>(e =>
        {
            e.Property(u => u.Id).HasDefaultValueSql("SEQ_USUARIO.NEXTVAL");
            e.HasIndex(u => u.Cpf).IsUnique();
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Cultura>()
            .Property(c => c.Id).HasDefaultValueSql("SEQ_CULTURA.NEXTVAL");
        modelBuilder.Entity<Propriedade>()
            .Property(p => p.Id).HasDefaultValueSql("SEQ_PROPRIEDADE.NEXTVAL");
        modelBuilder.Entity<Talhao>()
            .Property(t => t.Id).HasDefaultValueSql("SEQ_TALHAO.NEXTVAL");
        modelBuilder.Entity<LeituraSensor>()
            .Property(l => l.Id).HasDefaultValueSql("SEQ_LEITURA.NEXTVAL");
        modelBuilder.Entity<DadoSatelite>()
            .Property(d => d.Id).HasDefaultValueSql("SEQ_DADO_SAT.NEXTVAL");
        modelBuilder.Entity<AlertaAgricola>()
            .Property(a => a.Id).HasDefaultValueSql("SEQ_ALERTA.NEXTVAL");
        modelBuilder.Entity<Recomendacao>()
            .Property(r => r.Id).HasDefaultValueSql("SEQ_RECOMENDACAO.NEXTVAL");
        modelBuilder.Entity<Irrigacao>()
            .Property(i => i.Id).HasDefaultValueSql("SEQ_IRRIGACAO.NEXTVAL");

        // PROPRIEDADE -> USUARIO (N:1)
        modelBuilder.Entity<Propriedade>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Propriedades)
            .HasForeignKey(p => p.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        // TALHAO -> PROPRIEDADE (N:1)
        modelBuilder.Entity<Talhao>()
            .HasOne(t => t.Propriedade)
            .WithMany(p => p.Talhoes)
            .HasForeignKey(t => t.IdPropriedade)
            .OnDelete(DeleteBehavior.Restrict);

        // TALHAO -> CULTURA (N:1, opcional)
        modelBuilder.Entity<Talhao>()
            .HasOne(t => t.Cultura)
            .WithMany(c => c.Talhoes)
            .HasForeignKey(t => t.IdCultura)
            .OnDelete(DeleteBehavior.SetNull);

        // SENSOR -> TALHAO (N:1)
        modelBuilder.Entity<Sensor>(e =>
        {
            e.Property(s => s.Id).HasDefaultValueSql("SEQ_SENSOR.NEXTVAL");
            e.HasIndex(s => s.Codigo).IsUnique();
            e.HasOne(s => s.Talhao)
             .WithMany(t => t.Sensores)
             .HasForeignKey(s => s.IdTalhao)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // LEITURA_SENSOR -> SENSOR (N:1)
        modelBuilder.Entity<LeituraSensor>()
            .HasOne(l => l.Sensor)
            .WithMany(s => s.Leituras)
            .HasForeignKey(l => l.IdSensor)
            .OnDelete(DeleteBehavior.Restrict);

        // DADO_SATELITE -> TALHAO (N:1)
        modelBuilder.Entity<DadoSatelite>()
            .HasOne(d => d.Talhao)
            .WithMany(t => t.DadosSatelite)
            .HasForeignKey(d => d.IdTalhao)
            .OnDelete(DeleteBehavior.Restrict);

        // ALERTA_AGRICOLA -> TALHAO (N:1)
        modelBuilder.Entity<AlertaAgricola>()
            .HasOne(a => a.Talhao)
            .WithMany(t => t.Alertas)
            .HasForeignKey(a => a.IdTalhao)
            .OnDelete(DeleteBehavior.Restrict);

        // RECOMENDACAO -> TALHAO (N:1) e -> ALERTA (N:1, opcional)
        modelBuilder.Entity<Recomendacao>(e =>
        {
            e.HasOne(r => r.Talhao)
             .WithMany(t => t.Recomendacoes)
             .HasForeignKey(r => r.IdTalhao)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(r => r.Alerta)
             .WithMany(a => a.Recomendacoes)
             .HasForeignKey(r => r.IdAlerta)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // IRRIGACAO -> TALHAO (N:1)
        modelBuilder.Entity<Irrigacao>()
            .HasOne(i => i.Talhao)
            .WithMany(t => t.Irrigacoes)
            .HasForeignKey(i => i.IdTalhao)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
