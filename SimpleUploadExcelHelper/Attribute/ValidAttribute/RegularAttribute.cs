using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class RegularAttribute : ValidBaseAttribute
    {
        public string RegExpress { get; set; }

        public RegularAttribute(string regExpress, string errorMsgFormat)
        {
            this.RegExpress = regExpress;
            base.ErrorMsgFormat = errorMsgFormat;
        }

        public override bool Valid(string val)
        {
            var isValid = true;

            var reg = new Regex(this.RegExpress);

            if (reg.IsMatch(val))
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
