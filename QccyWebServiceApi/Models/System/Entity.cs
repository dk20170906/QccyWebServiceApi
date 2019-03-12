using EdaSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Models.System
{
    public abstract class Entity : IEntity
    {

        public Entity()
        {
            this.Id = Guid.NewGuid();
            this.TimeStamp = DateTime.Now;
        }
        public Entity(Guid guid)
        {
            this.Id = guid;
            this.TimeStamp = DateTime.Now;
        }
        public Guid Id { get; }
        public string EntityId { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime UpdateTimeStamp { get; set; }
        public string Remark { get; set; }


    }
}
