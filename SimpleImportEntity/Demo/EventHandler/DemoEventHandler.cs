using SimpleUploadExcelHelper.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleImportEntity.Demo.EventHandler
{
    public class DemoEventHandler : IEventHandler<DemoEntity>
    {
        public List<DemoEntity> Handle(List<DemoEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.AppendError("测试Handler");
            }

            return entities;
        }

        public EventResult<DemoEntity> Handle(EventBase<DemoEntity> evt)
        {
            throw new NotImplementedException();
        }
    }
}
