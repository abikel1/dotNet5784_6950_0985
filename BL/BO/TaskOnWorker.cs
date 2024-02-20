namespace BO;
/// <summary>
/// task on worker
/// </summary>
/// <param name="Id">the id of task</param>
/// <param name="Name">the name of task</param>
public class TaskOnWorker
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
