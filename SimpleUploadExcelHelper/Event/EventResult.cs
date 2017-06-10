using SimpleUploadExcelHelper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper.Event
{
    public class EventResult<T> where T : EntityBase
    {
        public List<T> Entities { get; set; }
    }
}
