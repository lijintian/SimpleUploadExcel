using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;
using SimpleUploadExcelHelper.Entities;
using SimpleUploadExcelHelper;
using SimpleImportEntity.Demo.DataSource;

namespace SimpleImportEntity
{
    [DBTable("TestBatchImportTable")]
    public class TestDataSourceModel : EntityBase
    {
        [ExcelColumn("城市")]
        [DBTableColumn("City")]
        [Require("请填写城市")]
        [CityDatasource]
        [DataSourceField("Name", "Code", "在数据库中不存在")]
        public string Field2 { get; set; }

        [ExcelColumn("Field1")]
        [DBTableColumn("Field1")]
        [Require("请填写Field1")]
        [Int("请输入整数")]
        public int Field1 { get; set; }
    }
}
