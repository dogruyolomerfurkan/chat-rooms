using Microsoft.EntityFrameworkCore;

namespace Chatter.Persistence.Extensions;

public static class DbContextExtension
{
    /// <summary>
    ///  Change tracker aracılığı ile, audit entitylerin propertylerine bakarak, eğer ilgili property varsa o property'nin datetime değerini setler.
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task SaveChangesAuditAsync(this DbContext context)
    {
        #region ConstValues

        const string CreatedDate = "CreatedDate";
        const string UpdatedDate = "UpdatedDate";
        const string DeletedDate = "DeletedDate";
        const string IsDeleted = "IsDeleted";

        #endregion

        #region CreatedAudit

        var addedEntries = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        if (addedEntries.Any())
        {
            foreach (var addedEntry in addedEntries)
            {
                var propertyCreatedDateControl = addedEntry.GetType().GetProperties().Any(x => x.Name == CreatedDate);

                if (propertyCreatedDateControl)
                {
                    addedEntry.GetType().GetProperty(CreatedDate).SetValue(addedEntry, DateTime.UtcNow);
                }
            }
        }

        #endregion

        #region DeletedAudit

        var deletedEntries = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted)
            .Select(x => x.Entity);

        if (deletedEntries.Any())
        {
            foreach (var deletedEntry in deletedEntries)
            {
                var propertyInfo = deletedEntry.GetType().GetProperties().Any(x => x.Name == DeletedDate);
                if (propertyInfo)
                {
                    deletedEntry.GetType().GetProperty(DeletedDate).SetValue(deletedEntry, DateTime.UtcNow);
                }
            }
        }

        #endregion

        #region ModifiedAudit

        var modifiedEntries = context.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);
        if (modifiedEntries.Any())
        {
            foreach (var modifiedEntry in modifiedEntries)
            {
                var propertyDeletedInfo = modifiedEntry.GetType().GetProperties().Any(x => x.Name == IsDeleted);
                if (propertyDeletedInfo)
                {
                    var isDeleted = (bool) (modifiedEntry.GetType().GetProperty(IsDeleted).GetValue(modifiedEntry));
                    if (isDeleted)
                    {
                        modifiedEntry.GetType().GetProperty(DeletedDate).SetValue(modifiedEntry, DateTime.UtcNow);
                    }
                }

                var propertyInfo = modifiedEntry.GetType().GetProperties().Any(x => x.Name == UpdatedDate);
                if (propertyInfo)
                {
                    modifiedEntry.GetType().GetProperty(UpdatedDate).SetValue(modifiedEntry, DateTime.UtcNow);
                }
            }
        }

        #endregion

    }
}