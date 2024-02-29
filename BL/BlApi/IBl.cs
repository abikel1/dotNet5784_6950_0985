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
        #region
        /// <summary>
        /// תאריך ושעה נוכחיים.
        /// </summary>
        public DateTime Clock { get; }

        /// <summary>
        /// מקדם את השעון בשנה אחת.
        /// </summary>
        public DateTime AdvanceByYear();

        /// <summary>
        /// מקדם את השעון ביום אחד.
        /// </summary>
        public DateTime AdvanceByDay();

        /// <summary>
        /// מקדם את השעון בשעה אחת.
        /// </summary>
        public DateTime AdvanceByHour();

        /// <summary>
        /// מאפס את השעון לתאריך ושעה נוכחיים.
        /// </summary>
        public void ResetClock();
        #endregion
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

