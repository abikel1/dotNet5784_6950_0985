using BlApi;
using BO;
using DO;
using System.ComponentModel.DataAnnotations;

namespace BlImplementation
{
    // Internal implementation of the IBl interface
    internal class Bl : IBl
    {
        // Private field for accessing the data access layer
        private DalApi.IDal _dal = DalApi.Factory.Get;

        // Properties returning instances of worker, task, and worker-task implementations
        public IWorker Worker => new WorkerImplementation();
        public ITask Task => new TaskImplementation();

        // Method to initialize the database
        public void InitializeDB() => DalTest.Initialization.Do();
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

        // Method to get the end project date from the data access layer
        public DateTime? GetEndProjectDate() => _dal.GetEndProjectDate();

        // Method to get the start project date from the data access layer
        public DateTime? GetStartProjectDate() => _dal.GetStartProjectDate();

        // Method to get the status of the project
        public StatusProject GetStatusProject()
        {
            if (BlApi.Factory.Get().GetStartProjectDate() == null)
                return StatusProject.Planning;
            else
            {
                bool flag = _dal.Task.ReadAll().Where(x => x!.BeginWork == null).Any();
                if (flag)
                    return StatusProject.Schedule;
            }
            return StatusProject.Execution;
        }

        // Method to set the end project date in the data access layer
        public void SetEndProjectDate(DateTime? endDate) => _dal.SetEndProjectDate(endDate);

        // Method to set the start project date in the data access layer
        public void SetStartProjectDate(DateTime? startDate) => _dal.SetStartProjectDate(startDate);

        // Method to reset the database
        public void ResetDB() => DalTest.Initialization.Reset();
    }
}
