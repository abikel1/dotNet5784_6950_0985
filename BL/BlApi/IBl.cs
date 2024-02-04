

using BO;

namespace BlApi;

public interface IBl
{
    public IWorker Worker { get;}
    public ITask Task { get;}
    public IWorkerOnTask WorkerOnTask { get;}
    public ITaskInList taskInList { get;}
    public static DateTime? ProjectStartDate { get; set; }
    public static DateTime? ProjectEndDate { get; set; }
    public StatusProject GetStatusProject();
    public void autoSchedule();
}
