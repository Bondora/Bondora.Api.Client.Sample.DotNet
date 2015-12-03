using System;

namespace Bondora.Api.Client.Sample.DotNet
{
    public static class Logger
    {
        public static void LogError(string msg, params object[] args)
        {
            SetColor(ConsoleColor.Red);
            Log(msg, args);
        }

        public static void LogWarning(string msg, params object[] args)
        {
            SetColor(ConsoleColor.Yellow);
            Log(msg, args);
        }

        public static void LogSuccess(string msg, params object[] args)
        {
            SetColor(ConsoleColor.Green);
            Log(msg, args);
        }

        public static void LogInfo(string msg, params object[] args)
        {
            SetColor(ConsoleColor.White);
            Log(msg, args);
        }

        private static void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void Log(string msg, params object[] args)
        {
            if (args.Length == 0)
                Console.WriteLine(msg);
            else
                Console.WriteLine(msg, args);
        }
    }
}
