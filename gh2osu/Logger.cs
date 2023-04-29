namespace gh2osu
{
    internal static class Logger
    {
        private static void Log(string message)
        {
            System.Console.WriteLine(message);
        }

        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Log("[INFO]  " + message);
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log("[WARN]  " + message);
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log("[ERROR] " + message);
        }
    }
}
