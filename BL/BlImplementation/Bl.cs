using BlApi;
using BO;
using DO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation;

internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IWorker Worker => new WorkerImplementation();

    public ITask Task => new TaskImplementation();

    public IWorkerOnTask WorkerOnTask => new WorkerOnTaskImplementation();

    //public ITaskInList taskInList => new TaskInListImplementation();

    public void autoSchedule()
    {
        var tasks = BlApi.Factory.Get().Task;
        var tasks1 = tasks.ReadTasks();
        foreach( var t in tasks1)
        {
            BO.Task task = tasks.Read(t.Id);
            if(task.DependencyTasks is null)
            {
                task.BeginWork= IBl.ProjectStartDate;
            }
            else
            {
                DateTime? max = DateTime.Now;
                foreach(var dlist in task.DependencyTasks)
                {
                    if(tasks.Read(dlist.Id).DeadLine>max)
                    {
                        max = tasks.Read(dlist.Id).DeadLine;
                    }
                }
                task.BeginWork = max;
            }
            tasks.UpdateTask(task);
        }
        //var x = from t in tasksWithoutPrevios
        //        let ta = tasks.Read(t.Id)
        //        where ta.DependencyTasks is null
        //        select ta.BeginWork = IBl.ProjectStartDate;
        //var y = from t in tasksWithoutPrevios
        //        let ta = tasks.Read(t.Id)
        //        where ta.DependencyTasks is not null
        //        from d in ta.DependencyTasks!
        //        let t1=tasks.Read(d.Id)
        //        select


    }

    public StatusProject GetStatusProject ()
    {
        return StatusProject.Schedule;
        //if (IBl.ProjectStartDate == null)
        //    return StatusProject.Planning;
        //else
        //{
        //    bool flag = _dal.Task.ReadAll().Where(x => x!.BeginWork == null).Any();//return true if there are some tasks with out begin work date
        //    if (flag)
        //        return StatusProject.Schedule;
        //}
        //return StatusProject.Execution;
    }
}
