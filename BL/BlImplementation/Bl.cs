using BlApi;
using BO;

namespace BlImplementation;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public IWorkerOnTask WorkerOnTask => new WorkerOnTaskImplementation();

    public ITaskInList taskInList => new TaskInListImplementation();

    public void autoSchedule()
    {
        IEnumerable<BO.Task> taskswithoutprevios = _dal.Task.ReadAll(x=>x.)
    }

    public StatusProject GetStatusProject ()
    {
        if (IBl.ProjectStartDate == null)
            return StatusProject.Planning;
        else
        {
            bool flag = _dal.Task.ReadAll().Where(x => x!.BeginWork == null).Any();//return true if there are some tasks with out begin work date
            if (flag)
                return StatusProject.Schedule;
        }
        return StatusProject.Execution;
    }
}
