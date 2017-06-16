using SimpleUploadExcelHelper.Entities;
using SimpleUploadExcelHelper.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUploadExcelHelper.Handler
{
    public interface IEntitiesHandler
    {
        /// <summary>
        /// 实现时EntityBase可以转成ConcreteEntity
        /// </summary>
        /// <param name="entities"></param>
        void Handle(List<EntityBase> entities);
    }
}
