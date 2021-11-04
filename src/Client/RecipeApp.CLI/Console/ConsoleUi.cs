using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace RecipeApp.CLI.Console
{
    public interface IConsoleUi
    {
        public void Clear();
        public void WriteLine(string value = null);
        public void WaitForEnter();
        public void PrintObject(object json);
        public void Spacer(int? size = null);
        public bool GetBoolFromUser(string prompt, bool defaultValue = true);
        public string GetStringFromUser(string prompt);
        public int GetPositiveIntFromUser(string prompt);
        public decimal GetDecimalFromUser(string prompt);
        public string GetOptionFromUser(string prompt, List<string> options, bool retry = true, string quitString = "q");
        public string GetOptionFromUserAllowCustom(string prompt, List<string> options);
    }

    public class ConsoleUi : IConsoleUi
    {
        private string header;
        private int headerWidth;
        public ConsoleUi(string appName, int width = 20)
        {
            headerWidth = width;
            header = GetHeader(appName);
            Clear();
        }

        public void Clear()
        {
            System.Console.Clear();
            System.Console.WriteLine(header);
        }

        public void WriteLine(string value = null)
        {
            System.Console.WriteLine(value);
        }

        public void PrintObject(object json)
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(json, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() }));
        }

        public void Spacer(int? size = null)
        {
            if (size == null)
            {
                size = headerWidth;
            }
            var spacer = string.Empty;
            for(int i = 0; i < size; i++)
            {
                spacer += "-";
            }
            System.Console.WriteLine(spacer);
        }

        public bool GetBoolFromUser(string prompt, bool defaultValue = true)
        {
            System.Console.WriteLine(prompt);
            var input = System.Console.ReadLine();
            if (input.ToLower().StartsWith("y"))
            {
                return true;
            }
            else if (input.ToLower().StartsWith("n")){
                return false;
            }
            else
            {
                return defaultValue;
            }
        }

        public string GetStringFromUser(string prompt)
        {
            System.Console.WriteLine(prompt);
            var input = System.Console.ReadLine();
            return input;
        }

        public int GetPositiveIntFromUser(string prompt)
        {
            System.Console.WriteLine(prompt);
            var inputString = System.Console.ReadLine();
            try
            {
                var input = int.Parse(inputString);
                if (input < 0)
                {
                    input = 0;
                }
                return input;
            }
            catch
            {
                return 0;
            }
        }

        public decimal GetDecimalFromUser(string prompt)
        {
            System.Console.WriteLine(prompt);
            var inputString = System.Console.ReadLine();
            try
            {
                var input = decimal.Parse(inputString);
                return input;
            }
            catch
            {
                return 0;
            }
        }

        public string GetOptionFromUser(string prompt, List<string> options, bool retry = true, string quitString = "q")
        {
            var output = string.Empty;
            do
            {
                System.Console.WriteLine(prompt);
                System.Console.WriteLine();
                for (int i = 0; i < options.Count; i++)
                {
                    System.Console.WriteLine($"\t{i + 1}: {options[i]}");
                }
                if (retry)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine($"\t({quitString} to quit)");
                }
                var input = System.Console.ReadLine();
                foreach (var option in options)
                {
                    if (input.ToLower() == option.ToLower())
                    {
                        output = option;
                        retry = false;
                    }
                }
                try
                {
                    var inputNum = int.Parse(input);
                    if (inputNum > 0 && inputNum <= options.Count)
                    {
                        output = options[inputNum - 1];
                        retry = false;
                        System.Console.WriteLine(output);
                    }
                }
                catch
                {
                    //do nothing, handle as string
                }
                if (retry && input == quitString)
                {
                    retry = false;
                }
                else if (retry)
                {
                    System.Console.WriteLine("\nInvalid Selection\n");
                }
            } while (retry);

            return output;
        }

        public string GetOptionFromUserAllowCustom(string prompt, List<string> options)
        {
            var output = string.Empty;
            System.Console.WriteLine(prompt);
            for (int i = 0; i < options.Count; i++)
            {
                System.Console.WriteLine($"\t{i + 1}: {options[i]}");
            }
            System.Console.WriteLine();
            var input = System.Console.ReadLine();
            foreach (var option in options)
            {
                if (input.ToLower() == option.ToLower())
                {
                    output = option;
                }
            }
            try
            {
                var inputNum = int.Parse(input);
                if (inputNum > 0 && inputNum <= options.Count)
                {
                    output = options[inputNum - 1];
                    System.Console.WriteLine(output);
                }
            }
            catch
            {
                //do nothing, handle as string
            }
            if (output == string.Empty)
            {
                output = input;
            }
            return output;
        }

        public void WaitForEnter()
        {
            System.Console.WriteLine("\nPress 'Enter' to continue");
            System.Console.ReadLine();
        }

        private string GetHeader(string appName)
        {
            var spacerCharacter = "-";
            var spacer = string.Empty;
            for (int i = 0; i < headerWidth; i++)
            {
                spacer += spacerCharacter;
            }

            var totalSpaceToFill = headerWidth - appName.Length;
            if (totalSpaceToFill < 2)
            {
                totalSpaceToFill = 2;
            }
            var spaceToFillOnOneSide = totalSpaceToFill / 2;
            var extraSpace = false;
            if (spaceToFillOnOneSide * 2 != totalSpaceToFill)
            {
                extraSpace = true;
            }

            var appNameSpacer = appName;
            for (int i = 0; i < spaceToFillOnOneSide; i++)
            {
                appNameSpacer = $"{spacerCharacter}{appNameSpacer}{spacerCharacter}";
            }
            if (extraSpace)
            {
                appNameSpacer += spacerCharacter;
            }
            return spacer + "\n" + appNameSpacer + "\n" + spacer;
        }
    }
}
