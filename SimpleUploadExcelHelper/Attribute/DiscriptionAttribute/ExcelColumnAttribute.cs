using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;

namespace SimpleUploadExcelHelper
{
    public class ExcelColumnAttribute : Attribute
    {
        public ExcelColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        public string ColumnName{get; private set; }
    }
}
