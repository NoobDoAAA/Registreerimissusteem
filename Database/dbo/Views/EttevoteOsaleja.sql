CREATE VIEW [dbo].[EttevoteOsaleja] 
	AS 
	SELECT 
	dbo.Osaleja.Id, 
	dbo.Osaleja.UritusId, 
	dbo.Ettevote.Nimi AS EttevoteNimi, 
	dbo.Ettevote.Registrikood,
	dbo.Makseviis.Id AS MakseviisId, 
	dbo.Makseviis.Nimi AS Makseviis, 
	dbo.Osaleja.OsavotjateArv, 
	dbo.Osaleja.Lisainfo 
	FROM dbo.Osaleja 
	LEFT JOIN dbo.Ettevote ON dbo.Osaleja.EttevoteId = dbo.Ettevote.Id 
	LEFT JOIN dbo.Makseviis ON dbo.Osaleja.MakseviisId = dbo.Makseviis.Id 
	WHERE 
	dbo.Osaleja.Kustutatud = 0 