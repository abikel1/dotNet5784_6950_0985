namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        internal static DateTime? startProject = null;
        internal static DateTime? endProject = null;
        internal const int StartTaskId = 0;
        internal static int nextTaskId = StartTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        internal const int StartDependencyId = 0;
        internal static int nextDependencyId = StartDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }

    internal static List<DO.Worker> Workers { get; } = new();
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
}
