
--https://stackoverflow.com/questions/7409051/why-use-the-sql-server-2008-geography-data-type
--https://stackoverflow.com/questions/38728227/system-data-entity-spatial-replacement-in-asp-net-core

--Get a reference to local post code
DECLARE @g geography
--SELECT @g = [Location] 
--FROM [dbo].[Locations] 
--WHERE [PostCode] = 'CV1 2HJ'
SET @g = geography::Point(52.404079, -1.509867, 4326); --CV1 2HJ
--SET @g = geography::Point(51.742141, -1.295653, 4326); --OX2 9GX

--Get post codes within 25 miles from local post code
declare @radiusInMiles float = 25;
declare @milesToMetres float = 1609.34;
declare @distance float = @radiusInMiles * @milesToMetres;

SELECT	[PostCode], 
		[Latitude], 
		[Longitude],
		--[Location].STDistance(@g),
		--[Location].STDistance(@g) / 1000 AS [Distance (km)],
		[Location].STDistance(@g) / @milesToMetres AS [Distance (miles)],
		--[Location].ToString(),
		[Country],
		[Region],
		[AdminDistrict],
		[AdminCounty], 
		[CreatedDate]
FROM [dbo].[Locations] 
WHERE [Location].STDistance(@g) <= @distance
--WHERE @g.STDistance([Location]) <= convert(float, 90) --1(60.0 * 8 / 5)
--AND		[Country] = 'England'
ORDER BY [Location].STDistance(@g)
