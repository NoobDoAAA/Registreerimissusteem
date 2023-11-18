CREATE TABLE [dbo].[Eraisik]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Eesnimi] NVARCHAR(50) NOT NULL, 
    [Perekonnanimi] NVARCHAR(50) NOT NULL, 
    [Isikukood] NCHAR(11) NOT NULL
)
