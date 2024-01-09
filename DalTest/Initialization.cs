namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static IWorker? s_dalWorker;
    private static ITask? s_dalTask;
    private static IDependency? s_dalDependency;
    private static readonly Random s_rand = new();

    private static void createWorker()
    {
        string[] nameWorkers = { "Avital", "Ayala", "Chagai", "Maor", "Shlomo", "Hadar", "Yehuda", "Sheli", "Nachum", "Ariel"};
        string[] emailWorkers = { "avital@gmail.com","ayala@gmail.com","chagai@gmail.co.il","maor@gmail.co.il","shlomo@gmail.com","hadar@gmail.co.il","yehuda@gmail.com","sheli@gmail.co.il","nachum@gmail.com","ariel@gmail.co.il" };
       // int id=s_rand.Next(200000000, 400000000);
        for (int i = 0; i < nameWorkers.Length; i++)//we will go through the name database
        {
         //   while (s_dalWorker!.Read(id) is not null)//it will continue to generate id until it reaches one that does not yet exist
         //   {
               int id = s_rand.Next(200000000, 400000000);
          //  }
            Rank rank = (Rank)s_rand.Next(0, 5);
            double price = s_rand.Next(0, 1000);
            Worker worker=new Worker(id, rank, price, nameWorkers[i], emailWorkers[i]);
            s_dalWorker!.Create(worker);
        }
    }
    private static void createTask()
    {
        string[] nameTasks = {
        "Choose Destination",//1
        "Set a Budget",//2
        "Pack for the Trip",//3
        "Set Up Communication Methods",//4
        "Create a Packing List",//5
        "Arrange Currency Exchange",//6
        "Learn Basic Phrases in Local Language",//7
        "Book Travel Insurance",//8
        "Prepare Travel Documents",//9
        "Book Airport Transfers",//10
        "Notify Banks and Credit Cards of Travel",//11
        "Check-in for Flights",//12
        "Research Visa Requirements",//13
        "Book Flights and Accommodation",//14
        "Plan Activities and Itinerary",//15
        "Research Transportation Options",//16
        "Research Local Customs and Etiquette",//17
        "Research Emergency Services",//18
        "Research Travel Restrictions and Requirements",//19
        "Download Offline Maps and Information"//20
        };
        string[] descriptionTasks = {
        " Select the desired country or city to visit",//1
        "Determine the total amount available for travel expenses",//2
        "Gather necessary clothing, toiletries, travel essentials, and medications",//3
        "Inform friends and family of travel plans and arrange communication methods (e.g., phone, internet)",//4
        "List all items needed for the trip to ensure nothing is forgotten",//5
        "Obtain the local currency of the destination country",//6
        "Familiarize yourself with essential phrases in the destination language",//7
        "Purchase travel insurance to cover medical expenses, cancellations, and emergency situations",//8
        "Gather passports, visas, travel insurance, and other necessary documents",//9
        "Arrange transportation to and from the airport at each destination",//10
        "Inform financial institutions of travel plans to avoid card blocks",//11
        "Complete online check-in for flights to save time at the airport",//12
        "Determine if a visa is needed and gather application requirements",//213
        "Secure flights and accommodation within the budget",//14
        "Research attractions, events, and tours, and create a daily schedule",//15
        "Explore local transportation options (e.g., public transit, taxis, ride-sharing, car rentals)",//16
        "Learn about cultural norms, dress codes, and expectations to avoid misunderstandings",//117
        "Note emergency contact numbers and procedures for the destination country",//18
        "Stay informed about any COVID-19-related restrictions or health requirements for the destination",//19
        "Save maps and important information to devices for offline access"//20
        };
        string?[] remarksTasks = { 
        "Consider factors like interests, budget, travel time, and visa requirements",//1
        "Account for flights, accommodation, activities, food, transportation, and emergencies",//2
        "Consider weather conditions, cultural norms, and activity requirements",//3
        null,//14
        null,//15
        "Compare exchange rates and fees at banks or currency exchange bureaus",//6
        "Use language learning apps, books, or online resources",//7
        null,//8
        "Make copies and store them securely",//9
        null,//10
        null,//11
        null,//12
        "Check official government websites for accurate information",//13
        "Compare prices across different platforms and consider travel insurance",//14
        "Allocate time for travel between destinations, rest, and flexibility",//15
        "Consider cost, convenience, and safety when making decisions",//16
        null,//17
        null,//18
        null,//19
        null//120
        };
        string[] productTasks = { 
        "Chosen destination",//1
        "Detailed budget breakdown",//2
        "Packed suitcase or travel bag",//3
        "Established communication methods",//4
        "Comprehensive packing list",//5
        "Local currency",//6
        "Basic understanding of common phrases",//7
        "Travel insurance policy",//8
        "Organized travel documents",//9
        "Confirmed airport transfer bookings",//10
        "Updated travel notifications for banks and credit cards",//11
        "Checked-in flight confirmations",//12
        "Visa application information (if applicable)",//13
        "Confirmed flight and accommodation bookings",//14
        "Detailed itinerary with activity descriptions",//15
        "Transportation plan for each destination",//16
        "Knowledge of local customs and etiquette",//17
        "List of emergency contacts and procedures",//18
        "Updated knowledge of travel restrictions",//19
        "Offline maps and travel information"//20
        };
        Rank difficult=(DO.Rank)0;
        for (int i = 0; i < 20; i++)
        {
            if (i < 5)
                difficult = (DO.Rank)0;
            else if (i < 12)
                difficult = (DO.Rank)1;
            else 
                difficult=(DO.Rank)2;
            Random rand = new Random(DateTime.Now.Millisecond);
            DateTime start = new DateTime(2024, 2, 9, 0, 0, 0);
            int rangestart = (start - DateTime.Today).Days;
            DateTime RanDay=start.AddDays(rand.Next(rangestart));
            DateTime createProject = RanDay;
            string name = nameTasks[i];
            string desciption = descriptionTasks[i];
            string? remark = remarksTasks[i];
            string product = productTasks[i];
            int timeTask=s_rand.Next(0,24);
            Task task = new Task(0, difficult, 0, desciption, false, name, createProject, null, null, timeTask, null, null, remark, product);
            s_dalTask!.Create(task);

        }
    }
    private static void createDependency()
    {
        s_dalDependency!.Create(new Dependency(0, 4, 1));
        s_dalDependency.Create(new Dependency(0, 19, 1));
        s_dalDependency.Create(new Dependency(0, 20, 1));
        s_dalDependency.Create(new Dependency(0, 8, 1));
        s_dalDependency.Create(new Dependency(0, 6, 1));
        s_dalDependency.Create(new Dependency(0, 7, 1));
        s_dalDependency.Create(new Dependency(0, 14, 1));
        s_dalDependency.Create(new Dependency(0, 18, 1));
        s_dalDependency.Create(new Dependency(0, 17, 1));
        s_dalDependency.Create(new Dependency(0, 14, 2));
        s_dalDependency.Create(new Dependency(0, 8, 2));
        s_dalDependency.Create(new Dependency(0, 1, 2));
        s_dalDependency.Create(new Dependency(0, 9, 13));
        s_dalDependency.Create(new Dependency(0, 1, 13));
        s_dalDependency.Create(new Dependency(0, 9, 14));
        s_dalDependency.Create(new Dependency(0, 10, 14));
        s_dalDependency.Create(new Dependency(0, 11, 14));
        s_dalDependency.Create(new Dependency(0, 12, 14));
        s_dalDependency.Create(new Dependency(0, 5, 15));
        s_dalDependency.Create(new Dependency(0, 3, 15));
        s_dalDependency.Create(new Dependency(0, 2, 15));
        s_dalDependency.Create(new Dependency(0, 15, 16));
        s_dalDependency.Create(new Dependency(0, 5, 17));
        s_dalDependency.Create(new Dependency(0, 3, 17));
        s_dalDependency.Create(new Dependency(0, 20, 2));
        s_dalDependency.Create(new Dependency(0, 20, 13));
        s_dalDependency.Create(new Dependency(0, 4, 2));
        s_dalDependency.Create(new Dependency(0, 4, 13));
        s_dalDependency.Create(new Dependency(0, 19, 2));
        s_dalDependency.Create(new Dependency(0, 19, 13));
        s_dalDependency.Create(new Dependency(0, 17, 2));
        s_dalDependency.Create(new Dependency(0, 17, 13));
        s_dalDependency.Create(new Dependency(0, 18, 2));
        s_dalDependency.Create(new Dependency(0, 18, 13));
        s_dalDependency.Create(new Dependency(0, 14, 2));
        s_dalDependency.Create(new Dependency(0, 14, 13));
        s_dalDependency.Create(new Dependency(0, 7, 2));
        s_dalDependency.Create(new Dependency(0, 7, 13));
        s_dalDependency.Create(new Dependency(0, 6, 2));
        s_dalDependency.Create(new Dependency(0, 6, 13));
        s_dalDependency.Create(new Dependency(0, 8, 2));
        s_dalDependency.Create(new Dependency(0, 8, 13));
    }
    public static void Do(IWorker? dalWorker, ITask? dalTask, IDependency? dalDependency)
    {
        s_dalWorker = dalWorker ?? throw new NullReferenceException("DAL can not be null!");
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");
        createWorker();
        createTask();
        createDependency(); 
    }
}
