CREATE TABLE [dbo].[Osaleja]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UritusId] INT NOT NULL, 
    [EraisikId] INT NULL, 
    [EttevoteId] INT NULL, 
    [OsavotjateArv] INT NULL, 
    [MakseviisId] INT NOT NULL, 
    [Lisainfo] TEXT NULL, 
    CONSTRAINT [FK_Osaleja_Uritus] FOREIGN KEY ([UritusId]) REFERENCES [Uritus]([Id]), 
    CONSTRAINT [FK_Osaleja_Eraisik] FOREIGN KEY ([EraisikId]) REFERENCES [Eraisik]([Id]), 
    CONSTRAINT [FK_Osaleja_Ettevote] FOREIGN KEY ([EttevoteId]) REFERENCES [Ettevote]([Id]), 
    CONSTRAINT [FK_Osaleja_Makseviis] FOREIGN KEY ([MakseviisId]) REFERENCES [Makseviis]([Id]) 
)
