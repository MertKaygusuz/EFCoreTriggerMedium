using DemoEFTriggerProject.DbBase;
using DemoEFTriggerProject.DBContext;
using static DemoEFTriggerProject.TokenExtensions.AccessInfoFromToken;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DemoEFTriggerProject.Triggers
{
    public class TableBaseTrigger : IBeforeSaveTrigger<TableBase>
    {
        private readonly DataContext _dataContext;

        public TableBaseTrigger(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task BeforeSave(ITriggerContext<TableBase> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType is ChangeType.Added)
            {
                context.Entity.CreatedAt = DateTime.Now;
                context.Entity.CreatedBy = GetMyUserId();
            }
            else if(context.ChangeType is ChangeType.Modified) 
            {
                context.Entity.LastUpdatedAt = DateTime.Now;
                context.Entity.LastUpdatedBy = GetMyUserId();
            }
            else if(context.ChangeType is ChangeType.Deleted)
            {
                context.Entity.DeletedAt = DateTime.Now;
                context.Entity.DeletedBy = GetMyUserId();

                _dataContext.Entry(context.Entity).State = EntityState.Modified;
            }

            return Task.CompletedTask;
        }
    }
}
