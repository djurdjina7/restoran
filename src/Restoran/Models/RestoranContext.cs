using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Restoran.Models
{
    public partial class RestoranContext : DbContext
    {
        public RestoranContext()
        {
        }

        public RestoranContext(DbContextOptions<RestoranContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Drzava> Drzava { get; set; }
        public virtual DbSet<Grad> Grad { get; set; }
        public virtual DbSet<Meni> Meni { get; set; }
        public virtual DbSet<Narudzba> Narudzba { get; set; }
        public virtual DbSet<Rezervacija> Rezervacija { get; set; }
        public virtual DbSet<Spisakzanarudzbu> Spisakzanarudzbu { get; set; }
        public virtual DbSet<Sto> Sto { get; set; }
        public virtual DbSet<Tipmenija> Tipmenija { get; set; }
        public virtual DbSet<Zaposleni> Zaposleni { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Drzava>(entity =>
            {
                entity.ToTable("drzava", "restoran");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Grad>(entity =>
            {
                entity.ToTable("grad", "restoran");

                entity.HasIndex(e => e.DrzavaId)
                    .HasName("fk_Grad_Drzava1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.DrzavaId)
                    .HasColumnName("Drzava_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.PostanskiBroj)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Drzava)
                    .WithMany(p => p.Grad)
                    .HasForeignKey(d => d.DrzavaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Grad_Drzava1");
            });

            modelBuilder.Entity<Meni>(entity =>
            {
                entity.ToTable("meni", "restoran");

                entity.HasIndex(e => e.TipMenijaId)
                    .HasName("fk_Meni_TipMenija1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Cijena).HasColumnType("decimal(10,2)");

                entity.Property(e => e.Kolicina).HasColumnType("int(11)");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.TipMenijaId)
                    .HasColumnName("TipMenija_Id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.TipMenija)
                    .WithMany(p => p.Meni)
                    .HasForeignKey(d => d.TipMenijaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Meni_TipMenija1");
            });

            modelBuilder.Entity<Narudzba>(entity =>
            {
                entity.ToTable("narudzba", "restoran");

                entity.HasIndex(e => e.StoId)
                    .HasName("fk_Narudzba_Sto1_idx");

                entity.HasIndex(e => e.ZaposleniId)
                    .HasName("fk_Narudzba_Zaposleni1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Cijena).HasColumnType("decimal(10,2)");

                entity.Property(e => e.StoId)
                    .HasColumnName("Sto_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ZaposleniId)
                    .HasColumnName("Zaposleni_Id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Sto)
                    .WithMany(p => p.Narudzba)
                    .HasForeignKey(d => d.StoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Narudzba_Sto1");

                entity.HasOne(d => d.Zaposleni)
                    .WithMany(p => p.Narudzba)
                    .HasForeignKey(d => d.ZaposleniId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Narudzba_Zaposleni1");
            });

            modelBuilder.Entity<Rezervacija>(entity =>
            {
                entity.ToTable("rezervacija", "restoran");

                entity.HasIndex(e => e.StoId)
                    .HasName("fk_Rezervacija_Sto1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.BrojOsoba).HasColumnType("int(11)");

                entity.Property(e => e.Datum).HasColumnType("date");

                entity.Property(e => e.PodaciGosta)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.StoId)
                    .HasColumnName("Sto_Id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Sto)
                    .WithMany(p => p.Rezervacija)
                    .HasForeignKey(d => d.StoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Rezervacija_Sto1");
            });

            modelBuilder.Entity<Spisakzanarudzbu>(entity =>
            {
                entity.HasKey(e => new { e.MeniId, e.NarudzbaId });

                entity.ToTable("spisakzanarudzbu", "restoran");

                entity.HasIndex(e => e.MeniId)
                    .HasName("fk_Meni_has_Narudzba_Meni1_idx");

                entity.HasIndex(e => e.NarudzbaId)
                    .HasName("fk_Meni_has_Narudzba_Narudzba1_idx");

                entity.Property(e => e.MeniId)
                    .HasColumnName("Meni_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NarudzbaId)
                    .HasColumnName("Narudzba_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cijena).HasColumnType("decimal(10,2)");

                entity.Property(e => e.Kolicina).HasColumnType("int(11)");

                entity.HasOne(d => d.Meni)
                    .WithMany(p => p.Spisakzanarudzbu)
                    .HasForeignKey(d => d.MeniId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Meni_has_Narudzba_Meni1");

                entity.HasOne(d => d.Narudzba)
                    .WithMany(p => p.Spisakzanarudzbu)
                    .HasForeignKey(d => d.NarudzbaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Meni_has_Narudzba_Narudzba1");
            });

            modelBuilder.Entity<Sto>(entity =>
            {
                entity.ToTable("sto", "restoran");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.BrojMjesta).HasColumnType("int(11)");

                entity.Property(e => e.BrojStola).HasColumnType("int(11)");

                entity.Property(e => e.Dostupan).HasColumnType("bit(1)");
            });

            modelBuilder.Entity<Tipmenija>(entity =>
            {
                entity.ToTable("tipmenija", "restoran");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Naziv)
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Zaposleni>(entity =>
            {
                entity.ToTable("zaposleni", "restoran");

                entity.HasIndex(e => e.GradId)
                    .HasName("fk_Zaposleni_Grad1_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("id_gost_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Adresa)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.BrojTelefona)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.GradId)
                    .HasColumnName("Grad_Id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ime)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.MaticniBroj)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Prezime)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Grad)
                    .WithMany(p => p.Zaposleni)
                    .HasForeignKey(d => d.GradId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Zaposleni_Grad1");
            });
        }
    }
}
