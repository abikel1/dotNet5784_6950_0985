namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency dependency = item with { Id = id };
        DataSource.Dependencies.Add(dependency);
        return id;
    }

    public void Delete(int id)
    {
        if (Read(id) is null)
        {
            throw new Exception($"Dependency with ID={id} is not exists");
        }
        DataSource.Dependencies.Remove(Read(id)!);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(item => item.Id == id) ?? throw new Exception($"Dependency with ID={id} does not exist");
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
        {
            throw new Exception($"Dependency with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
}
