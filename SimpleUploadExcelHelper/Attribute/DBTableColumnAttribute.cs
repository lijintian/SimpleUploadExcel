using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;

namespace SimpleUploadExcelHelper
{
    public class DBTableColumnAttribute : Attribute
    {
        /// <summary>
        /// 数据库表对应的列
        /// </summary>
        /// <param name="columnName">列名</param>
        public DBTableColumnAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        public string ColumnName{get; private set; }
    }
}
