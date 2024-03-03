using System.Net.Mail;

namespace BlApi;
public interface ITask
{
    /// <summary>
    /// return list of Tasks that establish the particular requirment
    /// </summary>
    /// <param name="filter">function according to what will be filtered</param>
    /// <returns>list of the tasks</returns>
    public IEnumerable<BO.TaskInList> ReadTaskInList(Func<BO.TaskInList, bool>? filter = null);
    /// <summary>
    /// return the details of the Task with the id that the function got
    /// </summary>
    /// <param name="id">the id of the task that I want his details </param>
    /// <returns>task object</returns>
    public IEnumerable<BO.Task> ReadTasks(Func<BO.Task, bool>? filter = null);
    /// <summary>
    /// return the details of the Task with the id that the function got
    /// </summary>
    /// <param name="id">the id of the task that I want his details </param>
    /// <returns>task object</returns>
    public BO.Task Read(int id);
    /// <summary>
    /// add new Task with the details that the function got
    /// </summary>
    /// <param name="task">the Task that I want to add</param>
    public int AddTask(BO.Task task);
    /// <summary>
    /// remove Task with the id that the function got
    /// </summary>
    /// <param name="id">the id of the task that I want to remove</param>
    public void RemoveTask(int id);
    /// <summary>
    /// update the Task with the details that the function got
    /// </summary>
    /// <param name="task">the task that I want to update</param>
    public void UpdateTask(BO.Task task);
    /// <summary>
    /// the functon checkInvalid if the previos tasks were done and if so it update the begintask of the task
    /// </summary>
    /// <param name="id">the id of the task that I want to update</param>
    /// <param name="date">the date that I want to update</param>
    public void UpdateStartDates(int id, DateTime? startDate);
    public IEnumerable<BO.TaskInList>? getPriviousTask(int id);
    public IEnumerable<BO.TaskInList> relevantTask(BO.Worker worker);
 //   Method for auto-scheduling(currently commented out)
     public void autoSchedule();
    public void clear();
    public void AddTaskForWorker(int idWorker, int idTask);

    public IEnumerable<BO.TaskInList> ReadPossibleDependencies(int id);//list of the dependencies that the task still doesnt have
}