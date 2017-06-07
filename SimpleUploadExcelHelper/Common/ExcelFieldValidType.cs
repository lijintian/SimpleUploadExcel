using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper.Common
{
    public enum ExcelFieldValidType
    {
        /// <summary>
        /// 无须校验
        /// </summary>
        None=0,
        /// <summary>
        /// 正则
        /// </summary>
        Regular=1,
        /// <summary>
        /// 数据源
        /// </summary>
        DataSource=2,
        /// <summary>
        /// 数字
        /// </summary>
        Number=3,
        /// <summary>
        /// 字母
        /// </summary>
        Letter=4,
        /// <summary>
        /// 城市 DataSource
        /// </summary>
        City=5
    }
}
