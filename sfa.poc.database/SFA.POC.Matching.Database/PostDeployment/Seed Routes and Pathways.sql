/*
Insert initial data for Routes and Pathways
*/

IF NOT EXISTS(SELECT 1 FROM [dbo].[Routes])
BEGIN
	INSERT INTO [dbo].[Routes]([Name])
	VALUES	('Agriculture, Environmental and Animal Care'),
			('Business and Administration'),
			('Catering and Hospitality'),
			('Construction'),
			('Creative and Design'),
			('Digital'),
			('Education & Childcare'),
			('Engineering and Manufacturing'),
			('Hair and Beauty'),
			('Health and Science' ),
			('Legal, Financial and Accounting')
END

IF NOT EXISTS(SELECT 1 FROM [dbo].[Pathways])
BEGIN
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Agriculture, Land Management and Production', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Agriculture, Environmental and Animal Care' 
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Animal are and Management', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Agriculture, Environmental and Animal Care'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Human Resources', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Business and Administration'

	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Management and Administration', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Business and Administration'

	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Catering', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Catering and Hospitality'

	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Building Services Engineering', [Id] 
	FROM [dbo].[Routes] WHERE [Name] = 'Construction'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 	
	SELECT 'Design, Surveying & Planning', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Construction'

	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Onsite Construction', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Construction'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Craft and Design', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Creative and Design'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Cultural Heritage and Visitor Attractions', [Id] 
	FROM [dbo].[Routes]
	WHERE [Name] = 'Creative and Design'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Music, Broadcast and Production', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Creative and Design'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Data and Digital Business Services', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Digital'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'IT Support and Services', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Digital'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Software and Applications Design and Development', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Digital'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Education', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Education & Childcare'

	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Design, Development and Control', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Engineering and Manufacturing'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Maintenance, Installation and Repair', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Engineering and Manufacturing'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Manufacturing and Process', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Engineering and Manufacturing'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Hair, Beauty and Aesthetics', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Hair and Beauty'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Health', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Health and Science'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Healthcare Science', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Health and Science'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Science', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Health and Science'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Accounting', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Legal, Financial and Accounting'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Financial', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Legal, Financial and Accounting'
	
	INSERT INTO [dbo].[Pathways]([Name], [RouteId]) 
	SELECT 'Legal', [Id] 
	FROM [dbo].[Routes] 
	WHERE [Name] = 'Legal, Financial and Accounting'

END