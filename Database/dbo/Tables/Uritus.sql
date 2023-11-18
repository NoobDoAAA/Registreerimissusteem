CREATE TABLE [dbo].[Uritus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nimi] NVARCHAR(50) NOT NULL, 
    [Toimumisaeg] DATETIME NOT NULL, 
    [ToimumiseKoht] NVARCHAR(100) NOT NULL, 
    [Lisainfo] TEXT NULL, 
    [Kustutatud] BIT NOT NULL
)
