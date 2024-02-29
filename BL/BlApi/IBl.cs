using BO;

namespace BlApi
{
    // Interface definition for the Business Logic layer
    public interface IBl
    {
        // Properties to access various entities
        public IWorker Worker { get; }
        public ITask Task { get; }
        public ITaskInList TaskInList { get; }
        public IUser User { get; }

        // Method to initialize the database
        public void InitializeDB();

        // Method to reset the database
        public void ResetDB();

        // Methods to get and set the start project date
        public DateTime? GetStartProjectDate();
        public void SetStartProjectDate(DateTime? startDate);

        // Methods to get and set the end project date
        public DateTime? GetEndProjectDate();
        public void SetEndProjectDate(DateTime? endDate);

        // Method to get the status of the project
        public StatusProject GetStatusProject();
    }
}

