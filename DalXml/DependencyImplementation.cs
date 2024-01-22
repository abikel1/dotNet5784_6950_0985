namespace Dal;
using DalApi;
using DO;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Resolvers;

internal class DependencyImplementation:IDependency
{
    readonly string s_dependencys_xml = "dependencys";
    readonly string s_dataconfig_xml="data-config";
    private const string _entity_name = nameof(Dependency);
    private const string _id = nameof(Dependency.Id);
    private const string _idDependentTask = nameof(Dependency.IdDependentTask);
    private const string _idPreviousTask = nameof(Dependency.IdPreviousTask);

    XElement? depRoot = new XElement("ArrayOfDependency");
    static Dependency GetDependency(XElement dep)
    {
        return new Dependency()
        {
            Id = int.TryParse((string?)dep.Element("Id"), out var Id) ? Id : throw new FormatException("can't convert id"),
            IdDependentTask = int.TryParse((string?)dep.Element("IdDependentTask"), out var IdDependentTask) ? IdDependentTask : throw new FormatException("can't convert id dependent task"),
            IdPreviousTask = int.TryParse((string?)dep.Element("IdPreviousTask"), out var IdPreviousTask) ? IdPreviousTask : throw new FormatException("can't convert id previous task"),
        };
    }
    public int Create(Dependency item)
    {
        depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        //if (Read(item.Id) != null)
        //{
        //    throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        //}
        XElement  dep=new XElement("Dependency",new XElement("Id", Config.NextDependencyId), new XElement("IdDependentTask",item.IdDependentTask),new XElement("IdPreviousTask",item.IdPreviousTask));
        depRoot.Add(dep);
        XMLTools.SaveListToXMLElement(depRoot,s_dependencys_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? depElem = (from d  in depRoot.Elements()
                             where (int?)d.Element("Id")==id
                             select d).FirstOrDefault() ?? throw new DalDoesNotExistException($"Engineer with Id = {id} is not exist");
        if (depElem != null)
        {
            depElem.Remove();
            XMLTools.SaveListToXMLElement(depRoot, s_dependencys_xml);
        }
    }

    public Dependency? Read(int id)
    {
        depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        return (from d in depRoot.Elements()
                let dep = GetDependency(d)
                where dep.Id == id
                select (DO.Dependency?)dep).FirstOrDefault();
        //XElement? depElem = depRoot.Elements().FirstOrDefault(dep => (int?)dep.Element("ID") == id);
        //if (depElem != null) 
        //{
        //    return GetDependency(depElem);
        //}
        //else
        //{
        //    return null;
        //}
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        //return  depRoot.Elements().Select(d=>GetDependency(d)).FirstOrDefault(filter);
        return (from d in depRoot.Elements()
                let dep = GetDependency(d)
                where filter(dep)
                select (DO.Dependency?)dep).FirstOrDefault();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        var dependencys = XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements();
        if (filter == null)
            return dependencys.Select(GetDependency);
        else
            return dependencys.Select(GetDependency).Where(filter);
    }

    public void Update(Dependency item)
    {
        XElement? depend = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        //depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement? depelem = depend.Elements().FirstOrDefault(d => (int?)d.Element("Id") == item.Id);
        if (depelem is null)
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} is not exists");
        }
        depelem.Remove();
        depend.Add(getXelementfromDependency(item));
        XMLTools.SaveListToXMLElement(depend, s_dependencys_xml);
        //Delete(item.Id);
        //depRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        //depRoot.Add(item);
        //XMLTools.SaveListToXMLElement(depRoot, s_dependencys_xml);
    }
    static XElement getXelementfromDependency(Dependency dependency)
    {
        return new XElement(_entity_name,
               new XElement(_id, dependency.Id),
               new XElement(_idDependentTask, dependency.IdDependentTask),
               new XElement(_idPreviousTask, dependency.IdPreviousTask));
    }
    public void clear()
    {
        List<DO.Dependency> dependencys = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(s_dependencys_xml);
        dependencys.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependencys, s_dependencys_xml);
        XMLTools.SaveListToXMLElement(new XElement("config", new XElement("NextTaskId", 0), new XElement("NextDependencyId", 0)), s_dataconfig_xml);
    }
}
