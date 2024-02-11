using DalApi;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dal;

sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public DateTime? GetStartProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        return root.ToDateTimeNullable("startDate");
    }

    public void SetStartProjectDate(DateTime? startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("startDate")!.Value=startDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }

    public DateTime? GetEndProjectDate()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        return root.ToDateTimeNullable("endDate");
    }

    public void SetEndProjectDate(DateTime? endDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("endDate")!.Value = endDate.ToString()!;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }
}
