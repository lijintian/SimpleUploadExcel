using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class DataSourceFieldAttribute : Attribute
    {
        public string ErrorMsg { set; get; }

        public string KeyFieldName { get; set; }

        public string ValueFieldName { get; set; }

        /// <summary>
        /// 必须与继承DataSourceBaseAttribute的Attribute同时出现
        /// </summary>
        /// <param name="dataSourceField">数据源字段</param>
        public DataSourceFieldAttribute(string keyFieldName,string valueFieldName,string errorMsg)
        {
            this.KeyFieldName = keyFieldName;
            this.ValueFieldName = valueFieldName;
            this.ErrorMsg = errorMsg;
        }
    }
}
