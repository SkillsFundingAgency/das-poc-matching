﻿using System;

namespace Esfa.Poc.Matching.FileReader.Excel
{
    public static class StringExtensions
    {
        public static DateTime ToDate(this string oaDate)
        {
            var date = DateTime.FromOADate(Convert.ToDouble(oaDate));

            return date;
        }
    }
}