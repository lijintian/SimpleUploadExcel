using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SimpleUploadExcelHelper.Common;

namespace SimpleUploadExcelHelper
{
    public class EntitiesHandlerAttribute : Attribute
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        /// <param name="handlerName"></param>
        public EntitiesHandlerAttribute(string handlerName)
        {
            this.HandlerName = handlerName;
        }

        public string HandlerName{get; private set; }
    }
}
