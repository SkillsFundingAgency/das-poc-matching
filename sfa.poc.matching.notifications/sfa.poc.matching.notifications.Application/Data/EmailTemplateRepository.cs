using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using sfa.poc.matching.notifications.Application.Entities;
using sfa.poc.matching.notifications.Application.Interfaces;

namespace sfa.poc.matching.notifications.Application.Data
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IMatchingConfiguration _config;

        public EmailTemplateRepository(IMatchingConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateName)
        {
            using (var connection = new SqlConnection(_config.SqlConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                var sql =
                    "SELECT * " +
                    "FROM [EmailTemplates] " +
                    "WHERE TemplateName LIKE @templateName " +
                    "AND Status = 'Live'";

                var emailTemplates = await connection.QueryAsync<EmailTemplate>(sql, new { templateName });
                return emailTemplates.FirstOrDefault();
            }
        }
    }
}
