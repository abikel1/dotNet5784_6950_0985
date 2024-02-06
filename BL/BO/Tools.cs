namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T obj)
    {
        string result = $"{typeof(T).Name} properties: \n";
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(obj);


            if (value != null)
            {
                if (value is IEnumerable<object> && !(value is string))
                {
                    var enumerable = value as IEnumerable<object>;
                    foreach (var item in enumerable!)
                    {
                        result += ToStringProperty(item);
                    }
                }
                else
                {
                    result += $"{property.Name}: {value}\n";
                }
            }
        }
        return result;
    }
}
