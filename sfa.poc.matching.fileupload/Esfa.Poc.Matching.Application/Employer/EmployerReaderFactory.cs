﻿using System;
using Esfa.Poc.Matching.Application.Common.Interfaces;
using Esfa.Poc.Matching.FileReader.Csv.Employer;
using Esfa.Poc.Matching.FileReader.Excel.Employer;

namespace Esfa.Poc.Matching.Application.Employer
{
    public class EmployerReaderFactory
    {
        public static IEmployerFileReader Create(string fileType)
        {
            switch (fileType)
            {
                case ".csv":
                    return new CsvEmployerFileReader();
                case ".xlsx":
                    return new ExcelEmployerFileReader();
            }

            throw new InvalidOperationException();
        }
    }
}