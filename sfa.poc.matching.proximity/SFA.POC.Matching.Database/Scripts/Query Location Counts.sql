--Get counts
SELECT		COUNT(1) AS [Total Locations]
FROM		[dbo].[Locations] 

SELECT		[Country], 
			COUNT([Country]) AS [Countries]
FROM		[dbo].[Locations] 
GROUP BY	[Country]
ORDER BY	[Country]

SELECT		[Country],
			COUNT([Country]) AS [Countries],
			[Region], 
			COUNT([Region]) AS [Regions]
FROM		[dbo].[Locations] 
GROUP BY [Country], [Region]
ORDER BY [Country], [Region]

