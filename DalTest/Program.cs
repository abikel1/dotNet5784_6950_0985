using Dal;
using DalApi;
using DalTest;
using DO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DalTest;
internal class Program
{
    private static readonly IDal s_dal = Factory.Get;//stage 4
      //private static readonly IDal s_dal = new Dal.DalList();//stage 2
    //static readonly IDal s_dal = new Dal.DalXml();
    static void Main(string[] args)
    {
        try
        {
            //           Initialization.Do(s_dal);//stage 2
            chooseEntities();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        } 
    }
    private static void chooseEntities()
    {
        int entity = 1;
        while(entity!=0)
        {
            Console.WriteLine("Choose the entity that you want to check");
            Console.WriteLine("Enter 1-Worker\n 2-Task\n 3-Dependency\n 4-data initialization\n 0-Exit");
            entity=int.Parse(Console.ReadLine()!);
            switch(entity)
            {
                case 0:
                    break;
                case 1:
                    crudWorker("Worker");
                    break;
                case 2:
                    crudTask("Task");
                    break;
                case 3:
                    crudDependency("Dependency");
                    break;
                case 4:
                    Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                    if (ans == "Y") //stage 3
                    {
                        s_dal!.Worker.clear();
                        s_dal!.Task.clear();
                        s_dal!.Dependency.clear();
                        Initialization.Do(); //stage 4
                    }
                    break;
            }
        }
    }
    private static void crudWorker(String entity)
    {
        Console.WriteLine("Choose the method that you want to do for the Worker");
        Console.WriteLine("Enter 0-Exit\n 1-Create\n 2-Read\n 3-ReadAll\n 4-update\n 5-Delete");
        int choise=int.Parse(Console.ReadLine()!);
        try
        {
            switch (choise)
            {
                case 0://exit
                    break;
                case 1://create worker
                    DO.Worker worker = createWorker();
                    s_dal!.Worker.Create(worker);
                    break;
                case 2://print the worker with the id that we enter
                    Console.WriteLine("Enter id");
                    int idR = int.Parse(Console.ReadLine()!);
                    DO.Worker? workerR = s_dal!.Worker.Read(idR);
                    if (workerR is null)
                      throw new DalDoesNotExistException($"Worker with ID={idR} does not exist");
                    Console.WriteLine(workerR);
                    break;
                case 3://Print all the workers
                    IEnumerable<DO.Worker?> listWorkers = s_dal!.Worker.ReadAll();
                    foreach(DO.Worker? work in listWorkers)
                    {
                        if(work!=null)
                            Console.WriteLine(work);
                    }
                    break;
                case 4:
                    Console.WriteLine("Enter the id of the worker that you want to update");
                    int idU=int.Parse(Console.ReadLine()!);
                    DO.Worker? workerU = s_dal!.Worker.Read(idU);
                    if (workerU is null)
                        throw new DalDoesNotExistException($"Worker with ID={idU} does not exist");
                    Console.WriteLine(workerU);
                    DO.Worker w = updateWorker(workerU);
                    s_dal!.Worker.Update(w);
                    break;
                case 5:
                    Console.WriteLine("Enter the id of the worker that you want to delete");
                    int idD = int.Parse(Console.ReadLine()!);
                    s_dal!.Worker.Delete(idD);
                    break;
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }

    }
    private static void crudTask(String entity)
    {
        Console.WriteLine("Choose the method that you want to do for the Task");
        Console.WriteLine("Enter 0-Exit\n 1-Create\n 2-Read\n 3-ReadAll\n 4-update\n 5-Delete");
        int choise = int.Parse(Console.ReadLine()!);
        try
        {
            switch (choise)
            {
                case 0://exit
                    break;
                case 1://create task
                    DO.Task task = createTask();
                    s_dal!.Task.Create(task);
                    break;
                case 2://print the task with the id that we enter
                    Console.WriteLine("Enter id");
                    int idR = int.Parse(Console.ReadLine()!);
                    DO.Task? taskR = s_dal!.Task.Read(idR);
                    if(taskR is null)
                        throw new DalDoesNotExistException($"Task with ID={idR} does not exist");
                    Console.WriteLine(taskR);
                    break;
                case 3://Print all the tasks
                    IEnumerable<DO.Task?> listTasks = s_dal!.Task.ReadAll();
                    foreach (DO.Task? task1 in listTasks)
                    {
                        if (task1 != null)
                            Console.WriteLine(task1);
                    }
                    break;
                case 4://update the task
                    Console.WriteLine("Enter the id of the task that you want to update");
                    int idU = int.Parse(Console.ReadLine()!);
                    DO.Task? taskU = s_dal!.Task.Read(idU);
                    if (taskU is null)
                        throw new DalDoesNotExistException($"Task with ID={idU} does not exist");
                    Console.WriteLine(taskU);
                    DO.Task t = updateTask(taskU);
                    s_dal!.Task.Update(t);
                    break;
                case 5://delete the task with the id that we enter
                    Console.WriteLine("Enter the id of the task that you want to delete");
                    int idD = int.Parse(Console.ReadLine()!);
                    s_dal!.Task.Delete(idD);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static void crudDependency(String entity)
    {
        Console.WriteLine("Choose the method that you want to do for the Task");
        Console.WriteLine("Enter 0-Exit\n 1-Create\n 2-Read\n 3-ReadAll\n 4-update\n 5-Delete");
        int choise = int.Parse(Console.ReadLine()!);
        try
        {
            switch (choise)
            {
                case 0://exit
                    break;
                case 1://create task
                    DO.Dependency dependency=createDependency();
                    s_dal!.Dependency!.Create(dependency);
                    break;
                case 2://print the dependency with the id that we enter
                    Console.WriteLine("Enter id");
                    int idR = int.Parse(Console.ReadLine()!);
                    DO.Dependency? dependencyR = s_dal!.Dependency.Read(idR);
                    if (dependencyR is null)
                        throw new DalDoesNotExistException($"Dependency with ID={idR} does not exist");
                    Console.WriteLine(dependencyR);
                    break;
                case 3://Print all the tasks
                    IEnumerable<DO.Dependency?> listDependecies = s_dal!.Dependency.ReadAll();
                    foreach (DO.Dependency? dependency1 in listDependecies)
                    {
                        if (dependency1 != null)
                            Console.WriteLine(dependency1);
                    }
                    break;
                case 4://update the dependency
                    Console.WriteLine("Enter the id of the dependency that you want to update");
                    int idU = int.Parse(Console.ReadLine()!);
                    DO.Dependency? dependencyU = s_dal!.Dependency.Read(idU);
                    if (dependencyU is null)
                        throw new DalDoesNotExistException($"dependency with ID={idU} does not exist");
                    Console.WriteLine(dependencyU);
                    DO.Dependency d = updateDependency(dependencyU);
                    s_dal!.Dependency!.Update(d);
                    break;
                case 5://delete the task with the id that we enter
                    Console.WriteLine("Enter the id of the dependency that you want to delete");
                    int idD = int.Parse(Console.ReadLine()!);
                    s_dal!.Dependency!.Delete(idD);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static DO.Worker createWorker() 
    {
        Console.WriteLine("enter id, name,Email,level between 0-4 and cost");
        int id = int.Parse(Console.ReadLine()!);
        string name = Console.ReadLine()!;
        string email = Console.ReadLine()!;
        int level = int.Parse(Console.ReadLine()!);
        Rank rank = (Rank)level;
        double cost = double.Parse(Console.ReadLine()!);
        Worker e = new Worker(id, rank, cost, name, email);
        return e;
    }
    private static DO.Task createTask()
    {
        Console.WriteLine("Enter name, difficulty between 0-4, taskDescreption, product, timeTask, remarks");
        String name = Console.ReadLine()!;
        int difficulty = int.Parse(Console.ReadLine()!);
        String taskDescreption = Console.ReadLine()!;
        String product = Console.ReadLine()!;
        int timeTask = int.Parse(Console.ReadLine()!);
        String? remarks = Console.ReadLine()!;
        if (remarks == "")
            remarks = null;
        DateTime CreateTask = DateTime.Now;
        Rank rank = (Rank)difficulty;
        DO.Task task = new DO.Task(0, rank, 0, taskDescreption, false, name, CreateTask, null, null, null, null, null, remarks, product);
        return task;
    }
    private static DO.Dependency createDependency()
    {
        Console.WriteLine("Enter the id task and the id task that depends on it");
        int idTask=int.Parse(Console.ReadLine()!);
        int idTaskD=int.Parse(Console.ReadLine()!);
        DO.Task? task = s_dal!.Task.Read(idTask);
        DO.Task? taskD = s_dal.Task.Read(idTaskD);
        DO.Dependency dependency = new DO.Dependency(0, idTask, idTaskD);
        return dependency;
    }
    private static DO.Worker updateWorker(DO.Worker? worker2) 
    {
        Console.WriteLine("Enter name,Email,level between 0-4 and cost");
        string name = Console.ReadLine()!;
        if (name == "") { name = worker2!.Name!; }
        string Email = Console.ReadLine()!;
        if (Email == "") { Email = worker2!.Email!; }
        string level = Console.ReadLine()!;
        Rank rank;
        if (level == "") { rank = worker2!.RankWorker; }
        else { rank = (Rank)(int.Parse(level)); }
        double cost1;
        string cost = Console.ReadLine()!;
        if (cost == "") { cost1 = worker2!.HourPrice; }
        else { cost1 = double.Parse(cost); }
        Worker w = new Worker(worker2!.Id, rank, cost1, name, Email);
        return w;
    }
    private static DO.Task updateTask(DO.Task? task2)
    {
        Console.WriteLine("Enter name, id worker, difficulty between 0-4, taskDescreption, product, timeTask, remarks");
        int id = task2!.Id;
        string name = Console.ReadLine()!;
        if (name == "")
            name = task2.Alias!;
        string idw = Console.ReadLine()!;
        int? workerId;
        if (idw == "") { workerId = task2.WorkerId; }
        else workerId = int.Parse(idw);
        string difficult = Console.ReadLine()!;
        Rank rank;
        if (difficult == "") { rank = task2.Difficulty; }
        else { rank = (Rank)int.Parse(difficult); }
        string? taskDescription = Console.ReadLine()!;
        if (taskDescription == "") { taskDescription = task2.TaskDescription; }
        string? product = Console.ReadLine()!;
        if (product == "") { product = task2.Product; }
        string time = Console.ReadLine()!;
        int? timeTask;
        if (time == "")
            timeTask = task2.TimeTask;
        else
            timeTask=int.Parse(time);
        string? remarks = Console.ReadLine()!;
        if (remarks == "") { remarks = task2!.Remarks; }
        //string? createTime = Console.ReadLine();
        //string? beginWorkDateP = Console.ReadLine();
        //string? beginWorkDate = Console.ReadLine();
        //string? deadLine = Console.ReadLine();
        //string? endWorkTime = Console.ReadLine();
        //DateTime? BeginWorkDateP = (beginWorkDateP == "") ? taskU.BeginWorkDateP : DateTime.Parse(beginWorkDateP);
        //DateTime? BeginWorkDate = (beginWorkDate == "") ? taskU.BeginWorkDate : DateTime.Parse(beginWorkDate);
        //DateTime? DeadLine = (deadLine == "") ? taskU.DeadLine : DateTime.Parse(deadLine);
        //DateTime? EndWorkTime = (endWorkTime == "") ? taskU.EndWorkTime : DateTime.Parse(endWorkTime);
        DO.Task t = new DO.Task(0, rank, workerId, taskDescription, false, name, task2.CreateTask, task2.BeginWork, task2.BeginTask, timeTask, task2.DeadLine, task2.EndWorkTime, remarks, product);
        return t;
    }
    private static DO.Dependency updateDependency(DO.Dependency? dependency2)
    {
        Console.WriteLine("DependentTask and DependsOnTask");//Enter the id of the task and the task that dependes on it
        string id = Console.ReadLine()!;//input the id of the task
        string idD = Console.ReadLine()!;//input the id of the task that depends on the previous task
        int id1 = dependency2!.IdPreviousTask;//The current value that exists in the entity
        int id2 = dependency2.IdDependentTask;//The current value that exists in the entity
        //update task 1
        if (id != "")//The user want to change the id
            id1 = int.Parse(id);
        //update task 2
        if (idD != "")
            id2 = int.Parse(idD);

        Dependency d = new Dependency(dependency2.Id, id1, id2);
        return d;
    }
}
