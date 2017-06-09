using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class RequireAttribute:ValidBaseAttribute
    {
        public RequireAttribute(string errorMsgFormat="")
        {
            if (string.IsNullOrEmpty(errorMsgFormat))
            {
                errorMsgFormat = "为必填字段";
            }
            base.ErrorMsgFormat = errorMsgFormat;
        }

        public override bool Valid(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                base.ErrorMsg = string.Format(base.ErrorMsgFormat, val);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
