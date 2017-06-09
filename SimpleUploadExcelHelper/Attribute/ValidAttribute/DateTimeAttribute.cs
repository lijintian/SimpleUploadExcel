using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class DateTimeAttribute : ValidBaseAttribute
    {
        public DateTimeAttribute(string errorMsgFormat="")
        {
            if (string.IsNullOrEmpty(errorMsgFormat))
            {
                errorMsgFormat = "请填写正确的日期格式";
            }
            base.ErrorMsgFormat = errorMsgFormat;
        }

        public override bool Valid(string val)
        {
            var isValid = true;

            DateTime dateTimeVal = DateTime.Now;

            if (DateTime.TryParse(val, out dateTimeVal))
            {
                isValid = true;
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
