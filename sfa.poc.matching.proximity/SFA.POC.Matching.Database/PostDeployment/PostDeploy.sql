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

--Insert test data for known post codes
IF NOT EXISTS(SELECT * FROM [dbo].[Locations])
BEGIN
	INSERT INTO [dbo].[Locations] ([Postcode], [Latitude], [Longitude])
	VALUES ('OX2 9GX', 51.742141, -1.295653),
		   ('CV1 2HJ', 52.404079, -1.509867),
		   ('OX49 5NU', 51.656146, -1.069849),
		   ('M32 0JG', 53.455654, -2.302836),
		   ('NE30 1DP', 55.011303, -1.439269)

	UPDATE	[dbo].[Locations] 
	SET		[Location] = geography::Point([Latitude], [Longitude], 4326);
END
GO

