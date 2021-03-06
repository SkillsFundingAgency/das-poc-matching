﻿using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Esfa.Poc.Matching.Application.Commands
{
    public class CreateEmployerCommand
    {
        private readonly IFileUploadContext _fileUploadContext;

        public CreateEmployerCommand(IFileUploadContext fileUploadContext)
        {
            _fileUploadContext = fileUploadContext;
        }

        public async Task Execute(Entities.Employer employer)
        {
            _fileUploadContext.Employer.Add(employer);

            int createdRecordCount;
            try
            {
                createdRecordCount = await _fileUploadContext.SaveAsync();
            }
            catch (DbUpdateException due)
            {
                // Log
                throw;
            }
        }
    }
}