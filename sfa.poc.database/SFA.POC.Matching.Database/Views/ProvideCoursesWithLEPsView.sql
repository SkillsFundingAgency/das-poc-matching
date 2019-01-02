CREATE VIEW [dbo].[ProvideCoursesWithLEPsView]
	AS SELECT		pcl.[ProviderId],
					p.[Name] AS [ProviderName],
					pcl.[CourseId],
					c.[Name] AS [CourseName],
					pcl.[LocationId],
					l.[Postcode],
					l.[Country],
					l.[AdminCounty],
					l.[AdminDistrict],
					l.[AdminDistrictCode],
					lep.[Name] AS [LEP]
		FROM		[dbo].[ProviderCourseLocations]	pcl
		INNER JOIN	[dbo].[Providers] p
		ON			p.[Id] = pcl.[ProviderId]
		INNER JOIN	[dbo].[Courses] c
		ON			c.[Id] = pcl.[CourseId]
		INNER JOIN	[dbo].[Locations] l
		ON			l.[Id] = pcl.[LocationId]
		LEFT JOIN	[dbo].[LocalEnterprisePartnershipDistricts] lepd
		ON			lepd.[AdminDistrictCode] = l.[AdminDistrictCode]
		LEFT JOIN	[dbo].[LocalEnterprisePartnerships] lep
		ON			lep.Id = lepd.[LocalEnterprisePartnershipId]
		WHERE		l.[Country] = 'England'
