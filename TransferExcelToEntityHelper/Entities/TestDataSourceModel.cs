using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchUploadExcelHelper.Common;
using BatchUploadExcelHelper.Entities;

namespace BatchUploadExcelHelper
{
    [DBTable("TestBatchImportTable")]
    public class TestDataSourceModel : EntityBase
    {
        
        [ExcelColumn("Field1",true, @"^[0-9]+$", "只能输入数字")]
        [DBTableColumn("Field1")]
        public string Field1 { get; set; }

        [ExcelColumn("城市",true,ExcelFieldValidType.City , "在数据库中不存在")]
        [DBTableColumn("City")]
        public string Field2 { get; set; }
    }
}
