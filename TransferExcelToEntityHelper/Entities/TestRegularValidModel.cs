using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchUploadExcelHelper.Common;
using BatchUploadExcelHelper.Entities;

namespace BatchUploadExcelHelper
{
    
    public class TestRegularValidModel : EntityBase
    {
        [ExcelColumn("Field1",true, @"^[0-9]+$", "只能输入数字")]
        public string Field1 { get; set; }

        [ExcelColumn("Field2", true, @"^[a-zA-Zs]+$", "只能输入字母")]
        public string Field2 { get; set; }

        [ExcelColumn("Field3", true, ExcelFieldValidType.Number)]
        public string Field3 { get; set; }

        [ExcelColumn("Field4", true, ExcelFieldValidType.Letter)]
        public string Field4 { get; set; }
    }
}
