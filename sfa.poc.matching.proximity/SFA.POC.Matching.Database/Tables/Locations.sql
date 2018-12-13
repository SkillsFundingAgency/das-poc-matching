CREATE TABLE [dbo].[Locations]
(
	[[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Postcode] VARCHAR(10) NOT NULL,
	[Latitude] [DECIMAL](9, 6) NOT NULL,
	[Longitude] [DECIMAL](9, 6) NOT NULL,
	[Location] [geography] NULL,
	[Country] NVARCHAR(50) NULL,
	[Region] NVARCHAR(200) NULL,
	[AdminDistrict] NVARCHAR(200) NULL,
	[AdminCounty] NVARCHAR(200) NULL, 
	[CreatedDate] [DateTime2] NOT NULL default(getdate()) 
)
