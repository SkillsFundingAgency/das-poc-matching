CREATE TABLE [dbo].[LocalEnterprisePartnershipDistricts]
(
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[AdminDistrictCode] VARCHAR(10) NULL,
	[Name] NVARCHAR(512) NOT NULL,
	[LocalEnterprisePartnershipId] [BIGINT] NOT NULL,
	[CreatedDate] [DATETIME2] NOT NULL default(getutcdate()) 
)
