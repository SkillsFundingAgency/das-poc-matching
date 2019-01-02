
--https://stackoverflow.com/questions/7409051/why-use-the-sql-server-2008-geography-data-type
--https://stackoverflow.com/questions/38728227/system-data-entity-spatial-replacement-in-asp-net-core

--Get a reference to local post code
DECLARE @g geography
SET @g = geography::Point(52.404079, -1.509867, 4326); --CV1 2HJ
--SET @g = geography::Point(51.742141, -1.295653, 4326); --OX2 9GX

declare @milesToMetres float = 1609.34;

--Get distances from local post code
SELECT	[Postcode], 
		[Latitude], 
		[Longitude],
		[Location].STDistance(@g) / 1000 AS [Distance (km)],
		[Location].STDistance(@g) / @milesToMetres AS [Distance (miles)],
		[Location].ToString() AS [Location],
		[Country],
		[Region],
		[AdminDistrict],
		[AdminCounty], 
		[CreatedDate]
FROM [dbo].[Locations] 
ORDER BY [Location].STDistance(@g)

