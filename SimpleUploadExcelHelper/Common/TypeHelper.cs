using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper.Common
{
    public class TypeHelper
    {
        public static Type GetType(string EntityClassName)
        {
            var entityAssemblyConfig = ConfigHelper.GetConfig("SimpleImportExcel:EntityAssemblyName");
            var uploadHelperAssembly = System.Reflection.Assembly.Load(entityAssemblyConfig.ToString());
            var entityAssemblyNameSpaceConfig = ConfigHelper.GetConfig("SimpleImportExcel:EntityNameSpace");

            var entityType = uploadHelperAssembly.GetType(entityAssemblyNameSpaceConfig.ToString() + "." + EntityClassName);

            if (entityType == null)
            {
                throw new Exception("SimpleUploadExcel-Exception：无法找到导入类型" + EntityClassName);
            }

            return entityType;
        }

        public static Type GetHandlerType(string HandlerTypeName)
        {
            var entityAssemblyConfig = ConfigHelper.GetConfig("SimpleImportExcel:EntityHandlerAssemblyName");
            var uploadHelperAssembly = System.Reflection.Assembly.Load(entityAssemblyConfig.ToString());
            var entityAssemblyNameSpaceConfig = ConfigHelper.GetConfig("SimpleImportExcel:EntityHandlerNameSpace");

            var entityType = uploadHelperAssembly.GetType(entityAssemblyNameSpaceConfig.ToString() + "." + HandlerTypeName);

            if (entityType == null)
            {
                throw new Exception("SimpleUploadExcel-Exception：无法找到导入类型Handler" + HandlerTypeName);
            }

            return entityType;
        }
    }
}
