using System;
using System.IO;
using System.Threading.Tasks;
using Esfa.Poc.Matching.Application.Commands;
using Esfa.Poc.Matching.Application.Enums;
using Esfa.Poc.Matching.Common;
using Esfa.Poc.Matching.Common.Interfaces;
using Esfa.Poc.Matching.Entities;

namespace Esfa.Poc.Matching.Application
{
    public class CreateFileUpload
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly CreateFileUploadCommand _createFileUploadCommand;

        public CreateFileUpload(IDateTimeProvider dateTimeProvider, CreateFileUploadCommand createFileUploadCommand)
        {
            _dateTimeProvider = dateTimeProvider;
            _createFileUploadCommand = createFileUploadCommand;
        }

        public async Task<Result> Create(string name, string createdBy)
        {
            var isValid = IsValidFileExtension(name);
            if (!isValid)
            {
                // TODO
            }

            var fileUploadType = GetFileUploadType(name);

            var fileUpload = new FileUpload
            {
                Path = name,
                Type = (int)fileUploadType,
                CreatedOn = _dateTimeProvider.Now(),
                CreatedBy = createdBy
            };

            await _createFileUploadCommand.Execute(fileUpload);

            var success = Result.Ok();

            return success ;
        }

        #region Private Methods

        private static bool IsValidFileExtension(string name)
        {
            var fileExtension = Path.GetExtension(name);
            if (fileExtension == ".csv" || fileExtension == ".xlsx")
                return true;

            return false;
        }

        private static FileUploadType GetFileUploadType(string name)
        {
            if (name.Contains("Employer"))
                return FileUploadType.Employer;

            if (name.Contains("Contact"))
                return FileUploadType.Contact;

            if (name.Contains("Query"))
                return FileUploadType.Query;

            throw new InvalidOperationException();
        }
        #endregion
    }
}