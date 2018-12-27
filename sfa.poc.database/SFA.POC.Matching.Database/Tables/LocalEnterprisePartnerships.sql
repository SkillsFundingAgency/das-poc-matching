CREATE TABLE [dbo].[LocalEnterprisePartnerships]
(
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(512) NOT NULL,
	[CreatedDate] [DateTime2] NOT NULL default(getutcdate()) 
)
