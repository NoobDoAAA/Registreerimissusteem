CREATE VIEW [dbo].[EraisikOsaleja] 
	AS 
	SELECT 
	dbo.Eraisik.Eesnimi, 
	dbo.Eraisik.Perekonnanimi, 
	dbo.Eraisik.Isikukood, 
	dbo.Makseviis.Nimi AS Makseviis, 
	dbo.Osaleja.Lisainfo 
	FROM dbo.Osaleja 
	LEFT JOIN dbo.Eraisik ON dbo.Osaleja.EraisikId = dbo.Eraisik.Id 
	LEFT JOIN dbo.Makseviis ON dbo.Osaleja.MakseviisId = dbo.Makseviis.Id 