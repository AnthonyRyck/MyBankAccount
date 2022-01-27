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
        public virtual DbSet<Configbank> Configbanks { get; set; } = null!;
        public virtual DbSet<Moistraitement> Moistraitements { get; set; } = null!;
        public virtual DbSet<Suivicompte> Suivicomptes { get; set; } = null!;
        public virtual DbSet<Transactionobligatoire> Transactionobligatoires { get; set; } = null!;
        public virtual DbSet<Typebudget> Typebudgets { get; set; } = null!;
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
                entity.HasKey(e => new { e.Idbudget, e.Idcompte })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("budgets");

                entity.HasIndex(e => e.Idcompte, "idcompte");

                entity.HasIndex(e => e.Typebudgetid, "typebudgetid");

                entity.Property(e => e.Idbudget)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idbudget");

                entity.Property(e => e.Idcompte).HasColumnName("idcompte");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Montant)
                    .HasPrecision(6, 2)
                    .HasColumnName("montant");

                entity.Property(e => e.Nombudget)
                    .HasMaxLength(25)
                    .HasColumnName("nombudget");

                entity.Property(e => e.Typebudgetid).HasColumnName("typebudgetid");

                entity.HasOne(d => d.IdcompteNavigation)
                    .WithMany(p => p.Budgets)
                    .HasForeignKey(d => d.Idcompte)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("budgets_ibfk_2");

                entity.HasOne(d => d.Typebudget)
                    .WithMany(p => p.Budgets)
                    .HasForeignKey(d => d.Typebudgetid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("budgets_ibfk_1");
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
            });

            modelBuilder.Entity<Configbank>(entity =>
            {
                entity.HasKey(e => new { e.Idcomptedefault, e.Annee, e.Mois })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("configbank");

                entity.HasIndex(e => e.Annee, "annee");

                entity.HasIndex(e => e.Mois, "mois");

                entity.Property(e => e.Idcomptedefault).HasColumnName("idcomptedefault");

                entity.Property(e => e.Annee).HasColumnName("annee");

                entity.Property(e => e.Mois).HasColumnName("mois");

                entity.HasOne(d => d.AnneeNavigation)
                    .WithMany(p => p.Configbanks)
                    .HasForeignKey(d => d.Annee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("configbank_ibfk_2");

                entity.HasOne(d => d.IdcomptedefaultNavigation)
                    .WithMany(p => p.Configbanks)
                    .HasForeignKey(d => d.Idcomptedefault)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("configbank_ibfk_1");

                entity.HasOne(d => d.MoisNavigation)
                    .WithMany(p => p.Configbanks)
                    .HasForeignKey(d => d.Mois)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("configbank_ibfk_3");
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
                entity.HasKey(e => e.Idsuivi)
                    .HasName("PRIMARY");

                entity.ToTable("suivicompte");

                entity.HasIndex(e => e.Idannee, "idannee");

                entity.HasIndex(e => e.Idbudget, "idbudget");

                entity.HasIndex(e => e.Idcompte, "idcompte");

                entity.HasIndex(e => e.Idmois, "idmois");

                entity.HasIndex(e => e.Typeid, "typeid");

                entity.Property(e => e.Idsuivi).HasColumnName("idsuivi");

                entity.Property(e => e.Datetransaction)
                    .HasColumnType("datetime")
                    .HasColumnName("datetransaction");

                entity.Property(e => e.Idannee).HasColumnName("idannee");

                entity.Property(e => e.Idbudget).HasColumnName("idbudget");

                entity.Property(e => e.Idcompte).HasColumnName("idcompte");

                entity.Property(e => e.Idmois).HasColumnName("idmois");

                entity.Property(e => e.Isvalidate)
                    .HasColumnType("bit(1)")
                    .HasColumnName("isvalidate")
                    .HasDefaultValueSql("b'0'");

                entity.Property(e => e.Montant)
                    .HasPrecision(6, 2)
                    .HasColumnName("montant");

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

                entity.Property(e => e.Montant)
                    .HasPrecision(6, 2)
                    .HasColumnName("montant");

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

            modelBuilder.Entity<Typebudget>(entity =>
            {
                entity.HasKey(e => e.Idtypebudget)
                    .HasName("PRIMARY");

                entity.ToTable("typebudget");

                entity.Property(e => e.Idtypebudget).HasColumnName("idtypebudget");

                entity.Property(e => e.Nomtypebudget)
                    .HasMaxLength(25)
                    .HasColumnName("nomtypebudget");
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
