CREATE TABLE [dbo].[Courses]
(
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(512) NOT NULL,
	[Description] [NVARCHAR](MAX) NULL,
	[CreatedDate] [DateTime2] NOT NULL default(getutcdate()) 
)
