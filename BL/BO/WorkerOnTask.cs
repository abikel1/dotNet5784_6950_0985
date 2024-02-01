
namespace BO;
/// <summary>
/// worker that do this task
/// </summary>
/// <param name="Id">the id of the worker</param>
/// <param name="Name">the name of the worker</param>
public class WorkerOnTask
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
