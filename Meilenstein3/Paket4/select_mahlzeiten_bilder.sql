use emensa;
SELECT m.ID, z.`Name` AS ZutatenName, m.Beschreibung, m.`Name`, b.`Alt-Text`, b.Titel, b.`Binärdaten`, b.Copyright FROM Mahlzeiten AS m 
JOIN MahlzeitenBilder mb ON mb.IDMahlzeiten=m.ID
JOIN Bilder b ON b.ID=mb.IDBilder
JOIN MahlzeitenZutaten mz ON mz.IDMahlzeiten=m.ID
JOIN Zutaten z ON z.ID=mz.IDZutaten
WHERE m.ID=1;