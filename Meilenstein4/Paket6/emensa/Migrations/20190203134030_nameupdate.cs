using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace emensa.Migrations
{
    public partial class nameupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "emensa");

            migrationBuilder.CreateTable(
                name: "Benutzer",
                schema: "emensa",
                columns: table => new
                {
                    Nummer = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Nutzername = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    LetzerLogin = table.Column<DateTimeOffset>(nullable: true, defaultValueSql: "NULL"),
                    EMail = table.Column<string>(name: "E-Mail", unicode: false, maxLength: 150, nullable: false),
                    SaltString = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    HashString = table.Column<string>(unicode: false, maxLength: 24, nullable: false),
                    AnlegeDatum = table.Column<DateTime>(nullable: false, defaultValueSql: "current_timestamp()"),
                    Aktiv = table.Column<byte>(type: "tinyint(1)", nullable: false),
                    Vorname = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Nachname = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Geburtsdatum = table.Column<DateTime>(type: "date", nullable: true, defaultValueSql: "NULL"),
                    Age = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benutzer", x => x.Nummer);
                });

            migrationBuilder.CreateTable(
                name: "Bilder",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    AltText = table.Column<string>(name: "Alt-Text", unicode: false, nullable: false),
                    Titel = table.Column<string>(unicode: false, maxLength: 150, nullable: false, defaultValueSql: "NULL"),
                    Binärdaten = table.Column<byte[]>(type: "longblob", nullable: false),
                    Copyright = table.Column<string>(unicode: false, maxLength: 300, nullable: false, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bilder", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Deklarationen",
                schema: "emensa",
                columns: table => new
                {
                    Zeichen = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    Beschriftung = table.Column<string>(unicode: false, maxLength: 32, nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deklarationen", x => x.Zeichen);
                });

            migrationBuilder.CreateTable(
                name: "Fachbereiche",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Website = table.Column<string>(unicode: false, maxLength: 250, nullable: true, defaultValueSql: "NULL"),
                    Adresse = table.Column<string>(unicode: false, maxLength: 300, nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fachbereiche", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Zutaten",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(5)", nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    Bio = table.Column<byte>(type: "tinyint(1)", nullable: false),
                    Vegetarisch = table.Column<byte>(type: "tinyint(1)", nullable: false),
                    Vegan = table.Column<byte>(type: "tinyint(1)", nullable: false),
                    Glutenfrei = table.Column<byte>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zutaten", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Bestellungen",
                schema: "emensa",
                columns: table => new
                {
                    Nummer = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    BenutzerNummer = table.Column<int>(type: "int(11)", nullable: false),
                    BestellZeitpunkt = table.Column<DateTime>(nullable: false, defaultValueSql: "current_timestamp()"),
                    Abholzeitpunkt = table.Column<DateTime>(nullable: false, defaultValueSql: "'0000-00-00 00:00:00'"),
                    Endpreis = table.Column<float>(type: "float(10,2)", nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestellungen", x => x.Nummer);
                    table.ForeignKey(
                        name: "c_fkBenutzerNummer",
                        column: x => x.BenutzerNummer,
                        principalSchema: "emensa",
                        principalTable: "Benutzer",
                        principalColumn: "Nummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FH Angehörige",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    fkBenutzer = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FH Angehörige", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkBentuzerFHange",
                        column: x => x.fkBenutzer,
                        principalSchema: "emensa",
                        principalTable: "Benutzer",
                        principalColumn: "Nummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Freunde",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Nutzer = table.Column<int>(type: "int(11)", nullable: false),
                    Freund = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Freunde", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkFreund",
                        column: x => x.Freund,
                        principalSchema: "emensa",
                        principalTable: "Benutzer",
                        principalColumn: "Nummer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "c_fkBenutzer",
                        column: x => x.Nutzer,
                        principalSchema: "emensa",
                        principalTable: "Benutzer",
                        principalColumn: "Nummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gäste",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Grund = table.Column<string>(unicode: false, nullable: false),
                    Ablaufdatum = table.Column<DateTime>(type: "date", nullable: false),
                    fkBenutzer = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gäste", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkBenutzerGast",
                        column: x => x.fkBenutzer,
                        principalSchema: "emensa",
                        principalTable: "Benutzer",
                        principalColumn: "Nummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kategorien",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Bezeichnung = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    fkOberKategorie = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    fkBild = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorien", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkKategorieBild",
                        column: x => x.fkBild,
                        principalSchema: "emensa",
                        principalTable: "Bilder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "c_fkOberKategorie",
                        column: x => x.fkOberKategorie,
                        principalSchema: "emensa",
                        principalTable: "Kategorien",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "gehörtZuFachbereiche",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    fkFachbereiche = table.Column<int>(type: "int(11)", nullable: false),
                    fkFHAnge = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gehörtZuFachbereiche", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkFachbereiche",
                        column: x => x.fkFachbereiche,
                        principalSchema: "emensa",
                        principalTable: "Fachbereiche",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "c_fkFHAnge",
                        column: x => x.fkFHAnge,
                        principalSchema: "emensa",
                        principalTable: "FH Angehörige",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mitarbeiter",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Büro = table.Column<int>(type: "int(11)", nullable: false),
                    Telefon = table.Column<int>(type: "int(11)", nullable: false),
                    fkFHange = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mitarbeiter", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkFHangeMitarb",
                        column: x => x.fkFHange,
                        principalSchema: "emensa",
                        principalTable: "FH Angehörige",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                schema: "emensa",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Studiengang = table.Column<string>(unicode: false, maxLength: 3, nullable: false),
                    Matrikelnummer = table.Column<int>(type: "int(11)", nullable: false),
                    fkFHange = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentID);
                    table.ForeignKey(
                        name: "c_fkFHangeStud",
                        column: x => x.fkFHange,
                        principalSchema: "emensa",
                        principalTable: "FH Angehörige",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mahlzeiten",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Beschreibung = table.Column<string>(unicode: false, nullable: false),
                    Vorrat = table.Column<int>(type: "int(11)", nullable: false),
                    fkKategorie = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    Verfügbar = table.Column<byte>(type: "tinyint(1)", nullable: true, defaultValueSql: "NULL"),
                    Name = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahlzeiten", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkMahlzeitenKategorie",
                        column: x => x.fkKategorie,
                        principalSchema: "emensa",
                        principalTable: "Kategorien",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BestellungEnthältMahlzeit",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Anzahl = table.Column<int>(type: "int(11)", nullable: false),
                    fkMahlzeit = table.Column<int>(type: "int(11)", nullable: false),
                    fkBestellungen = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BestellungEnthältMahlzeit", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkBestellungen",
                        column: x => x.fkBestellungen,
                        principalSchema: "emensa",
                        principalTable: "Bestellungen",
                        principalColumn: "Nummer",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "c_fkMahlzeit",
                        column: x => x.fkMahlzeit,
                        principalSchema: "emensa",
                        principalTable: "Mahlzeiten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kommentare",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    fkStudentID = table.Column<int>(type: "int(11)", nullable: false),
                    fkzuMahlzeit = table.Column<int>(type: "int(11)", nullable: true, defaultValueSql: "NULL"),
                    Bemerkung = table.Column<string>(unicode: false, nullable: true, defaultValueSql: "NULL"),
                    Bewertung = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kommentare", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkStudentID",
                        column: x => x.fkStudentID,
                        principalSchema: "emensa",
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "c_fkzuMahlzeit",
                        column: x => x.fkzuMahlzeit,
                        principalSchema: "emensa",
                        principalTable: "Mahlzeiten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MahlzeitDeklarationen",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    fkDeklaration = table.Column<string>(unicode: false, maxLength: 2, nullable: false),
                    fkMahlzeit = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MahlzeitDeklarationen", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkDeklaration",
                        column: x => x.fkDeklaration,
                        principalSchema: "emensa",
                        principalTable: "Deklarationen",
                        principalColumn: "Zeichen",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "c_fkMahlzeitDeklarationen",
                        column: x => x.fkMahlzeit,
                        principalSchema: "emensa",
                        principalTable: "Mahlzeiten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MahlzeitenBilder",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IDBilder = table.Column<int>(type: "int(11)", nullable: false),
                    IDMahlzeiten = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MahlzeitenBilder", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkBilderMahlzeiten",
                        column: x => x.IDBilder,
                        principalSchema: "emensa",
                        principalTable: "Bilder",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "c_fkMahlzeitenBilder",
                        column: x => x.IDMahlzeiten,
                        principalSchema: "emensa",
                        principalTable: "Mahlzeiten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MahlzeitenZutaten",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    IDZutaten = table.Column<int>(type: "int(5)", nullable: false),
                    IDMahlzeiten = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MahlzeitenZutaten", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkMahlzeitenZutaten",
                        column: x => x.IDMahlzeiten,
                        principalSchema: "emensa",
                        principalTable: "Mahlzeiten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "c_fkZutatenMahlzeiten",
                        column: x => x.IDZutaten,
                        principalSchema: "emensa",
                        principalTable: "Zutaten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preise",
                schema: "emensa",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Jahr = table.Column<int>(type: "int(11)", nullable: false),
                    Gastpreis = table.Column<float>(type: "float(4,2) unsigned", nullable: false),
                    Studentpreis = table.Column<float>(type: "float(4,2) unsigned", nullable: false),
                    MAPreis = table.Column<float>(name: "MA-Preis", type: "float(4,2) unsigned", nullable: false),
                    fkMahlzeiten = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preise", x => x.ID);
                    table.ForeignKey(
                        name: "c_fkPreiseMahlzeiten",
                        column: x => x.fkMahlzeiten,
                        principalSchema: "emensa",
                        principalTable: "Mahlzeiten",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "c_EmailEindeutig",
                schema: "emensa",
                table: "Benutzer",
                column: "E-Mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_NutzernameEindeutig",
                schema: "emensa",
                table: "Benutzer",
                column: "Nutzername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_fkBenutzerNummer",
                schema: "emensa",
                table: "Bestellungen",
                column: "BenutzerNummer");

            migrationBuilder.CreateIndex(
                name: "c_fkBestellungen",
                schema: "emensa",
                table: "BestellungEnthältMahlzeit",
                column: "fkBestellungen");

            migrationBuilder.CreateIndex(
                name: "c_MahlzeitBestellungUnique",
                schema: "emensa",
                table: "BestellungEnthältMahlzeit",
                columns: new[] { "fkMahlzeit", "fkBestellungen" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_fkBentuzerFHange",
                schema: "emensa",
                table: "FH Angehörige",
                column: "fkBenutzer");

            migrationBuilder.CreateIndex(
                name: "c_fkFreund",
                schema: "emensa",
                table: "Freunde",
                column: "Freund");

            migrationBuilder.CreateIndex(
                name: "c_fkBenutzer",
                schema: "emensa",
                table: "Freunde",
                column: "Nutzer");

            migrationBuilder.CreateIndex(
                name: "c_fkBenutzerGast",
                schema: "emensa",
                table: "Gäste",
                column: "fkBenutzer");

            migrationBuilder.CreateIndex(
                name: "c_fkFachbereiche",
                schema: "emensa",
                table: "gehörtZuFachbereiche",
                column: "fkFachbereiche");

            migrationBuilder.CreateIndex(
                name: "c_fkFHAnge",
                schema: "emensa",
                table: "gehörtZuFachbereiche",
                column: "fkFHAnge");

            migrationBuilder.CreateIndex(
                name: "c_fkKategorieBild",
                schema: "emensa",
                table: "Kategorien",
                column: "fkBild");

            migrationBuilder.CreateIndex(
                name: "c_fkOberKategorie",
                schema: "emensa",
                table: "Kategorien",
                column: "fkOberKategorie");

            migrationBuilder.CreateIndex(
                name: "c_fkStudentID",
                schema: "emensa",
                table: "Kommentare",
                column: "fkStudentID");

            migrationBuilder.CreateIndex(
                name: "c_fkzuMahlzeit",
                schema: "emensa",
                table: "Kommentare",
                column: "fkzuMahlzeit");

            migrationBuilder.CreateIndex(
                name: "c_fkMahlzeitDeklarationen",
                schema: "emensa",
                table: "MahlzeitDeklarationen",
                column: "fkMahlzeit");

            migrationBuilder.CreateIndex(
                name: "c_MahlzeitDeklarationenUnique",
                schema: "emensa",
                table: "MahlzeitDeklarationen",
                columns: new[] { "fkDeklaration", "fkMahlzeit" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_fkMahlzeitenKategorie",
                schema: "emensa",
                table: "Mahlzeiten",
                column: "fkKategorie");

            migrationBuilder.CreateIndex(
                name: "Name_UNIQUE",
                schema: "emensa",
                table: "Mahlzeiten",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_fkMahlzeitenBilder",
                schema: "emensa",
                table: "MahlzeitenBilder",
                column: "IDMahlzeiten");

            migrationBuilder.CreateIndex(
                name: "BilderMahlzeitUnique",
                schema: "emensa",
                table: "MahlzeitenBilder",
                columns: new[] { "IDBilder", "IDMahlzeiten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_fkMahlzeitenZutaten",
                schema: "emensa",
                table: "MahlzeitenZutaten",
                column: "IDMahlzeiten");

            migrationBuilder.CreateIndex(
                name: "ZutatProMahlzeit",
                schema: "emensa",
                table: "MahlzeitenZutaten",
                columns: new[] { "IDZutaten", "IDMahlzeiten" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "c_fkFHangeMitarb",
                schema: "emensa",
                table: "Mitarbeiter",
                column: "fkFHange");

            migrationBuilder.CreateIndex(
                name: "c_fkPreiseMahlzeiten",
                schema: "emensa",
                table: "Preise",
                column: "fkMahlzeiten");

            migrationBuilder.CreateIndex(
                name: "c_fkFHangeStud",
                schema: "emensa",
                table: "Student",
                column: "fkFHange");

            migrationBuilder.CreateIndex(
                name: "c_MatrikelnummerUnique",
                schema: "emensa",
                table: "Student",
                column: "Matrikelnummer",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestellungEnthältMahlzeit",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Freunde",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Gäste",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "gehörtZuFachbereiche",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Kommentare",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "MahlzeitDeklarationen",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "MahlzeitenBilder",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "MahlzeitenZutaten",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Mitarbeiter",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Preise",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Bestellungen",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Fachbereiche",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Student",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Deklarationen",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Zutaten",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Mahlzeiten",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "FH Angehörige",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Kategorien",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Benutzer",
                schema: "emensa");

            migrationBuilder.DropTable(
                name: "Bilder",
                schema: "emensa");
        }
    }
}
