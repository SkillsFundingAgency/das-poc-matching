--Clear the provider-course-location mapping then randomly assign some
delete from [dbo].[ProviderCourseLocations]

-- Assumes the ids are in a sequence starting with 1
declare	@courseId BIGINT,
		@locationId BIGINT,
		@providerId BIGINT,
		@maxCourseId BIGINT,
		@maxProviderId BIGINT,
		@maxLocationId BIGINT,
		@numProviders INT,
		@numCourses INT,
		@numLocations INT

SELECT @maxCourseId = MAX([Id]) FROM [dbo].[Courses]
SELECT @maxProviderId = MAX([Id]) FROM [dbo].[Providers]
SELECT @maxLocationId = MAX([Id]) FROM [dbo].[Locations]

DECLARE db_cursor CURSOR FOR 
	SELECT [Id] 
	FROM [dbo].[Providers]

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @providerId

WHILE @@FETCH_STATUS = 0  
BEGIN
	--SET @numProviders int = floor(rand() * 10);
	SET @numCourses = floor(rand() * 25);
	SET @numLocations = floor(rand() * 10);

	WHILE @numCourses > 0
	BEGIN
		SET @numCourses = @numCourses - 1

		SET @courseId = floor(rand() * @maxCourseId);
		SET @locationId = floor(rand() * @maxLocationId);

		INSERT INTO [dbo].[ProviderCourseLocations]
				([ProviderId],
				[CourseId],
				[LocationId])
			SELECT @providerId, @courseId, @locationId
			WHERE NOT EXISTS (SELECT 1 FROM [dbo].[ProviderCourseLocations]
							  WHERE [ProviderId] = @providerId
							  AND	[CourseId] = @courseId
							  AND	[LocationId] = @locationId)
			AND @providerId = (SELECT [Id] FROM [dbo].[Providers] WHERE [Id] = @providerId)
			AND @courseId = (SELECT [Id] FROM [dbo].[Courses] WHERE [Id] = @courseId)
			AND @locationId = (SELECT [Id] FROM [dbo].[Locations] WHERE [Id] = @locationId)
		END

	FETCH NEXT FROM db_cursor INTO @providerId
END 

CLOSE db_cursor  
DEALLOCATE db_cursor

SELECT		pcl.[ProviderId],
			p.[Name] AS [Provider Name],
			pcl.[CourseId],
			c.[Name] AS [Course Name],
			pcl.[LocationId],
			l.[Postcode],
			l.[Country],
			l.[AdminCounty],
			l.[AdminDistrict]
FROM		[dbo].[ProviderCourseLocations] pcl
INNER JOIN	[dbo].[Providers] p
ON			p.[id] = pcl.[ProviderId]
INNER JOIN	[dbo].[Courses] c
ON			c.[id] = pcl.[CourseId]
INNER JOIN	[dbo].[Locations] l
ON			l.[id] = pcl.[LocationId]
