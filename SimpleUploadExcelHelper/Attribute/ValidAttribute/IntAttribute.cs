using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class IntAttribute:ValidBaseAttribute
    {
        public IntAttribute(string errorMsgFormat="")
        {
            if (string.IsNullOrEmpty(errorMsgFormat))
            {
                errorMsgFormat = "请填写整数";
            }
            base.ErrorMsgFormat = errorMsgFormat; 
        }

        public override bool Valid(string val)
        {
            var isValid = true;

            var intVal = 0;

            if (int.TryParse(val, out intVal))
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
