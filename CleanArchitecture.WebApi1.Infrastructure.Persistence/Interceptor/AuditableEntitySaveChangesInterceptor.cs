using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.WebApi1.Application.Interfaces;
using CleanArchitecture.WebApi1.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CleanArchitecture.WebApi1.Infrastructure.Persistence.Interceptor
{
    internal class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IAccountService _currentUserService;

        public AuditableEntitySaveChangesInterceptor(
            IAccountService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<AuditableBaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "_currentUserService.UserId"; // need to inject user id
                    entry.Entity.Created = DateTime.Now;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified ||
                    entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModifiedBy = "_currentUserService.UserId"; /// need to inject user id
                    entry.Entity.LastModified = DateTime.Now;

                }
            }
        }
    }
}
public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
