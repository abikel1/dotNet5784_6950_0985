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
        public IWorker Worker => new WorkerImplementation(this);
        public ITask Task => new TaskImplementation(this);
        public IUser User => new UserImplementation(this);

        public ITaskInList TaskInList => new TaskInLIstImplementation(this);

        // Method to initialize the database
        public void InitializeDB() => DalTest.Initialization.Do();

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
                bool flag = _dal.Task.ReadAll().Where(x => x!.BeginWork != null && x.BeginTask != null).Any();
                if (!flag)
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
        private static DateTime s_Clock = DateTime.Now;

        public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

        public DateTime AdvanceByYear()
        {
            return Clock = Clock.AddYears(1);
        }

        public DateTime AdvanceByDay()
        {
            return Clock = Clock.AddDays(1);
        }

        public DateTime AdvanceByHour()
        {
            return Clock = Clock.AddHours(1);
        }

        public DateTime ResetClock()
        {
          return  Clock = DateTime.Now.Date;
        }
    }
}
