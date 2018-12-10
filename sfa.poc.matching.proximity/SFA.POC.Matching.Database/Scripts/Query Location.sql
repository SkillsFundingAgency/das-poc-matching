
--https://stackoverflow.com/questions/7409051/why-use-the-sql-server-2008-geography-data-type
--https://stackoverflow.com/questions/38728227/system-data-entity-spatial-replacement-in-asp-net-core

--Get a reference to local post code
DECLARE @g geography
SELECT @g = [Location] 
FROM [dbo].[Locations] 
WHERE [PostCode] = 'CV1 2HJ'

--Get distances from local post code
SELECT	[PostCode], 
		[Latitude], 
		[Longitude],
		[Location].STDistance(@g) / 1000 AS [Distance (km)],
		[Location].STDistance(@g) / 1000 * 5 / 8 AS [Distance (miles)],
		[Location].ToString()
FROM [dbo].[Locations] 
ORDER BY [Location].STDistance(@g)

--Get post codes within 50 miles from local post code
--declare @Longitude float = <somevalue>, @Latitude float = <somevalue>;
--declare @point = geography::Point(@Latitude, @Longitude, 4326);
--declare @distance = <distance in meters>;

declare @distance float = 60 * 1000 * 8 / 5;

SELECT	[PostCode], 
60.0 * 8 / 5,
		[Latitude], 
		[Longitude],
		[Location].STDistance(@g),
		[Location].STDistance(@g) / 1000 AS [Distance (km)],
		[Location].STDistance(@g) / 1000 * 5 / 8 AS [Distance (miles)],
		[Location].ToString()
FROM [dbo].[Locations] 
WHERE [Location].STDistance(@g) <= @distance
--WHERE @g.STDistance([Location]) <= convert(float, 90) --1(60.0 * 8 / 5)
ORDER BY [Location].STDistance(@g)
