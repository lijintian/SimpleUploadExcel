using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class LengthAttribute: ValidBaseAttribute
    {
        public int MinLen { get; set; }

        public int MaxLen { get; set; }

        public LengthAttribute(int maxLen, string errorMsgFormat="")
        {
            this.MaxLen = maxLen;

            if (string.IsNullOrEmpty(errorMsgFormat))
            {
                errorMsgFormat = "长度不能超过"+maxLen;
            }
            base.ErrorMsgFormat = errorMsgFormat;
        }

        public LengthAttribute(int minLen,int maxLen, string errorMsgFormat="")
        {
            this.MinLen = minLen;
            this.MaxLen = maxLen;
            if (string.IsNullOrEmpty(errorMsgFormat))
            {
                errorMsgFormat = "长度范围为" + minLen+"-"+maxLen;
            }
            base.ErrorMsgFormat = errorMsgFormat;
        }

        public override bool Valid(string val)
        {
            var isValid = true;

            if (val.Length>=this.MinLen&&val.Length<=this.MaxLen)
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
