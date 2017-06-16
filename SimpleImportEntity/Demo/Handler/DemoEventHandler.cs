using SimpleUploadExcelHelper.Entities;
using SimpleUploadExcelHelper.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleImportEntity.Handler
{
    public class DemoEntitiesHandler : IEntitiesHandler
    {
        public  void Handle(List<EntityBase> entities)
        {
            foreach (var entity in entities)
            {
                var demoEntity = (DemoEntity)entity;

                demoEntity.Field2 = "测试Handler";
                demoEntity.AppendError("测试Handler");
            }
        }
    }
}
