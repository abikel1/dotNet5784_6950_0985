

using BO;

namespace BlApi;

public interface IBl
{
    public IWorker Worker { get;}
    public ITask Task { get;}
    public IWorkerOnTask WorkerOnTask { get;}
    //public ITaskInList taskInList { get;}
    //public static DateTime? ProjectStartDate { get; set; }
    //public static DateTime? ProjectEndDate { get; set; }
    public DateTime? GetStartProjectDate();
    public void SetStartProjectDate(DateTime? startDate);
    public DateTime? GetEndProjectDate();
    public void SetEndProjectDate(DateTime? endDate);
    public StatusProject GetStatusProject();
    //public void autoSchedule();
}
