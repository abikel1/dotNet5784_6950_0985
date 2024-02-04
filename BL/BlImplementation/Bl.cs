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
        var tasks = BlApi.Factory.Get().Task;
        IEnumerable<BO.Task> tasksWithoutPrevios=tasks.ReadTasks().Where(x=>x.DependencyTasks==null);
        IEnumerable<BO.Task> tasksWithPrevios=tasks.ReadTasks().Where(x=>x.DependencyTasks!=null);
        var x = from t in tasksWithoutPrevios
                select t.BeginTask = IBl.ProjectStartDate;
        var y =from t in tasksWithPrevios
               from ta in t.DependencyTasks

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
