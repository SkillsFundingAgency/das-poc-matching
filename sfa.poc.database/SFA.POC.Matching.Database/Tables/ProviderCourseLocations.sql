CREATE TABLE [dbo].[ProviderCourseLocations]
(
	[Id] [BIGINT] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ProviderId] [BIGINT] NOT NULL,
	[CourseId] [BIGINT] NOT NULL,
	[LocationId] [BIGINT] NOT NULL
)
