using BlApi;
using DalApi;
using DalTest;
using DO;

namespace BlTest;
internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();// Initializing the business logic API instance
    static void Main(string[] args)
    {
        try // Handling exceptions that may occur during program execution
        {
            chooseEntities(); // Calling the method to choose entities
        }
        catch (Exception ex) // Catching any unhandled exceptions
        {
            Console.WriteLine(ex.Message); // Displaying the exception message
        }
    }

    private static void chooseEntities() // Method to choose entities
    {
        int entity = 1; // Initializing the entity variable
        while (entity != 0) // Loop to choose entities until 0 is entered
        {
            if (BlApi.Factory.Get().GetStartProjectDate() == null) // Checking if the start project date is not set
            {
                Console.WriteLine("enter the day, month, and year of the start date of the project");
                int.TryParse(Console.ReadLine(), out int day); // Parsing the day input
                int.TryParse(Console.ReadLine(), out int month); // Parsing the month input
                int.TryParse(Console.ReadLine(), out int year); // Parsing the year input
                DateTime date = new DateTime(year, month, day); // Creating a DateTime object with the entered date
                BlApi.Factory.Get().SetStartProjectDate(date); // Setting the start project date
            }
            // Prompting the user to choose an entity
            Console.WriteLine("Choose the entity that you want to check");
            Console.WriteLine("Enter 1-Worker\n 2-Task\n  3-data initialization\n 0-Exit");
            entity = int.Parse(Console.ReadLine()!); // Reading the user's choice
            switch (entity) // Switching based on the chosen entity
            {
                case 0: // Exiting the program
                    break;
                case 1: // Handling Worker entity
                    crudWorker("Worker"); // Calling the method to perform CRUD operations on Worker
                    break;
                case 2: // Handling Task entity
                    crudTask("Task"); // Calling the method to perform CRUD operations on Task
                    break;
                case 3: // Handling data initialization
                    Console.Write("Would you like to create Initial data? (Y/N)"); // Prompting the user
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); // Reading the user's response
                    if (ans == "Y") // If the user chooses to create initial data
                    {
                        s_bl!.Worker.clear(); // Clearing worker data
                        s_bl!.Task.clear(); // Clearing task data
                        Initialization.Do(); // Initializing data
                    }
                    break;
            }
        }
    }
    // Method to perform CRUD operations on Worker entity
    private static void crudWorker(String entity)
    {
        // Prompting the user to choose a method for Worker entity
        Console.WriteLine("Choose the method that you want to do for the Worker");
        Console.WriteLine("Enter 0-Exit\n 1-Create\n 2-Read\n 3-ReadAll\n 4-update\n 5-Delete");

        int.TryParse(Console.ReadLine(), out int choise);
        try // Handling exceptions that may occur during CRUD operations
        {
            switch (choise)
            {
                case 0://exit
                    break;
                case 1: // Creating a new worker
                    BO.Worker worker = createWorker(); // Calling the method to create a worker
                    s_bl!.Worker.AddWorker(worker); // Adding the worker using the business logic API
                    break;
                case 2: // Reading a worker
                    Console.WriteLine("Enter id"); // Prompting the user to enter the worker id
                    int.TryParse(Console.ReadLine(), out int idR); // Reading the user's input
                    BO.Worker? workerR = s_bl!.Worker.Read(idR); // Reading the worker from the business logic API
                    if (workerR is null) // Checking if the worker does not exist
                        throw new BO.BlDoesNotExistException($"Worker with ID={idR} does not exist"); // Throwing an exception
                    Console.WriteLine(workerR); // Displaying the worker information
                    break;
                case 3: // Reading all workers
                    IEnumerable<BO.Worker?> listWorkers = s_bl!.Worker.ReadWorkers(); // Reading all workers from the business logic API
                    foreach (BO.Worker? work in listWorkers) // Looping through each worker
                    {
                        if (work != null) // Checking if the worker is not null
                            Console.WriteLine(work); // Displaying the worker information
                    }
                    break;
                case 4: // Updating a worker
                    Console.WriteLine("Enter the id of the worker that you want to update"); // Prompting the user to enter the worker id
                    int.TryParse(Console.ReadLine(), out int idU); // Reading the user's input
                    BO.Worker? workerU = s_bl!.Worker.Read(idU); // Reading the worker from the business logic API
                    if (workerU is null) // Checking if the worker does not exist
                        throw new BO.BlDoesNotExistException($"Worker with ID={idU} does not exist"); // Throwing an exception
                    Console.WriteLine(workerU); // Displaying the worker information
                    BO.Worker w = updateWorker(workerU); // Calling the method to update the worker
                    s_bl!.Worker.UpdateWorker(w); // Updating the worker using the business logic API
                    break;
                case 5: // Deleting a worker
                    Console.WriteLine("Enter the id of the worker that you want to delete"); // Prompting the user to enter the worker id
                    int.TryParse(Console.ReadLine(), out int idD); // Reading the user's input
                    s_bl!.Worker.RemoveWorker(idD); // Removing the worker using the business logic API
                    break;
            }
        }
        catch (Exception ex) // Catching any unhandled exceptions
        {
            Console.WriteLine(ex.Message); // Displaying the exception message
        }

    }
    // Method to perform CRUD operations on Task entity
    private static void crudTask(String entity)
    {
        // Prompting the user to choose a method for Task entity
        Console.WriteLine("Choose the method that you want to do for the Task");
        Console.WriteLine("Enter 0-Exit\n 1-Create\n 2-Read\n 3-ReadAll\n 4-update\n 5-Delete");
        int.TryParse(Console.ReadLine(), out int choise); // Reading the user's choice
        try // Handling exceptions that may occur during CRUD operations
        {
            switch (choise) // Switching based on the chosen operation
            {
                case 0://exit
                    break;
                case 1://create task
                    BO.Task task = createTask(); // Calling the method to create a task
                    s_bl!.Task.AddTask(task); // Adding the task using the business logic API
                    break;
                case 2://print the task with the id that we enter
                    Console.WriteLine("Enter id"); // Prompting the user to enter the task id
                    int.TryParse(Console.ReadLine(), out int idR); // Reading the user's input
                    BO.Task? taskR = s_bl!.Task.Read(idR); // Reading the task from the business logic API
                    if (taskR is null) // Checking if the task does not exist
                        throw new BO.BlDoesNotExistException($"Task with ID={idR} does not exist"); // Throwing an exception
                    Console.WriteLine(taskR); // Displaying the task information
                    break;
                case 3://Print all the tasks
                    IEnumerable<BO.TaskInList?> listTasks = s_bl!.Task.ReadTasks(); // Reading all tasks from the business logic API
                    foreach (BO.TaskInList? task1 in listTasks) // Looping through each task
                    {
                        if (task1 != null) // Checking if the task is not null
                            Console.WriteLine(task1); // Displaying the task information
                    }
                    break;
                case 4://update the task
                    Console.WriteLine("Enter the id of the task that you want to update"); // Prompting the user to enter the task id
                    int.TryParse(Console.ReadLine(), out int idU); // Reading the user's input
                    BO.Task? taskU = s_bl!.Task.Read(idU); // Reading the task from the business logic API
                    if (taskU is null) // Checking if the task does not exist
                        throw new BO.BlDoesNotExistException($"Task with ID={idU} does not exist"); // Throwing an exception
                    Console.WriteLine(taskU); // Displaying the task information
                    BO.Task t = updateTask(taskU); // Calling the method to update the task
                    s_bl!.Task.UpdateTask(t); // Updating the task using the business logic API
                    break;
                case 5://delete the task with the id that we enter
                    Console.WriteLine("Enter the id of the task that you want to delete"); // Prompting the user to enter the task id
                    int.TryParse(Console.ReadLine(), out int idD); // Reading the user's input
                    s_bl!.Task.RemoveTask(idD); // Removing the task using the business logic API
                    break;
            }
        }
        catch (Exception ex) // Catching any unhandled exceptions
        {
            Console.WriteLine(ex.Message); // Displaying the exception message
        }
    }
    private static BO.Worker createWorker()
    {
        // Prompting the user to enter worker details
        Console.WriteLine("enter id, name, Email, level between 0-4, and cost");
        int.TryParse(Console.ReadLine(), out int id); // Reading worker id
        string name = Console.ReadLine()!; // Reading worker name
        string email = Console.ReadLine()!; // Reading worker email
        int.TryParse(Console.ReadLine(), out int level); // Reading worker level
        Rank rank = (Rank)level; // Converting level to Rank enum
        double.TryParse(Console.ReadLine(), out double cost); // Reading worker cost   
        // Creating a new worker objec
        BO.Worker worker = new BO.Worker()
        {
            Id = id,
            Name = name,
            Email = email,
            HourPrice = cost,
            RankWorker = (BO.Rank)rank,
            CurrentTask = null
        };
        return worker; // Returning the created worker object
    }
    private static BO.Task createTask()
    {
        // Prompting the user to enter task details
        Console.WriteLine("Enter name, difficulty between 0-4, task description, product, timeTask, remarks");
        String name = Console.ReadLine()!; // Reading task name
        int.TryParse(Console.ReadLine(), out int difficulty); // Reading task difficulty
        String taskDescription = Console.ReadLine()!; // Reading task description
        String product = Console.ReadLine()!; // Reading task product
        int.TryParse(Console.ReadLine(), out int timeTask); // Reading task time
        String? remarks = Console.ReadLine()!; // Reading task remarks
        if (remarks == "") // Checking if remarks are empty
            remarks = null; // Setting remarks to null
        DateTime createTask = DateTime.Now; // Setting creation date to current date
        BO.Rank rank = (BO.Rank)difficulty; // Converting difficulty to Rank enum
        List<BO.TaskInList> tasks = new List<BO.TaskInList>(); // Creating a list to store previous tasks
        Console.WriteLine("Do you have previous tasks of this task? Y/N");
        char answer = char.Parse(Console.ReadLine()!); // Reading user response
        if (answer == 'Y') // If user has previous tasks
        {
            Console.WriteLine("How many previous tasks do you have?"); // Prompting user to enter number of previous tasks
            int.TryParse(Console.ReadLine(), out int number); // Reading user input
            Console.WriteLine("Enter all the previous tasks of this task"); // Prompting user to enter previous tasks
            BO.Task previostask; // Declaring variable to store previous task
            BO.TaskInList taskInList; // Declaring variable to store previous task in list format
            for (int i = 0; i < number; i++) // Looping through each previous task
            {
                int.TryParse(Console.ReadLine(), out int previousid); // Reading previous task id
                previostask = s_bl.Task.Read(previousid); // Reading previous task from business logic API
                taskInList = new BO.TaskInList() // Creating TaskInList object for previous task
                {
                    Id = previousid,
                    Alias = previostask.Alias,
                    Description = previostask.TaskDescription,
                    StatusTask = previostask.StatusTask,
                };
                tasks.Add(taskInList);
            }
        }
          
        BO.Task? task = new BO.Task()
        {
            Id = 0,
            Difficulty = rank,
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
            DependencyTasks=tasks,
       
        };
        return task;
    }

    private static BO.Worker updateWorker(BO.Worker? worker2)
    {
        // Prompting the user to update worker details
        Console.WriteLine("Enter name, Email, level between 0-4, and cost");
        string name = Console.ReadLine()!; // Reading worker name
        if (name == "") { name = worker2!.Name!; } // If name is empty, use the existing name
        string email = Console.ReadLine()!; // Reading worker email
        if (email == "") { email = worker2!.Email!; } // If email is empty, use the existing email
        string level = Console.ReadLine()!; // Reading worker level
        BO.Rank rank; // Declaring variable to store worker rank
        if (level == "") { rank = worker2!.RankWorker; } // If level is empty, use the existing rank
        else { rank = (BO.Rank)(int.Parse(level)); } // Otherwise, parse level and convert to Rank enum
        double cost1; // Declaring variable to store worker cost
        string cost = Console.ReadLine()!; // Reading worker cost
        if (cost == "") { cost1 = worker2!.HourPrice; } // If cost is empty, use the existing cost
        else { cost1 = double.Parse(cost); } // Otherwise, parse cost                                       // Creating a new worker object with updated details
        BO.Worker worker = new BO.Worker()
        {
            Id = worker2!.Id, // Setting worker ID
            Name = name, // Setting worker name
            Email = email, // Setting worker email
            RankWorker = rank, // Setting worker rank
            HourPrice = cost1, // Setting worker cost
            CurrentTask = null // Setting worker current task to null
        };
        return worker; // Returning the updated worker object
    }
    private static BO.Task updateTask(BO.Task? task2)
    {
        // Prompting the user to update task details
        Console.WriteLine("Enter name, worker ID, difficulty between 0-4, task description, product, timeTask, remarks, begin work task");
        int id = task2!.Id; // Getting task ID
        string name = Console.ReadLine()!; // Reading task name
        if (name == "") // If name is empty
            name = task2.Alias!; // Use the existing alias
        string idw = Console.ReadLine()!; // Reading worker ID
        int workerId = 0; // Declaring variable to store worker ID
        if (idw == "") // If worker ID is empty
        {
            if (task2.Worker != null) // If task has a worker assigned
                workerId = task2.Worker.Id; // Get the existing worker ID
        }
        else
        {
            workerId = int.Parse(idw); // Otherwise, parse the input worker ID
        }
        string difficult = Console.ReadLine()!; // Reading task difficulty
        BO.Rank rank; // Declaring variable to store task difficulty
        if (difficult == "") { rank = task2.Difficulty; } // If difficulty is empty, use the existing difficulty
        else { rank = (BO.Rank)int.Parse(difficult); } // Otherwise, parse difficulty and convert to Rank enum
        string? taskDescription = Console.ReadLine()!; // Reading task description
        if (taskDescription == "") { taskDescription = task2.TaskDescription; } // If description is empty, use the existing description
        string? product = Console.ReadLine()!; // Reading task product
        if (product == "") { product = task2.Product; } // If product is empty, use the existing product
        string time = Console.ReadLine()!; // Reading task time
        int? timeTask; // Declaring variable to store task time
        if (time == "") // If time is empty
            timeTask = task2.TimeTask; // Use the existing time
        else
            timeTask = int.Parse(time); // Otherwise, parse time
        string? remarks = Console.ReadLine()!; // Reading task remarks
        if (remarks == "") { remarks = task2!.Remarks; } // If remarks are empty, use the existing remarks
        string? beginWork = Console.ReadLine(); // Reading begin work date
        DateTime? BeginWorkDate = null; // Declaring variable to store begin work date
        if (beginWork != "") // If begin work date is not empty
        {
            BeginWorkDate = DateTime.Parse(beginWork!); // Parse the input date
            s_bl.Task.UpdateStartDates(id, BeginWorkDate); // Update the start date of the task in the business logic API
        }
        if (BlApi.Factory.Get().GetStatusProject() == BO.StatusProject.Planning) // Checking if project status is Planning
        {
            // Creating a new task object with updated details
            BO.Task? task = new BO.Task()
            {
                Id = id, // Setting task ID
                Difficulty = rank, // Setting task difficulty
                TaskDescription = taskDescription, // Setting task description
                Alias = name, // Setting task alias
                CreateTask = task2.CreateTask, // Setting task creation date
                BeginTask = s_bl.Task.Read(id).BeginTask, // Setting begin task date
                BeginWork = s_bl.Task.Read(id).BeginWork, // Setting begin work date
                TimeTask = timeTask, // Setting task time
                DeadLine = s_bl.Task.Read(id).DeadLine, // Setting task deadline
                EndWorkTime = s_bl.Task.Read(id).EndWorkTime, // Setting end work time
                Remarks = remarks, // Setting task remarks
                Product = product, // Setting task product
                StatusTask = 0, // Setting task status to 0 (initial status)
                DependencyTasks = s_bl.Task.Read(id).DependencyTasks, // Setting task dependency tasks
            };
            return task;
        }
    }
}