CREATE TABLE [dbo].[Ettevote]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Nimi] NVARCHAR(50) NOT NULL, 
    [Registrikood] NVARCHAR(50) NOT NULL, 
    [Kustutatud] BIT NOT NULL
)
