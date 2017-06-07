using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;

namespace SimpleUploadExcelHelper
{
    public class DBTableAttribute : Attribute
    {
        /// <summary>
        /// 数据库表
        /// </summary>
        /// <param name="tableName">表名</param>
        public DBTableAttribute(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName{get; private set; }
    }
}
