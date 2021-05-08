using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.DbBase
{
    public interface ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }

        public int? DeletedBy { get; set; }
    }
}
