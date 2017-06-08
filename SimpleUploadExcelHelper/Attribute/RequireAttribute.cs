using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class RequireAttribute:ValidBaseAttribute
    {
        public RequireAttribute(string errorMsgFormat)
        {
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
