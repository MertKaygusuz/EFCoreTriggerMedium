using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DemoEFTriggerProject.DbBase;
using DemoEFTriggerProject.DbClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DemoEFTriggerProject.DBContext
{
    public partial class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Users>()
                   .HasIndex(x => x.Email)
                   .IsUnique();

            builder.Model.GetEntityTypes()
                           .Where(entityType => typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                           .ToList()
                           .ForEach(entityType =>
                           {
                               builder.Entity(entityType.ClrType)
                                      .HasQueryFilter(ConvertFilterExpressionForSoftDeletables(x => x.DeletedAt == null, entityType.ClrType));
                           });
        }


        private static LambdaExpression ConvertFilterExpressionForSoftDeletables(
                            Expression<Func<ISoftDeletable, bool>> filterExpression,
                            Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }
}
