

namespace BlApi;

public interface ITaskInList
{
   public void Add(int idTask, int Previous);
   public void Remove(int idDepend, int idPrevius);
}
