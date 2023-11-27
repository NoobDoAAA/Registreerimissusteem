/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select * from dbo.Makseviis where Nimi = 'Pangaülekanne')
begin
    INSERT INTO dbo.Makseviis (Nimi, Aktiivne) VALUES ('Pangaülekanne', 1)
end

if not exists (select * from dbo.Makseviis where Nimi = 'Sularaha')
begin
    INSERT INTO dbo.Makseviis (Nimi, Aktiivne) VALUES ('Sularaha', 1)
end