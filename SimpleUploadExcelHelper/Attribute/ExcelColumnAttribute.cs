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
        /// <summary>
        /// 无须校验
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="isRequire">是否必填</param>
        public ExcelColumnAttribute(string columnName,bool isRequire)
        {
            this.ColumnName = columnName;
            this.ValidType = ExcelFieldValidType.None;
            this.IsRequire = isRequire;
        }

        /// <summary>
        /// 正则校验
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="isRequire">是否必填</param>
        /// <param name="regular">正则表达式</param>
        /// <param name="validErrorMsg">校验错误信息</param>
        public ExcelColumnAttribute(string columnName, bool isRequire, string regular = @"\w*", string validErrorMsg = "")
        {
            var reg = new Regex(regular);
            this.ValidRegular = reg;
            this.ColumnName = columnName;
            this.ValidType = ExcelFieldValidType.Regular;
            this.ValidErrorMsg = validErrorMsg;
            this.IsRequire = isRequire;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="isRequire">是否必填</param>
        /// <param name="validType">校验类型</param>
        /// <param name="validErrorMsg">校验错误信息</param>
        public ExcelColumnAttribute(string columnName, bool isRequire, ExcelFieldValidType validType,string validErrorMsg = "")
        {
            this.ColumnName = columnName;
            this.ValidType = ExcelFieldValidType.Regular;
            this.IsRequire = isRequire;

            var reg = new Regex("");
            switch (validType)
            {
                case ExcelFieldValidType.Number:
                    reg = new Regex(@"^[0-9]+$");
                    if (string.IsNullOrEmpty(validErrorMsg))
                    {
                        validErrorMsg = "请输入数字";
                    }
                    break;
                case ExcelFieldValidType.Letter:
                    reg = new Regex(@"^[a-zA-Z]+$");
                    if (string.IsNullOrEmpty(validErrorMsg))
                    {
                        validErrorMsg = "请输入字母";
                    }
                    break;
                case ExcelFieldValidType.City:
                    this.DataSource = BaseDataSource.CreateInstance().City;
                    this.ValidType = ExcelFieldValidType.DataSource;
                    break;
                default:
                    //todo:实现其他的类型
                    throw new Exception(validType.ToString() + "类型未实现");

            }

            this.ValidRegular = reg;
            this.ValidErrorMsg = validErrorMsg;
        }



        public string DataSource { get;private set; }

        public ExcelFieldValidType ValidType { get; private set; }

        public string ValidErrorMsg { get; private set; }

        public string ColumnName{get; private set; }

        public Regex ValidRegular {get; private set; }

        public bool IsRequire{ get; private set; }
    }
}
