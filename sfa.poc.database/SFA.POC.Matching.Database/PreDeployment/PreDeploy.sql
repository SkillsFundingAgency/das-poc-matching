/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--Old version of table has a very badly named column - just throw the data away to let the scripts run and fix it
IF EXISTS (select * from sys.columns c 
		   where c.name = '[Id' 
		   and c.object_id = (select object_id FROM sys.tables 
							  WHERE name = 'Locations' 
							  AND schema_id =  SCHEMA_ID('dbo')))
BEGIN
	DELETE FROM [dbo].[Locations]
END


