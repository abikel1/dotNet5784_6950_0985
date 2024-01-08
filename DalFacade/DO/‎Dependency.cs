namespace DO;

/// <summary>
/// the dependency between the tasks
/// </summary>
/// <param name="Id">the id of the task</param>
/// <param name="IdDependentTask">id of the dependent task</param>
/// <param name="IdPreviousTask">id of the last task</param>

public record Dependency
(
    int Id,
    int IdDependentTask,
    int IdPreviousTask
)

{
    public Dependency() : this(0, 0, 0) { }//empty ctor
}
