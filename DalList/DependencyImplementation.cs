namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class DependencyImplementation : IDependency
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
            throw new DalDoesNotExistException($"Dependency with ID={id} is not exists");
        }
        DataSource.Dependencies.Remove(Read(id)!);
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(item => item.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return DataSource.Dependencies.FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        DataSource.Dependencies.Add(item);
    }
}
