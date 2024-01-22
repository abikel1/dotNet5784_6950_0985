namespace Dal;
using DalApi;
using DO;
using System.Xml.Linq;
using System.Xml.Resolvers;

internal class DependencyImplementation:IDependency
{
    readonly string s_dependencys_xml = "dependencys";
    static Dependency GetDependency(XElement dep)
    {
        return new Dependency()
        {
            Id = int.TryParse((string?)dep.Element("Id"), out var id) ? id : throw new FormatException("can't convert id"),
            IdDependentTask = int.TryParse((string?)dep.Element("IdDependentTask"), out var iddependenttask) ? iddependenttask : throw new FormatException("can't convert id dependent task"),
            IdPreviousTask = int.TryParse((string?)dep.Element("IdPreviousTask"), out var idprevioustask) ? id : throw new FormatException("can't convert id previous task"),
        };
    }
    public int Create(Dependency item)
    {
        int id = Config.NextDependencyId;
        Dependency dependency = item with { Id = Config.NextDependencyId };
        XElement? depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        depRoot.Add(dependency);
        XMLTools.SaveListToXMLElement(depRoot, s_dependencys_xml);
        return dependency.Id;
    }

    public void Delete(int id)
    {
        XElement? depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? depElem = depRoot.Elements().FirstOrDefault(dep => (int?)dep.Element("ID") == id);
        if(depElem!=null)
        {
            depElem.Remove();
            XMLTools.SaveListToXMLElement(depRoot, s_dependencys_xml);
        }
    }

    public Dependency? Read(int id)
    {
        XElement? depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? depElem = depRoot.Elements().FirstOrDefault(dep => (int?)dep.Element("ID") == id);
        if (depElem != null) 
        {
            return GetDependency(depElem);
        }
        else
        {
            return null;
        }
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        return  depRoot.Elements().Select(d=>GetDependency(d)).FirstOrDefault(filter);
//        XElement? depElem = depRoot.Elements().FirstOrDefault(filter);
 //       return depElem;
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if (filter == null)
            return XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(d => GetDependency(d));
        else
            return XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().Select(d => GetDependency(d)).Where(filter!);
        //XElement? depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        //if (filter != null)
        //{
        //    return (from d in depRoot.Elements()
        //            let dep = GetDependency(d)
        //            where filter(dep)
        //            select (DO.Dependency?)dep);
        //}
        //else
        //{
        //    return (from d in depRoot.Elements()
        //            select GetDependency(d));
        //}
    }

    public void Update(Dependency item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        XElement? depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        depRoot.Add(item);
        XMLTools.SaveListToXMLElement(depRoot, s_dependencys_xml);
    }
    public void clear()
    {
        List<DO.Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependencys_xml);
        dependencys.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependencys, s_dependencys_xml);
    }
}
