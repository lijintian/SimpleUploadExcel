using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper
{
    public class ConfigHelper
    {
        public static string GetConfig(string configName)
        {

            var config = System.Configuration.ConfigurationManager.AppSettings[configName];

            if (config == null)
            {
                throw new Exception("SimpleUploadExcel-Exception:未配置" + configName);
            }
            else
            {
                return config.ToString();
            }
        }
    }
}
