using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace emensa.Models
{
    public partial class emensaContext : DbContext
    {
        public emensaContext()
        {
        }

        public emensaContext(DbContextOptions<emensaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Benutzer> Benutzer { get; set; }
        public virtual DbSet<BestellungEnthältMahlzeit> BestellungEnthältMahlzeit { get; set; }
        public virtual DbSet<Bestellungen> Bestellungen { get; set; }
        public virtual DbSet<Bilder> Bilder { get; set; }
        public virtual DbSet<Deklarationen> Deklarationen { get; set; }
        public virtual DbSet<Fachbereiche> Fachbereiche { get; set; }
        public virtual DbSet<FhAngehörige> FhAngehörige { get; set; }
        public virtual DbSet<Freunde> Freunde { get; set; }
        public virtual DbSet<GehörtZuFachbereiche> GehörtZuFachbereiche { get; set; }
        public virtual DbSet<Gäste> Gäste { get; set; }
        public virtual DbSet<Kategorien> Kategorien { get; set; }
        public virtual DbSet<Kommentare> Kommentare { get; set; }
        public virtual DbSet<MahlzeitDeklarationen> MahlzeitDeklarationen { get; set; }
        public virtual DbSet<Mahlzeiten> Mahlzeiten { get; set; }
        public virtual DbSet<MahlzeitenBilder> MahlzeitenBilder { get; set; }
        public virtual DbSet<MahlzeitenZutaten> MahlzeitenZutaten { get; set; }
        public virtual DbSet<Mitarbeiter> Mitarbeiter { get; set; }
        public virtual DbSet<Preise> Preise { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Zutaten> Zutaten { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("Server=localhost;Port=3306;Database=emensa;Uid=denis;Pwd=;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Benutzer>(entity =>
            {
                entity.HasKey(e => e.Nummer);

                entity.ToTable("Benutzer", "emensa");

                entity.HasIndex(e => e.EMail)
                    .HasName("c_EmailEindeutig")
                    .IsUnique();

                entity.HasIndex(e => e.Nutzername)
                    .HasName("c_NutzernameEindeutig")
                    .IsUnique();

                entity.Property(e => e.Nummer).HasColumnType("int(11)");

                entity.Property(e => e.Aktiv).HasColumnType("tinyint(1)");

                entity.Property(e => e.Alter)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.AnlegeDatum).HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.EMail)
                    .IsRequired()
                    .HasColumnName("E-Mail")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Geburtsdatum)
                    .HasColumnType("date")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.LetzerLogin).HasDefaultValueSql("NULL");

                entity.Property(e => e.Nachname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nutzername)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Vorname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BestellungEnthältMahlzeit>(entity =>
            {
                entity.ToTable("BestellungEnthältMahlzeit", "emensa");

                entity.HasIndex(e => e.FkBestellungen)
                    .HasName("c_fkBestellungen");

                entity.HasIndex(e => new { e.FkMahlzeit, e.FkBestellungen })
                    .HasName("c_MahlzeitBestellungUnique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Anzahl).HasColumnType("int(11)");

                entity.Property(e => e.FkBestellungen)
                    .HasColumnName("fkBestellungen")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkMahlzeit)
                    .HasColumnName("fkMahlzeit")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkBestellungenNavigation)
                    .WithMany(p => p.BestellungEnthältMahlzeit)
                    .HasForeignKey(d => d.FkBestellungen)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("c_fkBestellungen");

                entity.HasOne(d => d.FkMahlzeitNavigation)
                    .WithMany(p => p.BestellungEnthältMahlzeit)
                    .HasForeignKey(d => d.FkMahlzeit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("c_fkMahlzeit");
            });

            modelBuilder.Entity<Bestellungen>(entity =>
            {
                entity.HasKey(e => e.Nummer);

                entity.ToTable("Bestellungen", "emensa");

                entity.Property(e => e.Nummer).HasColumnType("int(11)");

                entity.Property(e => e.Abholzeitpunkt).HasDefaultValueSql("'0000-00-00 00:00:00'");

                entity.Property(e => e.BestellZeitpunkt).HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.Endpreis)
                    .HasColumnType("float(10,2)")
                    .HasDefaultValueSql("NULL");
            });

            modelBuilder.Entity<Bilder>(entity =>
            {
                entity.ToTable("Bilder", "emensa");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AltText)
                    .IsRequired()
                    .HasColumnName("Alt-Text")
                    .IsUnicode(false);

                entity.Property(e => e.Binärdaten)
                    .IsRequired()
                    .HasColumnType("longblob");

                entity.Property(e => e.Copyright)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Titel)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("NULL");
            });

            modelBuilder.Entity<Deklarationen>(entity =>
            {
                entity.HasKey(e => e.Zeichen);

                entity.ToTable("Deklarationen", "emensa");

                entity.Property(e => e.Zeichen)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Beschriftung)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasDefaultValueSql("NULL");
            });

            modelBuilder.Entity<Fachbereiche>(entity =>
            {
                entity.ToTable("Fachbereiche", "emensa");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Adresse)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("NULL");
            });

            modelBuilder.Entity<FhAngehörige>(entity =>
            {
                entity.ToTable("FH Angehörige", "emensa");

                entity.HasIndex(e => e.FkBenutzer)
                    .HasName("c_fkBentuzerFHange");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkBenutzer)
                    .HasColumnName("fkBenutzer")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkBenutzerNavigation)
                    .WithMany(p => p.FhAngehörige)
                    .HasForeignKey(d => d.FkBenutzer)
                    .HasConstraintName("c_fkBentuzerFHange");
            });

            modelBuilder.Entity<Freunde>(entity =>
            {
                entity.ToTable("Freunde", "emensa");

                entity.HasIndex(e => e.Freund)
                    .HasName("c_fkFreund");

                entity.HasIndex(e => e.Nutzer)
                    .HasName("c_fkBenutzer");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Freund).HasColumnType("int(11)");

                entity.Property(e => e.Nutzer).HasColumnType("int(11)");

                entity.HasOne(d => d.FreundNavigation)
                    .WithMany(p => p.FreundeFreundNavigation)
                    .HasForeignKey(d => d.Freund)
                    .HasConstraintName("c_fkFreund");

                entity.HasOne(d => d.NutzerNavigation)
                    .WithMany(p => p.FreundeNutzerNavigation)
                    .HasForeignKey(d => d.Nutzer)
                    .HasConstraintName("c_fkBenutzer");
            });

            modelBuilder.Entity<GehörtZuFachbereiche>(entity =>
            {
                entity.ToTable("gehörtZuFachbereiche", "emensa");

                entity.HasIndex(e => e.FkFachbereiche)
                    .HasName("c_fkFachbereiche");

                entity.HasIndex(e => e.FkFhange)
                    .HasName("c_fkFHAnge");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkFachbereiche)
                    .HasColumnName("fkFachbereiche")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkFhange)
                    .HasColumnName("fkFHAnge")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkFachbereicheNavigation)
                    .WithMany(p => p.GehörtZuFachbereiche)
                    .HasForeignKey(d => d.FkFachbereiche)
                    .HasConstraintName("c_fkFachbereiche");

                entity.HasOne(d => d.FkFhangeNavigation)
                    .WithMany(p => p.GehörtZuFachbereiche)
                    .HasForeignKey(d => d.FkFhange)
                    .HasConstraintName("c_fkFHAnge");
            });

            modelBuilder.Entity<Gäste>(entity =>
            {
                entity.ToTable("Gäste", "emensa");

                entity.HasIndex(e => e.FkBenutzer)
                    .HasName("c_fkBenutzerGast");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Ablaufdatum).HasColumnType("date");

                entity.Property(e => e.FkBenutzer)
                    .HasColumnName("fkBenutzer")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Grund)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.FkBenutzerNavigation)
                    .WithMany(p => p.Gäste)
                    .HasForeignKey(d => d.FkBenutzer)
                    .HasConstraintName("c_fkBenutzerGast");
            });

            modelBuilder.Entity<Kategorien>(entity =>
            {
                entity.ToTable("Kategorien", "emensa");

                entity.HasIndex(e => e.FkBild)
                    .HasName("c_fkKategorieBild");

                entity.HasIndex(e => e.FkOberKategorie)
                    .HasName("c_fkOberKategorie");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Bezeichnung)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FkBild)
                    .HasColumnName("fkBild")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.FkOberKategorie)
                    .HasColumnName("fkOberKategorie")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("NULL");

                entity.HasOne(d => d.FkBildNavigation)
                    .WithMany(p => p.Kategorien)
                    .HasForeignKey(d => d.FkBild)
                    .HasConstraintName("c_fkKategorieBild");

                entity.HasOne(d => d.FkOberKategorieNavigation)
                    .WithMany(p => p.InverseFkOberKategorieNavigation)
                    .HasForeignKey(d => d.FkOberKategorie)
                    .HasConstraintName("c_fkOberKategorie");
            });

            modelBuilder.Entity<Kommentare>(entity =>
            {
                entity.ToTable("Kommentare", "emensa");

                entity.HasIndex(e => e.FkStudentId)
                    .HasName("c_fkStudentID");

                entity.HasIndex(e => e.FkzuMahlzeit)
                    .HasName("c_fkzuMahlzeit");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Bemerkung)
                    .IsUnicode(false)
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Bewertung).HasColumnType("int(11)");

                entity.Property(e => e.FkStudentId)
                    .HasColumnName("fkStudentID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkzuMahlzeit)
                    .HasColumnName("fkzuMahlzeit")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("NULL");

                entity.HasOne(d => d.FkStudent)
                    .WithMany(p => p.Kommentare)
                    .HasForeignKey(d => d.FkStudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("c_fkStudentID");

                entity.HasOne(d => d.FkzuMahlzeitNavigation)
                    .WithMany(p => p.Kommentare)
                    .HasForeignKey(d => d.FkzuMahlzeit)
                    .HasConstraintName("c_fkzuMahlzeit");
            });

            modelBuilder.Entity<MahlzeitDeklarationen>(entity =>
            {
                entity.ToTable("MahlzeitDeklarationen", "emensa");

                entity.HasIndex(e => e.FkMahlzeit)
                    .HasName("c_fkMahlzeitDeklarationen");

                entity.HasIndex(e => new { e.FkDeklaration, e.FkMahlzeit })
                    .HasName("c_MahlzeitDeklarationenUnique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkDeklaration)
                    .IsRequired()
                    .HasColumnName("fkDeklaration")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.FkMahlzeit)
                    .HasColumnName("fkMahlzeit")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.FkDeklarationNavigation)
                    .WithMany(p => p.MahlzeitDeklarationen)
                    .HasForeignKey(d => d.FkDeklaration)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("c_fkDeklaration");

                entity.HasOne(d => d.FkMahlzeitNavigation)
                    .WithMany(p => p.MahlzeitDeklarationen)
                    .HasForeignKey(d => d.FkMahlzeit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("c_fkMahlzeitDeklarationen");
            });

            modelBuilder.Entity<Mahlzeiten>(entity =>
            {
                entity.ToTable("Mahlzeiten", "emensa");

                entity.HasIndex(e => e.FkKategorie)
                    .HasName("c_fkMahlzeitenKategorie");

                entity.HasIndex(e => e.Name)
                    .HasName("Name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Beschreibung)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.FkKategorie)
                    .HasColumnName("fkKategorie")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Verfügbar)
                    .HasColumnType("tinyint(1)")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.Vorrat).HasColumnType("int(11)");

                entity.HasOne(d => d.FkKategorieNavigation)
                    .WithMany(p => p.Mahlzeiten)
                    .HasForeignKey(d => d.FkKategorie)
                    .HasConstraintName("c_fkMahlzeitenKategorie");
            });

            modelBuilder.Entity<MahlzeitenBilder>(entity =>
            {
                entity.ToTable("MahlzeitenBilder", "emensa");

                entity.HasIndex(e => e.Idmahlzeiten)
                    .HasName("c_fkMahlzeitenBilder");

                entity.HasIndex(e => new { e.Idbilder, e.Idmahlzeiten })
                    .HasName("BilderMahlzeitUnique")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idbilder)
                    .HasColumnName("IDBilder")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idmahlzeiten)
                    .HasColumnName("IDMahlzeiten")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.IdbilderNavigation)
                    .WithMany(p => p.MahlzeitenBilder)
                    .HasForeignKey(d => d.Idbilder)
                    .HasConstraintName("c_fkBilderMahlzeiten");

                entity.HasOne(d => d.IdmahlzeitenNavigation)
                    .WithMany(p => p.MahlzeitenBilder)
                    .HasForeignKey(d => d.Idmahlzeiten)
                    .HasConstraintName("c_fkMahlzeitenBilder");
            });

            modelBuilder.Entity<MahlzeitenZutaten>(entity =>
            {
                entity.ToTable("MahlzeitenZutaten", "emensa");

                entity.HasIndex(e => e.Idmahlzeiten)
                    .HasName("c_fkMahlzeitenZutaten");

                entity.HasIndex(e => new { e.Idzutaten, e.Idmahlzeiten })
                    .HasName("ZutatProMahlzeit")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idmahlzeiten)
                    .HasColumnName("IDMahlzeiten")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Idzutaten)
                    .HasColumnName("IDZutaten")
                    .HasColumnType("int(5)");

                entity.HasOne(d => d.IdmahlzeitenNavigation)
                    .WithMany(p => p.MahlzeitenZutaten)
                    .HasForeignKey(d => d.Idmahlzeiten)
                    .HasConstraintName("c_fkMahlzeitenZutaten");

                entity.HasOne(d => d.IdzutatenNavigation)
                    .WithMany(p => p.MahlzeitenZutaten)
                    .HasForeignKey(d => d.Idzutaten)
                    .HasConstraintName("c_fkZutatenMahlzeiten");
            });

            modelBuilder.Entity<Mitarbeiter>(entity =>
            {
                entity.ToTable("Mitarbeiter", "emensa");

                entity.HasIndex(e => e.FkFhange)
                    .HasName("c_fkFHangeMitarb");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Büro).HasColumnType("int(11)");

                entity.Property(e => e.FkFhange)
                    .HasColumnName("fkFHange")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Telefon).HasColumnType("int(11)");

                entity.HasOne(d => d.FkFhangeNavigation)
                    .WithMany(p => p.Mitarbeiter)
                    .HasForeignKey(d => d.FkFhange)
                    .HasConstraintName("c_fkFHangeMitarb");
            });

            modelBuilder.Entity<Preise>(entity =>
            {
                entity.ToTable("Preise", "emensa");

                entity.HasIndex(e => e.FkMahlzeiten)
                    .HasName("c_fkPreiseMahlzeiten");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkMahlzeiten)
                    .HasColumnName("fkMahlzeiten")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Gastpreis).HasColumnType("float(4,2) unsigned");

                entity.Property(e => e.Jahr).HasColumnType("int(11)");

                entity.Property(e => e.MaPreis)
                    .HasColumnName("MA-Preis")
                    .HasColumnType("float(4,2) unsigned");

                entity.Property(e => e.Studentpreis).HasColumnType("float(4,2) unsigned");

                entity.HasOne(d => d.FkMahlzeitenNavigation)
                    .WithMany(p => p.Preise)
                    .HasForeignKey(d => d.FkMahlzeiten)
                    .HasConstraintName("c_fkPreiseMahlzeiten");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student", "emensa");

                entity.HasIndex(e => e.FkFhange)
                    .HasName("c_fkFHangeStud");

                entity.HasIndex(e => e.Matrikelnummer)
                    .HasName("c_MatrikelnummerUnique")
                    .IsUnique();

                entity.Property(e => e.StudentId)
                    .HasColumnName("StudentID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FkFhange)
                    .HasColumnName("fkFHange")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Matrikelnummer).HasColumnType("int(11)");

                entity.Property(e => e.Studiengang)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkFhangeNavigation)
                    .WithMany(p => p.Student)
                    .HasForeignKey(d => d.FkFhange)
                    .HasConstraintName("c_fkFHangeStud");
            });

            modelBuilder.Entity<Zutaten>(entity =>
            {
                entity.ToTable("Zutaten", "emensa");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(5)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Bio).HasColumnType("tinyint(1)");

                entity.Property(e => e.Glutenfrei).HasColumnType("tinyint(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vegan).HasColumnType("tinyint(1)");

                entity.Property(e => e.Vegetarisch).HasColumnType("tinyint(1)");
            });
        }
    }
}
