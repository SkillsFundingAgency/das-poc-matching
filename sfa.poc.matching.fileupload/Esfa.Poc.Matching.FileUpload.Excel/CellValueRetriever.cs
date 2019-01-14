using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Esfa.Poc.Matching.FileReader.Excel
{
    internal class CellValueRetriever
    {
        internal static string Get(SpreadsheetDocument document, CellType cell)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var cellValue = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(cellValue)].InnerText;
            }

            return cellValue;
        }
    }
}