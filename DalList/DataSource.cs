namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        public static DateTime? startProject { get; set; } = null;
        public static DateTime? endProject { get; set; } = null;
        internal const int StartTaskId = 1;
        internal static int nextTaskId = StartTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        internal const int StartDependencyId = 1;
        internal static int nextDependencyId = StartDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }

    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
}
