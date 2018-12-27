CREATE VIEW [dbo].[LocationsView]
	AS 	SELECT	[Postcode], 
		[Latitude], 
		[Longitude],
		[Location].ToString() AS [Location],
		[Country],
		[Region],
		[AdminDistrictCode],
		[AdminDistrict],
		[AdminCounty], 
		[CreatedDate]
FROM [dbo].[Locations] 
