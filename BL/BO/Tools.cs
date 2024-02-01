namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        var properties=typeof(T).GetProperties();
        string result=$"{typeof(T).Name} properties: \n";
        foreach(var property in properties)
        {
            result += $"{property.Name}: {property.GetValue(obj)}\n";
        }
        return result ;
    }
}
