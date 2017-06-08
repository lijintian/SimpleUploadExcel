using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class DataSourceBaseAttribute : Attribute
    {
        public virtual DataTable GetDataSource()
        {
            throw new NotImplementedException("SimpleUploadExcelHelper-Exception：没有实现获取数据源方法。");
        }
    }
}
