using BlApi;
using DalApi;
using DalTest;
using DO;

namespace BlTest;
internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        try
        {
            chooseEntities();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void chooseEntities()
    {
        int entity = 1;
        while (entity != 0)
        {
            Console.WriteLine("Choose the entity that you want to check");
            Console.WriteLine("Enter 1-Worker\n 2-Task\n  3-data initialization\n 0-Exit");
            entity = int.Parse(Console.ReadLine()!);
            switch (entity)
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
                    Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                    if (ans == "Y") //stage 3
                    {
                        //s_bl!.Worker.clear();
                        //s_bl!.Task.clear();
                        //s_bl!.Dependency.clear();
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

        int.TryParse(Console.ReadLine(), out int choise);
        try
        {
            switch (choise)
            {
                case 0://exit
                    break;
                case 1://create worker
                    BO.Worker worker = createWorker();
                    s_bl!.Worker.AddWorker(worker);
                    break;
                case 2://print the worker with the id that we enter
                    Console.WriteLine("Enter id");
                    int.TryParse(Console.ReadLine(), out int idR);
                    BO.Worker? workerR = s_bl!.Worker.Read(idR);
                    if (workerR is null)
                        throw new BO.BlDoesNotExistException($"Worker with ID={idR} does not exist");
                    Console.WriteLine(workerR);
                    break;
                case 3://Print all the workers
                    IEnumerable<BO.Worker?> listWorkers = s_bl!.Worker.ReadWorkers();
                    foreach (BO.Worker? work in listWorkers)
                    {
                        if (work != null)
                            Console.WriteLine(work);
                    }
                    break;
                case 4:
                    Console.WriteLine("Enter the id of the worker that you want to update");
                    int.TryParse(Console.ReadLine(), out int idU);
                    BO.Worker? workerU = s_bl!.Worker.Read(idU);
                    if (workerU is null)
                        throw new BO.BlDoesNotExistException($"Worker with ID={idU} does not exist");
                    Console.WriteLine(workerU);
                    BO.Worker w = updateWorker(workerU);
                    s_bl!.Worker.UpdateWorker(w);
                    break;
                case 5:
                    Console.WriteLine("Enter the id of the worker that you want to delete");
                    int.TryParse(Console.ReadLine(), out int idD);
                    s_bl!.Worker.RemoveWorker(idD);
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
        int.TryParse(Console.ReadLine(), out int choise);
        try
        {
            switch (choise)
            {
                case 0://exit
                    break;
                case 1://create task
                    BO.Task task = createTask();
                    s_bl!.Task.AddTask(task);
                    break;
                case 2://print the task with the id that we enter
                    Console.WriteLine("Enter id");
                    int.TryParse(Console.ReadLine(), out int idR);
                    BO.Task? taskR = s_bl!.Task.Read(idR);
                    if (taskR is null)
                        throw new BO.BlDoesNotExistException($"Task with ID={idR} does not exist");
                    Console.WriteLine(taskR);
                    break;
                case 3://Print all the tasks
                    IEnumerable<BO.TaskInList?> listTasks = s_bl!.Task.ReadTasks();
                    foreach (BO.TaskInList? task1 in listTasks)
                    {
                        if (task1 != null)
                            Console.WriteLine(task1);
                    }
                    break;
                case 4://update the task
                    Console.WriteLine("Enter the id of the task that you want to update");
                    int.TryParse(Console.ReadLine(), out int idU);
                    BO.Task? taskU = s_bl!.Task.Read(idU);
                    if (taskU is null)
                        throw new BO.BlDoesNotExistException($"Task with ID={idU} does not exist");
                    Console.WriteLine(taskU);
                    BO.Task t = updateTask(taskU);
                    s_bl!.Task.UpdateTask(t);
                    break;
                case 5://delete the task with the id that we enter
                    Console.WriteLine("Enter the id of the task that you want to delete");
                    int.TryParse(Console.ReadLine(), out int idD);
                    s_bl!.Task.RemoveTask(idD);
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private static BO.Worker createWorker()
    {
        Console.WriteLine("enter id, name,Email,level between 0-4 and cost");
        int.TryParse(Console.ReadLine(), out int id);
        string name = Console.ReadLine()!;
        string email = Console.ReadLine()!;
        int.TryParse(Console.ReadLine(), out int level);
        Rank rank = (Rank)level;
        double.TryParse(Console.ReadLine(), out double cost);
        BO.Worker worker = new BO.Worker()
        {
            Id = id,
            Name = name,
            Email = email,
            HourPrice = cost,
            RankWorker = (BO.Rank)rank,
            CurrentTask = null
        };
        return worker;
    }
    private static BO.Task createTask()
    {
        Console.WriteLine("Enter name, difficulty between 0-4, taskDescreption, product, timeTask, remarks");
        String name = Console.ReadLine()!;
        int.TryParse(Console.ReadLine(), out int difficulty);
        String taskDescreption = Console.ReadLine()!;
        String product = Console.ReadLine()!;
        int.TryParse(Console.ReadLine(), out int timeTask);
        String? remarks = Console.ReadLine()!;
        if (remarks == "")
            remarks = null;
        DateTime createTask = DateTime.Now;
        Rank rank = (Rank)difficulty;
        BO.Task? task = new BO.Task()
        {
            Id = 0,
            Difficulty = (BO.Rank)rank,
            TaskDescription = taskDescreption,
            Alias = name,
            CreateTask = createTask,
            BeginTask=null,
            BeginWork=null,
            TimeTask= timeTask,
            DeadLine=null,
            EndWorkTime=null,
            Remarks=remarks,
            Product=product,
            StatusTask=0,
            DependencyTasks=null,
       
        };
        return task;
    }

    private static BO.Worker updateWorker(BO.Worker? worker2)
    {
        Console.WriteLine("Enter name,Email,level between 0-4 and cost");
        string name = Console.ReadLine()!;
        if (name == "") { name = worker2!.Name!; }
        string email = Console.ReadLine()!;
        if (email == "") { email = worker2!.Email!; }
        string level = Console.ReadLine()!;
        BO.Rank rank;
        if (level == "") { rank = worker2!.RankWorker; }
        else { rank = (BO.Rank)(int.Parse(level)); }
        double cost1;
        string cost = Console.ReadLine()!;
        if (cost == "") { cost1 = worker2!.HourPrice; }
        else { cost1 = double.Parse(cost); }
        BO.Worker worker = new BO.Worker()
        {
            Id = worker2!.Id,
            Name = name,
            Email = email,
            RankWorker = rank,
            HourPrice=cost1,
            CurrentTask = null
        };
        return worker;
    }
    private static BO.Task updateTask(BO.Task? task2)
    {
        Console.WriteLine("Enter name, id worker, difficulty between 0-4, taskDescreption, product, timeTask, remarks,begin work task");
        int id = task2!.Id;
        string name = Console.ReadLine()!;
        if (name == "")
            name = task2.Alias!;
        string idw = Console.ReadLine()!;
        int workerId=0;
        if (idw == "")
        {
            if (task2.Worker != null) workerId = task2.Worker.Id;
        }
        else workerId = int.Parse(idw);
        string difficult = Console.ReadLine()!;
        BO.Rank rank;
        if (difficult == "") { rank = task2.Difficulty; }
        else { rank = (BO.Rank)int.Parse(difficult); }
        string? taskDescription = Console.ReadLine()!;
        if (taskDescription == "") { taskDescription = task2.TaskDescription; }
        string? product = Console.ReadLine()!;
        if (product == "") { product = task2.Product; }
        string time = Console.ReadLine()!;
        int? timeTask;
        if (time == "")
            timeTask = task2.TimeTask;
        else
            timeTask = int.Parse(time);
        string? remarks = Console.ReadLine()!;
        if (remarks == "") { remarks = task2!.Remarks; }
        string? beginWork = Console.ReadLine();
        DateTime? BeginWorkDate=null;
        if (beginWork != "")
        {
            BeginWorkDate = DateTime.Parse(beginWork!);
            s_bl.Task.UpdateDtartDates(id, BeginWorkDate);
        }
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning)
        {
            BO.Task? task = new BO.Task()
            {
                Id = id,
                Difficulty = rank,
                TaskDescription = taskDescription,
                Alias = name,
                CreateTask = task2.CreateTask,
                BeginTask = s_bl.Task.Read(id).BeginTask,
                BeginWork = s_bl.Task.Read(id).BeginWork,
                TimeTask = timeTask,
                DeadLine = s_bl.Task.Read(id).DeadLine,
                EndWorkTime = s_bl.Task.Read(id).EndWorkTime,
                Remarks = remarks,
                Product = product,
                StatusTask = 0,
                DependencyTasks = s_bl.Task.Read(id).DependencyTasks,
            };
            return task;
        }
        else
        {
            BO.Task? task = new BO.Task()
            {
                Id = id,
                Difficulty = rank,
                Worker = new BO.WorkerOnTask
                {
                    Id = workerId,
                    Name = s_bl.Worker.Read(workerId).Name
                },
                TaskDescription = taskDescription,
                Alias = name,
                CreateTask = task2.CreateTask,
                BeginTask = s_bl.Task.Read(id).BeginTask,
                BeginWork = s_bl.Task.Read(id).BeginWork,
                TimeTask = timeTask,
                DeadLine = s_bl.Task.Read(id).DeadLine,
                EndWorkTime = s_bl.Task.Read(id).EndWorkTime,
                Remarks = remarks,
                Product = product,
                StatusTask = 0,
                DependencyTasks = s_bl.Task.Read(id).DependencyTasks,
            };
            return task;
        }
    }
}