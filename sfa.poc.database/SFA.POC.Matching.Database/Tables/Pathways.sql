CREATE TABLE [dbo].[Pathways]
(
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(50) NOT NULL,
	[RouteId] [BIGINT] NOT NULL
)
GO
ALTER TABLE [dbo].[Pathways]
	ADD CONSTRAINT FK_Pathways_RouteId 
	FOREIGN KEY ([RouteId]) 
	REFERENCES [dbo].[Routes] ([Id])
