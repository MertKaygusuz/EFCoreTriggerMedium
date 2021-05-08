using DemoEFTriggerProject.DbBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.DbClasses
{
    public class DbDemoClass : TableBase
    {
        [Key]
        public int Id { get; set; }

        public string StringField { get; set; }
        
        public int IntField { get; set; }
    }
}
