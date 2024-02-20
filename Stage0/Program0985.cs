partial class Program
{
    private static void Main(string[] args)
    {
        //Console.WriteLine("Hello, World!");
        Welcome0985();
        Welcome6950();
        Console.ReadKey();
    }
    static partial void Welcome6950();
    private static void Welcome0985()
    {
        Console.WriteLine("Enter your name: ");
        string? name = Console.ReadLine();
        Console.WriteLine(name + ", welcome to my first consile application");
    }
}

