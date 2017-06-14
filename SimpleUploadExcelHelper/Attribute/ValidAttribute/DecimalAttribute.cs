using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class DecimalAttribute : ValidBaseAttribute
    {
        public int IntLen { get; set; }

        public int DecLen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intLen">整数长度</param>
        /// <param name="decLen">小数长度</param>
        /// <param name="errorMsgFormat"></param>
        public DecimalAttribute(int intLen,int decLen, string errorMsgFormat="")
        {
            if (string.IsNullOrEmpty(errorMsgFormat))
            {
                errorMsgFormat = "请填写" + intLen + "位内整数" + decLen + "位内小数";
            }
            this.IntLen = intLen;
            this.DecLen = decLen;
            base.ErrorMsgFormat = errorMsgFormat;
        }

        public override bool Valid(string val)
        {
            decimal decVal = 0;

            var isValid = true;

            if (decimal.TryParse(val, out decVal))
            {
                var reg = new Regex("^[+-]?[0-9]{0,"+this.IntLen+"}$");
                if (val.Contains("."))
                {
                    reg= new Regex("^[+-]?[0-9]{0," + this.IntLen + "}.[0-9]{0," + this.DecLen + "}$");
                }

                if (reg.IsMatch(val))
                {
                    IsValid = true;
                }
                else
                {
                    base.ErrorMsg = string.Format(base.ErrorMsgFormat, val);
                    isValid = false;
                }
            }
            else
            {
                base.ErrorMsg = string.Format(base.ErrorMsgFormat, val);
                isValid = false;
            }

            return isValid;
        }
    }
}
