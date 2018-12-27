/*
Insert initial data for LEPs
*/

IF NOT EXISTS(SELECT 1 FROM [dbo].[LocalEnterprisePartnerships])
BEGIN
	INSERT INTO [dbo].[LocalEnterprisePartnerships]([Name]) 
	VALUES	('Cumbria'),
			('Greater Manchester'),
			('Liverpool City Region'),
			('Cheshire AND Warrington'),
			('Leeds City Region'),
			('Sheffield City Region'),
			('Derby, Derbyshire, Nottingham and Nottinghamshire'),
			('Leicester and Leicestershire'),
			('Greater Birmingham and Solihull'),
			('Coventry and Warwickshire'),
			('The Marches'),
			('Greater Cambridge & Greater Peterborough'),
			('Hertfordshire'),
			('Oxfordshire LEP'),
			('Solent'),
			('West of England'),
			('Cornwall and the Isles of Scilly'),
			('Tees Valley'),
			('Greater Lincolnshire'),
			('South East Midlands'),
			('Thames Valley Berkshire'),
			('South East'),
			('Stoke-on-Trent and Staffordshire'),
			('Coast to Capital'),
			('New Anglia'),
			('Black Country'),
			('Worcestershire'),
			('North Eastern'),
			('York and North Yorkshire'),
			('Enterprise M3'),
			('London'),
			('Heart of the South West'),
			('Lancashire'),
			('Gloucestershire'),
			('Humber'),
			('Dorset'),
			('Swindon and Wiltshire'),
			('Northamptonshire'),
			('Buckinghamshire Thames Valley')
END
