namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        int id = Config.NextTaskId;
        Task task = item with { Id = id };
        tasks.Add(task);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
        return id;
    }

    public void Delete(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (Read(id) is null)
        {
            throw new DalDoesNotExistException($"Task with ID={id} is not exists");
        }
        tasks.Remove(Read(id)!);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }

    public Task? Read(int id)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(item => item.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        return tasks.FirstOrDefault(filter);
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        if (filter == null)
            return tasks.Select(item => item);
        else
            return tasks.Where(filter);
    }

    public void Update(Task item)
    {
        if (Read(item.Id) is null)
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} is not exists");
        }
        Delete(item.Id);
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }
    public void clear()
    {
        List<DO.Task> tasks = XMLTools.LoadListFromXMLSerializer<DO.Task>(s_tasks_xml);
        tasks.Clear();
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasks, s_tasks_xml);
    }
}
