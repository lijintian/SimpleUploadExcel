using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class ValidBaseAttribute: Attribute
    {
        public string ErrorMsgFormat { set; get; }

        public string ErrorMsg{ set; get; }

        public bool IsValid { get; set; }

        public virtual bool Valid(string val)
        {
            this.ErrorMsg = string.Empty;
            return true;
        }
    }
}
