using DO;

namespace DalApi;

public interface ICrud<T> where T:class
{
    int Create(T item); //Creates new  object in DAL
    T? Read(int id); //Reads object by its ID 
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); //stage 2
    void Update(T item); //Updates object
    void Delete(int id); //Deletes an object by its Id
    T? Read(Func<T, bool> filter); // stage 2
    void clear();
}
