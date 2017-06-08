using SimpleUploadExcelHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleImportEntity.Demo.DataSource
{
    public class CityDatasource: DataSourceBaseAttribute
    {
        public override DataTable GetDataSource()
        {
            return GetDataSourceInstance.CreateInstance().City;
        }
    }
}
