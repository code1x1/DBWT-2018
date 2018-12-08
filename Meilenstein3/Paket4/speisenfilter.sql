SELECT 
    m.ID,
    m.Vorrat,
    k.Bezeichnung AS KName,
    m.`Name`,
    b.`Alt-Text`,
    b.Titel,
    b.ID AS BID,
    b.`Binärdaten`,
    b.Copyright
FROM
    Mahlzeiten AS m
        JOIN
    MahlzeitenBilder mb ON mb.IDMahlzeiten = m.ID
        JOIN
    Bilder b ON b.ID = mb.IDBilder
        JOIN
    Kategorien k ON k.ID = m.fkKategorie
        LEFT JOIN
    MahlzeitenZutaten mz ON mz.IDMahlzeiten = m.ID
        LEFT JOIN
    Zutaten z ON z.ID = mz.IDZutaten
WHERE
    k.ID = @filter
        OR @filter IS NULL
        AND z.Vegetarisch = @vegetarisch
        OR @vegetarisch IS NULL
        AND z.Vegan = @vegan
        OR @vegan IS NULL
        AND m.Verfügbar = @verfugbar
        OR @verfugbar IS NULL
LIMIT 8