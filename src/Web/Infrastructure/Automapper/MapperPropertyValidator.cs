namespace WarehouseManagementSystem.Web.Infrastructure.Automapper;

internal static class MapperPropertyValidator
{
    internal static void CheckForDisallowedNulls(object source, object destination)
    {
        foreach (var sourceProperty in source.GetType().GetProperties())
        {
            var underlyingType = Nullable.GetUnderlyingType(sourceProperty.PropertyType);
            if (underlyingType is { })
            {
                var destinationProperty = destination.GetType().GetProperty(sourceProperty.Name);
                if (destinationProperty is { } && destinationProperty.PropertyType == underlyingType && sourceProperty.GetValue(source) == null)
                {
                    throw new ArgumentException($"Значение {sourceProperty.Name} не может быть null");
                }
            }
        }
    }
}
