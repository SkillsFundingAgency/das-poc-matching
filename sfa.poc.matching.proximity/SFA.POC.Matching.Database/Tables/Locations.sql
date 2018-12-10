CREATE TABLE [dbo].[Locations]
(
	[[Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Postcode] VARCHAR(10) NOT NULL,
	[Latitude] [DECIMAL](9, 6) NOT NULL,
	[Longitude] [DECIMAL](9, 6) NOT NULL,
	[Location] [geography] NULL
)
