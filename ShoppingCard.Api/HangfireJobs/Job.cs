namespace ShoppingCard.Api.HangfireJobs
{
    public static class Job
    {
        public static void SayHelloJob(string name)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"hello {name}");
            Console.ResetColor();
        }
    }
}
