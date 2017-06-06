using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchUploadExcelHelper.Entities
{
    public class EntityBase
    {
        public DataRow OriginalData { get; set; }

        public string ExcelRowNo { get; set; }

        public string ErrorDiscription { get; set; }

        public virtual bool IsValid() {
            return string.IsNullOrEmpty(this.ErrorDiscription);
        }

        public void AppendError(string error)
        {
            this.ErrorDiscription += error + Environment.NewLine;
        }
    }
}
