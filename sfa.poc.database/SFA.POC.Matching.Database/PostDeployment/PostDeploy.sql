/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--Create roles
--Data Analyst - Read all views excluding HMRC
IF DATABASE_PRINCIPAL_ID('DataAnalyst') IS NULL
BEGIN
	CREATE ROLE [DataAnalyst]
END


--Grant permissions on views to roles
GRANT SELECT ON [dbo].[LocationsView] TO [DataAnalyst]
GRANT SELECT ON [dbo].[ProvideCoursesWithLEPsView] TO [DataAnalyst]


--Set up initial data
:r ".\Seed Courses.sql"
:r ".\Seed Providers.sql"
:r ".\Seed LocalEnterprisePartnerships.sql"
:r ".\Seed District Mapping.sql"
:r ".\Seed Locations.sql"

--Ceate random mapping
:r ".\Randomly Seed ProviderCourseLocations.sql"

--Set up email templates
:r ".\Create Email Templates.sql"
