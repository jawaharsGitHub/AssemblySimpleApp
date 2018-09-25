using System;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public static class FormGeneral
    {

        public static string GetGridCellValue(DataGridView grid, int rowIndex, string columnName)
        {
            var cellValue = Convert.ToString(grid.Rows[grid.CurrentCell.RowIndex].Cells[columnName].Value);
            return (cellValue == string.Empty) ? null : cellValue;
        }

    }
}
