using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BankDataAccess
{
    public partial class bankingContext : DbContext
    {
        public bankingContext(DbContextOptions<bankingContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Anneetraitement> Anneetraitements { get; set; } = null!;
        public virtual DbSet<Budget> Budgets { get; set; } = null!;
        public virtual DbSet<Compte> Comptes { get; set; } = null!;
        public virtual DbSet<Moistraitement> Moistraitements { get; set; } = null!;
        public virtual DbSet<Suivicompte> Suivicomptes { get; set; } = null!;
        public virtual DbSet<Transactionobligatoire> Transactionobligatoires { get; set; } = null!;
        public virtual DbSet<Typestransaction> Typestransactions { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Anneetraitement>(entity =>
            {
                entity.HasKey(e => e.Annee)
                    .HasName("PRIMARY");

                entity.ToTable("anneetraitement");

                entity.Property(e => e.Annee)
                    .ValueGeneratedNever()
                    .HasColumnName("annee");
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.Idbudget)
                    .HasName("PRIMARY");

                entity.ToTable("budgets");

                entity.Property(e => e.Idbudget).HasColumnName("idbudget");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Nombudget)
                    .HasMaxLength(25)
                    .HasColumnName("nombudget");
            });

            modelBuilder.Entity<Compte>(entity =>
            {
                entity.HasKey(e => e.Idcompte)
                    .HasName("PRIMARY");

                entity.ToTable("comptes");

                entity.Property(e => e.Idcompte).HasColumnName("idcompte");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Nomcompte)
                    .HasMaxLength(25)
                    .HasColumnName("nomcompte");

                entity.HasMany(d => d.Idbudgets)
                    .WithMany(p => p.Idcomptes)
                    .UsingEntity<Dictionary<string, object>>(
                        "Comptebudget",
                        l => l.HasOne<Budget>().WithMany().HasForeignKey("Idbudget").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("comptebudget_ibfk_2"),
                        r => r.HasOne<Compte>().WithMany().HasForeignKey("Idcompte").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("comptebudget_ibfk_1"),
                        j =>
                        {
                            j.HasKey("Idcompte", "Idbudget").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("comptebudget");

                            j.HasIndex(new[] { "Idbudget" }, "idbudget");

                            j.IndexerProperty<int>("Idcompte").HasColumnName("idcompte");

                            j.IndexerProperty<int>("Idbudget").HasColumnName("idbudget");
                        });
            });

            modelBuilder.Entity<Moistraitement>(entity =>
            {
                entity.HasKey(e => e.Mois)
                    .HasName("PRIMARY");

                entity.ToTable("moistraitement");

                entity.Property(e => e.Mois)
                    .ValueGeneratedNever()
                    .HasColumnName("mois");
            });

            modelBuilder.Entity<Suivicompte>(entity =>
            {
                entity.HasKey(e => new { e.Idcompte, e.Idannee, e.Idmois })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("suivicompte");

                entity.HasIndex(e => e.Idannee, "idannee");

                entity.HasIndex(e => e.Idmois, "idmois");

                entity.HasIndex(e => e.Typeid, "typeid");

                entity.Property(e => e.Idcompte).HasColumnName("idcompte");

                entity.Property(e => e.Idannee).HasColumnName("idannee");

                entity.Property(e => e.Idmois).HasColumnName("idmois");

                entity.Property(e => e.Datetransaction)
                    .HasColumnType("datetime")
                    .HasColumnName("datetransaction");

                entity.Property(e => e.Isvalidate)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isvalidate");

                entity.Property(e => e.Montant).HasColumnName("montant");

                entity.Property(e => e.Nomtransaction)
                    .HasMaxLength(25)
                    .HasColumnName("nomtransaction");

                entity.Property(e => e.Typeid).HasColumnName("typeid");

                entity.HasOne(d => d.IdanneeNavigation)
                    .WithMany(p => p.Suivicomptes)
                    .HasForeignKey(d => d.Idannee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("suivicompte_ibfk_2");

                entity.HasOne(d => d.IdcompteNavigation)
                    .WithMany(p => p.Suivicomptes)
                    .HasForeignKey(d => d.Idcompte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("suivicompte_ibfk_1");

                entity.HasOne(d => d.IdmoisNavigation)
                    .WithMany(p => p.Suivicomptes)
                    .HasForeignKey(d => d.Idmois)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("suivicompte_ibfk_3");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Suivicomptes)
                    .HasForeignKey(d => d.Typeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("suivicompte_ibfk_4");
            });

            modelBuilder.Entity<Transactionobligatoire>(entity =>
            {
                entity.HasKey(e => e.Idtransac)
                    .HasName("PRIMARY");

                entity.ToTable("transactionobligatoire");

                entity.HasIndex(e => e.Idcompte, "idcompte");

                entity.HasIndex(e => e.Typeid, "typeid");

                entity.Property(e => e.Idtransac).HasColumnName("idtransac");

                entity.Property(e => e.Idcompte).HasColumnName("idcompte");

                entity.Property(e => e.Jour).HasColumnName("jour");

                entity.Property(e => e.Montant).HasColumnName("montant");

                entity.Property(e => e.Nomtransaction)
                    .HasMaxLength(25)
                    .HasColumnName("nomtransaction");

                entity.Property(e => e.Typeid).HasColumnName("typeid");

                entity.HasOne(d => d.IdcompteNavigation)
                    .WithMany(p => p.Transactionobligatoires)
                    .HasForeignKey(d => d.Idcompte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transactionobligatoire_ibfk_1");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Transactionobligatoires)
                    .HasForeignKey(d => d.Typeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transactionobligatoire_ibfk_2");
            });

            modelBuilder.Entity<Typestransaction>(entity =>
            {
                entity.HasKey(e => e.Idtype)
                    .HasName("PRIMARY");

                entity.ToTable("typestransaction");

                entity.Property(e => e.Idtype).HasColumnName("idtype");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Nom)
                    .HasMaxLength(25)
                    .HasColumnName("nom");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
