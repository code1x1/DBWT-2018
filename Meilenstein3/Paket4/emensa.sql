use emensa;

-- DROP STATEMENTS

-- DROP TRIGGER IF EXISTS `Mahlzeiten_verfügbar_update`;
-- DROP TRIGGER IF EXISTS `Mahlzeiten_verfügbar_insert`;

DROP TABLE IF EXISTS `MahlzeitenBilder`;
DROP TABLE IF EXISTS `MahlzeitDeklarationen`;
DROP TABLE IF EXISTS `gehörtZuFachbereiche`;
DROP TABLE IF EXISTS `MahlzeitenZutaten`;
DROP TABLE IF EXISTS `Deklarationen`;
DROP TABLE IF EXISTS `Kommentare`;
DROP TABLE IF EXISTS `Fachbereiche`;
DROP TABLE IF EXISTS `Zutaten`;
DROP TABLE IF EXISTS `Preise`;
DROP TABLE IF EXISTS `BestellungEnthältMahlzeit`;
DROP TABLE IF EXISTS `Mahlzeiten`;
DROP TABLE IF EXISTS `Kategorien`;
DROP TABLE IF EXISTS `Bestellungen`;
DROP TABLE IF EXISTS `Mitarbeiter`;
DROP TABLE IF EXISTS `Bilder`;
DROP TABLE IF EXISTS `Student`;
DROP TABLE IF EXISTS `Gäste`;
DROP TABLE IF EXISTS `FH Angehörige`;
DROP TABLE IF EXISTS `Benutzer`;

-- CREATE STATEMENTS

CREATE TABLE `Benutzer` (
	Nummer INT NOT NULL AUTO_INCREMENT,
    Nutzername VARCHAR(150) NOT NULL,
    LetzerLogin TIMESTAMP NULL DEFAULT NULL,
    `E-Mail` VARCHAR(150) NOT NULL,
    Salt VARCHAR(32) NOT NULL,
    `Hash` VARCHAR(24) NOT NULL,
    AnlegeDatum DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP(),
    Aktiv BOOL NOT NULL,
    Vorname VARCHAR(100) NOT NULL,
    Nachname VARCHAR(100) NOT NULL,
    Geburtsdatum DATE, 
    CONSTRAINT c_pkBenutzer PRIMARY KEY(Nummer),
    CONSTRAINT c_EmailEindeutig UNIQUE(`E-Mail`),
    CONSTRAINT c_NutzernameEindeutig UNIQUE(Nutzername)
);

CREATE TABLE `FH Angehörige`(
	ID INT NOT NULL AUTO_INCREMENT,
    fkBenutzer INT NOT NULL,
    CONSTRAINT c_fkBentuzerFHange FOREIGN KEY(fkBenutzer) 
		REFERENCES Benutzer(Nummer) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT c_pkFhAnge PRIMARY KEY(ID)
);

CREATE TABLE `Gäste`(
	Grund TEXT NOT NULL,
	Ablaufdatum DATE NOT NULL,
    fkBenutzer INT NOT NULL,
    CONSTRAINT c_fkBenutzerGast FOREIGN KEY(fkBenutzer) 
		REFERENCES Benutzer(Nummer) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE `Student`(
	StudentID INT NOT NULL AUTO_INCREMENT,
	Studiengang ENUM('ET','INF','ISE','MCD','WI') NOT NULL,
    Matrikelnummer INT NOT NULL,
    CONSTRAINT c_MatrikelnummerRange 
		CHECK(Matrikelnummer >= 10000000 AND Matrikelnummer <= 999999999),
    CONSTRAINT c_MatrikelnummerUnique UNIQUE(Matrikelnummer),
	fkFHange INT NOT NULL,
    CONSTRAINT c_fkFHangeStud FOREIGN KEY(fkFHange) 
		REFERENCES `FH Angehörige`(ID) ON DELETE CASCADE ON UPDATE CASCADE ,
	CONSTRAINT c_pkStudentID PRIMARY KEY(StudentID)
);

CREATE TABLE `Mitarbeiter`(
	Büro INT NOT NULL,
    Telefon INT NOT NULL,
    fkFHange INT NOT NULL,
    CONSTRAINT c_fkFHangeMitarb FOREIGN KEY(fkFHange) 
		REFERENCES `FH Angehörige`(ID) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE `Bestellungen`(
	Nummer INT NOT NULL AUTO_INCREMENT,
    BestellZeitpunkt TIMESTAMP NOT NULL DEFAULT NOW(),
    Abholzeitpunkt TIMESTAMP,
    CONSTRAINT c_AbholzeitBestellzeit 
		CHECK(Abholzeitpunkt > BestellZeitpunkt OR Abholzeitpunkt = NULL),
    CONSTRAINT c_pkBestellungen PRIMARY KEY(Nummer)
);

CREATE TABLE `Bilder`(
	ID INT NOT NULL AUTO_INCREMENT,
    `Alt-Text` TEXT NOT NULL,
    Titel VARCHAR(150),
    `Binärdaten` LONGBLOB NOT NULL,
    CONSTRAINT c_pkBilder PRIMARY KEY(ID)
);

CREATE TABLE `Kategorien`(
	ID INT NOT NULL AUTO_INCREMENT,
    Bezeichnung VARCHAR(150) NOT NULL,
    fkOberKategorie INT,
    fkBild INT,
    CONSTRAINT c_fkKategorieBild FOREIGN KEY(fkBild) 
		REFERENCES `Bilder`(ID) ON DELETE SET NULL,
    CONSTRAINT c_fkOberKategorie FOREIGN KEY(fkOberKategorie) 
		REFERENCES `Kategorien`(ID) ON DELETE SET NULL,
	CONSTRAINT c_pkKategorien PRIMARY KEY(ID)
);

CREATE TABLE `Mahlzeiten`(
	ID INT NOT NULL AUTO_INCREMENT,
    Beschreibung TEXT NOT NULL,
    Vorrat INT NOT NULL,
    fkKategorie INT,
    CONSTRAINT c_fkMahlzeitenKategorie FOREIGN KEY(fkKategorie) 
		REFERENCES `Kategorien`(ID),
    CONSTRAINT c_pkMahlzeiten PRIMARY KEY(ID)
);

CREATE TABLE `MahlzeitenBilder`(
	IDBilder INT NOT NULL,
    CONSTRAINT c_fkBilderMahlzeiten FOREIGN KEY(IDBilder) 
		REFERENCES `Bilder`(ID) ON DELETE CASCADE,
	IDMahlzeiten INT NOT NULL,
    CONSTRAINT c_fkMahlzeitenBilder FOREIGN KEY(IDMahlzeiten) 
		REFERENCES `Mahlzeiten`(ID) ON DELETE CASCADE,
    CONSTRAINT BilderMahlzeitUnique UNIQUE(IDBilder,IDMahlzeiten)    
);

CREATE TABLE `BestellungEnthältMahlzeit`(
	Anzahl INT NOT NULL,
    fkMahlzeit INT NOT NULL,
    CONSTRAINT c_fkMahlzeit FOREIGN KEY(fkMahlzeit) 
		REFERENCES `Mahlzeiten`(ID),
    fkBestellungen INT NOT NULL,
    CONSTRAINT c_fkBestellungen 
		FOREIGN KEY(fkBestellungen) REFERENCES `Bestellungen`(Nummer),
	CONSTRAINT c_MahlzeitBestellungUnique UNIQUE(fkMahlzeit,fkBestellungen)
);

CREATE TABLE `Preise`(
	Jahr INT NOT NULL,
    Gastpreis FLOAT (4,2) UNSIGNED NOT NULL,
    Studentpreis FLOAT (4,2) UNSIGNED NOT NULL,
    `MA-Preis` FLOAT (4,2) UNSIGNED NOT NULL,
    fkMahlzeiten INT NOT NULL,
    CONSTRAINT c_fkPreiseMahlzeiten FOREIGN KEY(fkMahlzeiten) 
		REFERENCES Mahlzeiten(ID) ON DELETE CASCADE,
    CONSTRAINT c_GastpreisLimit CHECK(Gastpreis < 99.99),
    CONSTRAINT c_StudentpreisLimit CHECK(Studentpreis < 99.99),
    CONSTRAINT c_MApreisLimit CHECK(`MA-Preis` < 99.99),
    CONSTRAINT c_StudentMitarbeiterPreise CHECK(Studentpreis < `MA-Preis`)
);

CREATE TABLE `Zutaten`(
	ID INT(5) NOT NULL,
    `Name` VARCHAR(100) NOT NULL,
    Bio BOOL NOT NULL,
    Vegetarisch BOOL NOT NULL,
    Vegan BOOL NOT NULL,
    Glutenfrei BOOL NOT NULL,
    CONSTRAINT c_pkZutaten PRIMARY KEY(ID),
    CONSTRAINT c_ZutatenIDRange CHECK(ID>=10000 AND ID<=99999)
);

CREATE TABLE `MahlzeitenZutaten`(
	IDZutaten INT(5) NOT NULL,
    CONSTRAINT c_fkZutatenMahlzeiten FOREIGN KEY(IDZutaten) 
		REFERENCES `Zutaten`(ID) ON DELETE CASCADE,
	IDMahlzeiten INT NOT NULL,
    CONSTRAINT c_fkMahlzeitenZutaten FOREIGN KEY(IDMahlzeiten) 
		REFERENCES `Mahlzeiten`(ID) ON DELETE CASCADE,
    CONSTRAINT ZutatProMahlzeit UNIQUE(IDZutaten,IDMahlzeiten)
);

CREATE TABLE `Fachbereiche`(
	ID INT NOT NULL AUTO_INCREMENT,
    `Name` VARCHAR(100) NOT NULL,
    Website VARCHAR(250),
    CONSTRAINT c_pkFachbereiche PRIMARY KEY(ID)
);

CREATE TABLE `gehörtZuFachbereiche`(
	fkFachbereiche INT NOT NULL,
    fkFHAnge INT NOT NULL,
    CONSTRAINT c_fkFachbereiche FOREIGN KEY(fkFachbereiche) 
		REFERENCES `Fachbereiche`(ID) ON DELETE CASCADE,
    CONSTRAINT c_fkFHAnge FOREIGN KEY(fkFHAnge) 
		REFERENCES `FH Angehörige`(ID) ON DELETE CASCADE
);

CREATE TABLE `Kommentare`(
	ID INT NOT NULL AUTO_INCREMENT,
    fkStudentID INT NOT NULL,
    fkzuMahlzeit INT,
    Bemerkung TEXT,
    Bewertung INT NOT NULL,
	CONSTRAINT c_pkKommentare PRIMARY KEY(ID),
	CONSTRAINT c_fkStudentID FOREIGN KEY(fkStudentID) 
		REFERENCES `Student`(StudentID),
	CONSTRAINT c_fkzuMahlzeit FOREIGN KEY(fkzuMahlzeit) 
		REFERENCES `Mahlzeiten`(ID)ON DELETE SET NULL
);

CREATE TABLE `Deklarationen`(
	Zeichen VARCHAR(2) NOT NULL,
    Beschriftung VARCHAR(32),
    CONSTRAINT c_pkDeklarationen PRIMARY KEY(Zeichen)
);

CREATE TABLE `MahlzeitDeklarationen`(
	fkDeklaration VARCHAR(2) NOT NULL,
    CONSTRAINT c_fkDeklaration FOREIGN KEY(fkDeklaration) 
		REFERENCES `Deklarationen`(Zeichen),
	fkMahlzeit INT NOT NULL,
    CONSTRAINT c_fkMahlzeitDeklarationen FOREIGN KEY(fkMahlzeit) 
		REFERENCES `Mahlzeiten`(ID),
	CONSTRAINT c_MahlzeitDeklarationenUnique UNIQUE(fkDeklaration,fkMahlzeit)
);

ALTER TABLE `Mahlzeiten` 
ADD COLUMN `Name` VARCHAR(100) NOT NULL AFTER `fkKategorie`,
ADD COLUMN `Verfügbar` BOOL AFTER `fkKategorie`,
ADD UNIQUE INDEX `Name_UNIQUE` (`Name` ASC);

ALTER TABLE `Benutzer` 
ADD COLUMN `Alter` INT AS (YEAR(NOW()) - YEAR(Geburtsdatum)) 
VIRTUAL AFTER Geburtsdatum;

ALTER TABLE `Bestellungen`
ADD COLUMN `Endpreis` FLOAT(10,2);

ALTER TABLE `Bilder`
ADD COLUMN `Copyright` VARCHAR(300);

-- DELIMITER //
-- CREATE TRIGGER `Mahlzeiten_verfügbar_insert`
--   AFTER INSERT ON `Mahlzeiten` FOR EACH ROW 
-- BEGIN
--  IF NEW.`Vorrat` > 0 THEN
--   UPDATE `Mahlzeiten` SET `Verfügbar` = TRUE;
--  ELSE 
--   UPDATE `Mahlzeiten` SET `Verfügbar` = FALSE;
--  END IF;
-- END 
-- // 
-- DELIMITER ;
-- 
-- DELIMITER //
-- CREATE TRIGGER `Mahlzeiten_verfügbar_update`
--   AFTER UPDATE ON `Mahlzeiten` FOR EACH ROW 
-- BEGIN
--  IF NEW.`Vorrat` > 0 THEN
--   UPDATE `Mahlzeiten` SET `Verfügbar` = TRUE;
--  ELSE 
--   UPDATE `Mahlzeiten` SET `Verfügbar` = FALSE;
--  END IF;
-- END
-- // 
-- DELIMITER ;


-- INSERT STATEMENTS

INSERT INTO `Benutzer` 
(`Nutzername`, `E-Mail`, `Salt`, `Hash`, `Aktiv`, `Vorname`, `Nachname`, `Geburtsdatum`) 
VALUES 
('db9382s', 'db@test.de', '1', '2', '1', 'denis', 'behrends', '1990-12-17'),
('db9383s', 'db2@test.de', '1', '2', '1', 'denis2', 'behrends', '1990-12-17'),
('db9384s', 'db3@test.de', '1', '2', '1', 'denis3', 'behrends', '1990-12-17'),
('db9385s', 'db4@test.de', '1', '2', '1', 'denis4', 'behrends', '1990-12-17');

INSERT INTO `FH Angehörige` (`fkBenutzer`) 
VALUES ('1'),('2'),('3');

INSERT INTO `Student` (`Studiengang`, `Matrikelnummer`, `fkFHange`) VALUES 
('INF', '45032620', '1'),
('ET', '422003210', '2');

INSERT INTO `emensa`.`Mitarbeiter` (`Büro`, `Telefon`, `fkFHange`) VALUES ('222', '1231423', '3');

DELETE FROM `emensa`.`Benutzer` WHERE Nummer=1;


INSERT INTO `Zutaten` VALUES 
('10000', 'Amaranth', '1', '1', '1', '1'),
('10001', 'Champignons', '1', '1', '1', '1'),
('10002', 'Fenchel', '1', '1', '1', '1'),
('10003', 'Hanfmehl', '1', '1', '1', '1'),
('10004', 'Heilbutt', '1', '0', '0', '1'),
('10005', 'Kurkumin', '1', '1', '1', '1'),
('10006', 'Lachs', '1', '0', '0', '1'),
('10007', 'Paprika', '1', '1', '1', '1'),
('10008', 'Sojasprossen', '1', '1', '1', '1'),
('10009', 'Weizenmehl', '1', '1', '1', '0'),
('10010', 'Zitronensäure', '1', '1', '1', '1'),
('10011', 'Aal', '0', '0', '0', '1'),
('10012', 'Alginat', '0', '1', '1', '1'),
('10013', 'Barsch', '0', '0', '0', '1'),
('10014', 'Brantweinessig', '0', '1', '1', '1');

INSERT INTO `Kategorien`(`Bezeichnung`, `fkOberKategorie`) VALUES 
('Gerichte',NULL)
,('Getränke',NULL)
,('Fast Food', '1')
,('Salat', '1')
,('Saft', '2')
,('Softdrinks', '2')
,('Fleisch', '1')
,('Fisch', '1')
,('Gemüse', '1')
,('Smoothie', '2');

INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('1', 'Steak in BBQ Sauce und Salat', 'Steak', '', 'https://unsplash.com/photos/auIbTAcSH6E');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('2', 'Erdbeer Smoothie mit fein geschnittenem Obst', 'Erdbeer Smoothie', '', 'https://unsplash.com/photos/nkHBFwVBzkg');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('3', 'Pommes mit Speck und Gemüse Käse überbacken', 'Pommes mit Speck', '', 'https://unsplash.com/photos/R18ecx07b3c');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('4', 'Leckere Teigtasche mit Fleisch und Gemüse', 'Rind Gemüse Tacco', '', 'https://unsplash.com/photos/vzX2rgUbQXM');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('5', 'BBQ Burger mit Gemüse', 'BBQ Burger', '', 'https://unsplash.com/photos/rbcvIrxw6KA');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('6', 'Pfannkuchen mit Erdbeeren', 'Pfannkuchen', '', 'https://unsplash.com/photos/GuvimT4IFok');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('7', 'Himbeersorbet', 'Himbeereis', '', 'https://unsplash.com/photos/MXovqM130UI');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('8', 'Nudelsuppe mit Gemüse', 'Nudelsuppe', '', 'https://unsplash.com/photos/WBX-ZLr8P7I');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('9', 'Rotebete Pizza mit Avocado', 'Rotebete Pizza', '', 'https://unsplash.com/photos/smN1dzUTj9Y');
INSERT INTO `emensa`.`Bilder` (`ID`, `Alt-Text`, `Titel`, `Binärdaten`, `Copyright`) VALUES ('10', 'Salami Pizza mit Kartoffeln und Gemüse', 'Salami Pizza', '', 'https://unsplash.com/photos/MNtag_eXMKw');



INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Steak mit BBQ Sauce', '5', '7', 'Steak');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Erdbeersmoothie mit frischen Früchten', '2', '10', 'Erdbeer Smoothie');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Bacon Pommes mit leckerer Sauce überbacken', '100', '1', 'Bacon Pommes');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Tacco mit Gemüse und Rindfleisch', '0', '1', 'Tacco');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Chicken Burger mit BBQ Sauce', '0', '1', 'Burger');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Pfannkuchen mit Ahornsirup', '50', '1', 'Pfannkuchen');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Himbeereis am Stil', '32', '1', 'Himbeereis');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Nudelsuppe mit Gemüse', '300', '1', 'Nudelsuppe');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Rotebetepizza mit Avocado besticht durch seine außergewöhnliche Farbe', '24', '1', 'Rotebetepizza');
INSERT INTO `emensa`.`Mahlzeiten` (`Beschreibung`, `Vorrat`, `fkKategorie`, `Name`) VALUES ('Salamipizza mit Käse und Gemüse', '3', '1', 'Salamipizza');

UPDATE `emensa`.`Mahlzeiten` SET `Vorrat`='8', `Verfügbar`='1' WHERE `ID`='3';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='4';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='5';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='0' WHERE `ID`='6';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='0' WHERE `ID`='7';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='8';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='9';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='10';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='11';
UPDATE `emensa`.`Mahlzeiten` SET `Verfügbar`='1' WHERE `ID`='12';


INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('1', '1');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('2', '2');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('3', '3');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('4', '4');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('5', '5');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('6', '6');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('7', '7');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('8', '8');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('9', '9');
INSERT INTO `emensa`.`MahlzeitenBilder` (`IDBilder`, `IDMahlzeiten`) VALUES ('10', '10');

INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '5.99', '2', '3', '1');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '8.99', '3', '7', '2');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '7.69', '4', '7', '3');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '14', '8', '12', '4');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '17', '5', '13', '5');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '13.99', '2', '10', '6');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '19.99', '10', '14', '7');
INSERT INTO `emensa`.`Preise` (`Jahr`, `Gastpreis`, `Studentpreis`, `MA-Preis`, `fkMahlzeiten`) VALUES ('2018', '6', '4', '5', '8');
