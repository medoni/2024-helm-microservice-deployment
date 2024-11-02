using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;

// TODO: Remove
namespace POS.Persistence.PostgreSql.Extensions;
internal static class EfCoreChangeTrackingExtensions
{
    public static void UpdateValuesGraph<TEntity>(
        this EntityEntry? trackedEntry,
        DbContext context,
        object newEntity
    )
    {
        var visitedEntities = new HashSet<object>();

        UpdateValuesGraphCore<TEntity>(
            context,
            visitedEntities,
            trackedEntry,
            newEntity
        );
    }

    private static void UpdateValuesGraphCore<TEntity>(
        DbContext context,
        HashSet<object> visitedEntities,
        EntityEntry? trackedEntry,
        object newEntity
    )
    {
        if (visitedEntities.Contains(newEntity)) return;

        if (trackedEntry is null || trackedEntry.State == EntityState.Detached)
        {
            context.Update(newEntity);
        }
        else
        {
            trackedEntry.CurrentValues.SetValues(newEntity);
            UpdateNavigtions(context, visitedEntities, trackedEntry, newEntity);
        }

        visitedEntities.Add(newEntity);
    }

    private static void UpdateNavigtions(
        DbContext context,
        HashSet<object> visitedEntities,
        EntityEntry trackedEntry,
        object newEntity
    )
    {
        foreach (var navigation in trackedEntry.Navigations)
        {
            if (navigation.Metadata.IsCollection)
            {
                UpdateCollection(
                    trackedEntry.Context,
                    visitedEntities,
                    navigation,
                    newEntity
                );
            }
            else
            {
                var trackedNavigationEntity = navigation.Metadata.PropertyInfo?.GetValue(trackedEntry.Entity);
                var trackedNavigationEntry = trackedNavigationEntity == null ? null : trackedEntry.Context.Entry(trackedNavigationEntity);
                var referencedEntity = navigation.Metadata.PropertyInfo?.GetValue(newEntity);

                if (referencedEntity == null)
                {
                }
                else if (trackedNavigationEntry == null && referencedEntity == null)
                {
                }
                else
                {
                    UpdateValuesGraphCore<dynamic>(
                        context,
                        visitedEntities,
                        trackedNavigationEntry,
                        referencedEntity
                    );
                }
            }
        }
    }

    private static void UpdateCollection(
        DbContext context,
        HashSet<object> visitedEntities,
        NavigationEntry navigation,
        object newEntity
    )
    {
        var trackedCollection = (IEnumerable?)navigation.CurrentValue;
        var updatedCollection = (IEnumerable?)navigation.Metadata.PropertyInfo?.GetValue(newEntity);
        if (updatedCollection == null) return;
        if (trackedCollection == null) throw new NotImplementedException();
        if (trackedCollection == updatedCollection) return;

        var elementType = navigation.Metadata.ClrType;
        var keyProperties = GetKeyProperties(context, elementType);

        var trackedList = trackedCollection.Cast<object>().ToList();
        var updatedList = updatedCollection.Cast<object>().ToList();

        // Find matching entities using primary key
        foreach (var updatedItem in updatedList)
        {
            var matchingTrackedItem = trackedList.FirstOrDefault(trackedItem => EntityKeysMatch(trackedItem, updatedItem, keyProperties));

            if (matchingTrackedItem is null)
            {
                matchingTrackedItem = context.Attach(updatedItem);
            }

            var matchingTrackedEntry = context.Entry(matchingTrackedItem);
            UpdateValuesGraphCore<dynamic>(
                context,
                visitedEntities,
                matchingTrackedEntry,
                matchingTrackedItem
            );
        }

        // Remove items no longer present in the updated collection
        foreach (var trackedItem in trackedList)
        {
            if (!updatedList.Any(updatedItem => EntityKeysMatch(trackedItem, updatedItem, keyProperties)))
            {
                context.Remove(trackedItem);
            }
        }
    }

    private static IEnumerable<IProperty> GetKeyProperties(DbContext context, Type entityType)
    {
        if (entityType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(entityType))
        {
            return GetKeyProperties(context, entityType.GetGenericArguments()[0]);
        }

        var entityTypeModel = context.Model.FindEntityType(entityType);
        return entityTypeModel?.FindPrimaryKey()?.Properties
            ?? throw new InvalidOperationException("No properties found.");
    }

    private static bool EntityKeysMatch(object trackedEntity, object updatedEntity, IEnumerable<IProperty> keyProperties)
    {
        foreach (var keyProperty in keyProperties)
        {
            var trackedValue = keyProperty.PropertyInfo!.GetValue(trackedEntity);
            var updatedValue = keyProperty.PropertyInfo!.GetValue(updatedEntity);

            if (!Equals(trackedValue, updatedValue))
                return false;
        }

        return true;
    }
}
