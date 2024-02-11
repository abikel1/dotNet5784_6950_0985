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
    //public void autoSchedule()
    //{
    //    var tasks = BlApi.Factory.Get().Task;
    //    var tasks1 = tasks.ReadTasks();
    //    foreach( var t in tasks1)
    //    {
    //        BO.Task task = tasks.Read(t.Id);
    //        if(task.DependencyTasks is null)
    //        {
    //            task.BeginWork= IBl.ProjectStartDate;
    //        }
    //        else
    //        {
    //            DateTime? max = DateTime.Now;
    //            foreach(var dlist in task.DependencyTasks)
    //            {
    //                if(tasks.Read(dlist.Id).DeadLine>max)
    //                {
    //                    max = tasks.Read(dlist.Id).DeadLine;
    //                }
    //            }
    //            task.BeginWork = max;
    //        }
    //        tasks.UpdateTask(task);
    //    }
    //}

    public DateTime? GetEndProjectDate()=>_dal.GetEndProjectDate();

    public DateTime? GetStartProjectDate()=>_dal.GetStartProjectDate();

    public StatusProject GetStatusProject ()
    {
        if (BlApi.Factory.Get().GetStartProjectDate() == null)
            return StatusProject.Planning;
        else
        {
            bool flag = _dal.Task.ReadAll().Where(x => x!.BeginWork == null).Any();//return true if there are some tasks with out begin work date
            if (flag)
                return StatusProject.Schedule;
        }
        return StatusProject.Execution;
    }

    public void SetEndProjectDate(DateTime? endDate)=>_dal.SetEndProjectDate(endDate);

    public void SetStartProjectDate(DateTime? startDate)=>_dal.SetStartProjectDate(startDate);
}
