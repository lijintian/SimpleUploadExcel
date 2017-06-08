using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;
using SimpleUploadExcelHelper.Entities;
using SimpleUploadExcelHelper;

namespace SimpleImportEntity
{
    
    public class TestRegularValidModel : EntityBase
    {
        [ExcelColumn("Field1")]
        public string Field1 { get; set; }

        [ExcelColumn("Field2")]
        public string Field2 { get; set; }

        [ExcelColumn("Field3")]
        public string Field3 { get; set; }

        [ExcelColumn("Field4")]
        public string Field4 { get; set; }


        public override bool IsValid()
        {
            if (this.Field1 != "处理号")
            {
                base.AppendError("Field1只能输入处理号！");
            }

            return string.IsNullOrEmpty(base.ErrorDiscription);
        }
    }
}
