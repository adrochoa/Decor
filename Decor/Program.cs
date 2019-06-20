using System;
using System.Collections.Generic;
using System.Reflection;

namespace Decor
{
    static class Program
    {
        static void Main(string[] args)
        {
            List<string> methodStrings = (new List<MethodInfo>(typeof(Program).GetMethods()))
                .FindAll(x => x.IsStatic && x.Name != "Main").ConvertAll(x => x.Name);

            string prompt = "prompt> ";
            string startupMsg = "You can \"Regress\" all methods or Tab to cycle through and auto-complete method handles. Type \"quit\" to quit.";

            Func<string, List<char>, List<string>, string> lambda = ((strCmd, charList, strList) =>
            {
                if (strCmd.ToLower() == "quit")
                {
                    return "quit";
                }

                return strCmd == "Regress" ? TryRegress(methodStrings) : TryCallMethod(strCmd);
            });

            InteractivePrompt.Run(lambda, prompt, startupMsg, methodStrings);
        }

        private static string TryRegress(List<string> handles)
        {
            foreach (var mstr in handles)
            {
                TryCallMethod(mstr);
            }
            return "completed regression\n";
        }

        private static string TryCallMethod(string handle)
        {
            try
            {
                typeof(Program).GetMethod(handle).Invoke(null, null);
                return $"{handle} executed.\n";
            }
            catch
            {
                return $"{handle} not found.\n";
            }
        }

        public static void HelloWorld()
        {
            Console.WriteLine("Hello World!");
        }

        public static void GoodbyeWorld()
        {
            Console.WriteLine("Goodbye World!");
        }

        public static void ThisMethod()
        {
            Console.WriteLine("You called this method (\"ThisMethod\")!");
        }
    }
}
