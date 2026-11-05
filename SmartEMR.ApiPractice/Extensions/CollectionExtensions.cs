namespace SmartEMR.ApiPractice.Extensions;

public static class CollectionExtensions
{
    public static T AddAndReturn<T>(this ICollection<T> collection, T item)
    {
        collection.Add(item);
        return item;
    }
}