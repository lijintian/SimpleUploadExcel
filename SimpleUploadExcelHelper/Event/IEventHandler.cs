using SimpleUploadExcelHelper.Entities;
using SimpleUploadExcelHelper.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper.Event
{
    public interface IEventHandler<T> where T:EntityBase
    {
        EventResult<T> Handle(EventBase<T> evt);

        List<T> Handle(List<T> entities);
    }
}
