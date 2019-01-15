/*
Set up email templates. Initial version is a copy of the templates for Apply Web.

Recipient email addresses have been removed from the script.
*/


IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = 'ApplySignupError')
	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
	VALUES (NEWID(), 'Live', 'ApplySignupError', 'ApplySignupError', '', GETDATE(), 'System')

IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = 'VacancyService_CandidateContactUsMessage')
	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
	VALUES (NEWID(), 'Live', 'VacancyService_CandidateContactUsMessage', 'VacancyService_CandidateContactUsMessage', '', GETDATE(), 'System')

IF NOT EXISTS( SELECT * FROM [dbo].[EmailTemplates] WHERE [TemplateId] = '192e704d-a5db-4f77-9b20-4eb9cdff5501')
	INSERT INTO [dbo].[EmailTemplates] ([Id], [Status], [TemplateName], [TemplateId], [Recipients], [CreatedAt], [CreatedBy])
	VALUES (NEWID(), 'Live', 'Test_Template', '192e704d-a5db-4f77-9b20-4eb9cdff5501', '', GETDATE(), 'System')

GO
