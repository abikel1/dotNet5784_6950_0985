namespace BO;

public class TaskOnWorker
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
