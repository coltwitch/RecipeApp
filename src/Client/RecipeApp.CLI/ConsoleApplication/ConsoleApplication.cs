using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.CLI
{
    public class ConsoleApplication
    {
        private IConsoleHelper _console;
        private Dictionary<string, IManagerHandler> _handlers;

        public ConsoleApplication(string appName, int width = 100)
        {
            _console = new ConsoleHelper(appName, width);
            _handlers = new Dictionary<string, IManagerHandler>();
        }

        #region Public Methods
        public ManagerHandler<T> RegisterManager<T>(T managerInstance)
        {
            var handler = new ManagerHandler<T>(_console, managerInstance);
            _handlers.Add(managerInstance.GetType().Name, handler);
            return handler;
        }

        public void RegisterCustomHandler(IManagerHandler handler)
        {
            _handlers.Add(handler.GetType().Name, handler);
        }

        public void Run()
        {
            var continueRunning = true;
            while (continueRunning)
            {
                _console.Clear();
                var options = _handlers?.Select(x => x.Key).ToList();
                if (options == null || options.Count == 0)
                {
                    _console.WriteLine("No handlers have been registered. Please use 'RegisterManager' or 'RegisterCustomHandler' to add a handler to the application");
                    return;
                }
                var response = _console.GetOptionFromUser("Which handler do you want to interact with?", options);
                _console.Navigate(response);
                try
                {
                    if (!string.IsNullOrEmpty(response))
                    {
                        var handler = _handlers[response];
                        if (handler != null)
                        {
                            handler.Run();
                        }
                    }
                    else
                    {
                        continueRunning = false;
                    }
                }
                catch (Exception ex)
                {
                    _console.Navigate("ERROR THROWN");
                    _console.PrintObject(ex);
                }
                if (continueRunning)
                {
                    _console.WaitForEnter();
                }
            }
        }
        #endregion

        #region ConsoleHelper

        public interface IConsoleHelper
        {
            public void Navigate(string step = null);
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
            public DateTime GetDateTimeFromUser(string prompt, bool setTimeOfDay = false);
        }

        public class ConsoleHelper : IConsoleHelper
        {
            private string header;
            private int headerWidth;
            private List<string> navPath;
            private string navSeparator = " -> ";
            public ConsoleHelper(string appName, int width = 20)
            {
                navPath = new List<string>();
                headerWidth = width;
                header = GetHeader(appName);
                Clear();
            }
            public void Navigate(string step = null)
            {
                if (string.IsNullOrEmpty(step))
                {
                    if (navPath.Count > 0)
                    {
                        step = navPath[^1];
                    }
                }
                if (navPath.Contains(step))
                {
                    var index = navPath.IndexOf(step);
                    var newPath = new List<string>();
                    for (int i = 0; i < index; i++)
                    {
                        newPath.Add(navPath[i]);
                    }
                    navPath = newPath;
                }
                if (!string.IsNullOrEmpty(step))
                {
                    navPath.Add(step);
                }

                Clear(false);
            }

            public void Clear()
            {
                Clear(true);
            }

            private void Clear(bool clearNav)
            {
                if (clearNav)
                {
                    navPath = new List<string>();
                }
                System.Console.Clear();
                WriteLine(header);
                if (navPath.Count > 0)
                {
                    WriteLine(string.Join(navSeparator, navPath));
                    Spacer();
                }
            }

            public void WriteLine(string value = null)
            {
                System.Console.WriteLine(value);
            }

            public void PrintObject(object json)
            {
                IEnumerable<object> enumerable = (json as IEnumerable<object>);
                if (enumerable != null)
                {
                    foreach (var obj in enumerable)
                    {
                        WriteLine(JsonConvert.SerializeObject(obj,
                            Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                        //Formatting.Indented, new JsonConverter[] { new StringEnumConverter() }));
                    }
                }
                else
                {
                    WriteLine(JsonConvert.SerializeObject(json,
                        Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    //Formatting.Indented, new JsonConverter[] { new StringEnumConverter() }));
                }
            }

            public void Spacer(int? size = null)
            {
                if (size == null)
                {
                    size = headerWidth;
                }
                var spacer = string.Empty;
                for (int i = 0; i < size; i++)
                {
                    spacer += "-";
                }
                WriteLine(spacer);
            }

            public bool GetBoolFromUser(string prompt, bool defaultValue = true)
            {
                WriteLine(prompt);
                var input = System.Console.ReadLine();
                if (input.ToLower().StartsWith("y"))
                {
                    return true;
                }
                else if (input.ToLower().StartsWith("n"))
                {
                    return false;
                }
                else
                {
                    return defaultValue;
                }
            }

            public string GetStringFromUser(string prompt)
            {
                WriteLine(prompt);
                var input = System.Console.ReadLine();
                return input;
            }

            public int GetPositiveIntFromUser(string prompt)
            {
                WriteLine(prompt);
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
                WriteLine(prompt);
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
                    WriteLine(prompt);
                    WriteLine();
                    for (int i = 0; i < options.Count; i++)
                    {
                        WriteLine($"\t{i + 1}: {options[i]}");
                    }
                    if (retry)
                    {
                        WriteLine();
                        WriteLine($"\t({quitString} to quit)");
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
                            WriteLine(output);
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
                        WriteLine("\nInvalid Selection\n");
                    }
                } while (retry);

                return output;
            }

            public string GetOptionFromUserAllowCustom(string prompt, List<string> options)
            {
                var output = string.Empty;
                WriteLine(prompt);
                for (int i = 0; i < options.Count; i++)
                {
                    WriteLine($"\t{i + 1}: {options[i]}");
                }
                WriteLine();
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
                        WriteLine(output);
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
                WriteLine("\nPress 'Enter' to continue");
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
                return spacer + "\n" + appNameSpacer + "\n" + spacer + "\n";
            }

            public DateTime GetDateTimeFromUser(string prompt, bool setTimeOfDay = false)
            {
                var year = GetPositiveIntFromUser("Enter four digit year");
                var month = GetPositiveIntFromUser("Enter two digit month");
                var day = GetPositiveIntFromUser("Enter two digit day");
                var hour = 0;
                var minute = 0;
                var second = 0;
                if (setTimeOfDay)
                {
                    hour = GetPositiveIntFromUser("Enter two digit hours");
                    minute = GetPositiveIntFromUser("Enter two digit minutes");
                    second = GetPositiveIntFromUser("Enter seconds");
                }
                try
                {
                    return new DateTime(year, month, day, hour, minute, second);
                }
                catch
                {
                    if (setTimeOfDay)
                    {
                        return DateTime.Now;
                    }
                    return DateTime.Today;
                }
            }
        }

        #endregion

        #region ManagerHandler
        public interface IManagerHandler
        {
            public void Run();
        }
        
        public class ManagerHandler<T> : IManagerHandler
        {
            private Dictionary<string, string> idList;
            private Dictionary<string, object> defaultList;
            private List<string> optionsList;
            private Dictionary<string, Func<T, object>> funcList;
            private IConsoleHelper _console;
            private T _manager;

            public ManagerHandler(IConsoleHelper console, T manager)
            {
                _console = console;
                _manager = manager;
                idList = new Dictionary<string, string>();
                optionsList = new List<string>();
                funcList = new Dictionary<string, Func<T, object>>();
                defaultList = new Dictionary<string, object>();

                Type managerType = typeof(T);
                var publicMethods = managerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var method in publicMethods)
                {
                    optionsList.Add(method.Name);
                    funcList.Add(method.Name, x => { return method.Invoke(_manager, GetParameters(method.GetParameters())); });
                }
            }

            public ManagerHandler<T> RegisterDefaultValue<U>(object value)
            {
                defaultList.TryAdd(typeof(U).FullName, value);
                return this;
            }

            public ManagerHandler<T> RegisterMethodAsIdReader(string idName, string methodName)
            {
                idList.TryAdd(idName, null);
                var managerType = _manager.GetType();
                var method = managerType.GetMethod(methodName);
                if (method != null)
                {
                    funcList[methodName] = x =>
                    {
                        var id = idList[idName];
                        var useStored = false;
                        if (!string.IsNullOrEmpty(id))
                        {
                            useStored = _console.GetBoolFromUser($"Use stored ID '{id}'? (y/n)");
                        }
                        if (!useStored)
                        {
                            id = _console.GetStringFromUser($"Enter '{idName}'");
                            idList[idName] = id;
                        }
                        var result = method.Invoke(_manager, new object[] { id });
                        return result;
                    };
                }
                return this;
            }

            public ManagerHandler<T> RegisterMethodAsIdSelector(string idName, string propertyName, string methodName)
            {
                idList.TryAdd(idName, null);
                var managerType = _manager.GetType();
                var method = managerType.GetMethod(methodName);
                if (method != null)
                {
                    funcList[methodName] = x =>
                    {
                        var result = method.Invoke(_manager, GetParameters(method.GetParameters()));
                        var options = ((IEnumerable<object>)result).ToList().Select(y => {
                            var resultType = y.GetType();
                            var propertyInfo = resultType.GetProperty(propertyName);
                            var idValue = propertyInfo.GetValue(y);
                            return idValue.ToString();
                        }).ToList();
                        var selection = _console.GetOptionFromUser($"Select which to store in '{idName}'", options);
                        idList[idName] = selection;
                        return result;
                    };
                }
                return this;
            }

            private object[] GetParameters(ParameterInfo[] parameters)
            {
                var output = new List<object>();
                foreach (var parameter in parameters)
                {
                    var type = parameter.ParameterType.FullName;
                    var name = parameter.Name;
                    object typeDefault = null;
                    try
                    {
                        typeDefault = defaultList[type];
                    }
                    catch
                    {
                        //do nothing
                    }
                    if (typeDefault != null)
                    {
                        var useDefault = _console.GetBoolFromUser($"Use default '{name}'? (y/n)");
                        if (useDefault)
                        {
                            output.Add(typeDefault);
                        }
                        else
                        {
                            output.Add(GetObjectFromUser(type, name));
                        }
                    }
                    else
                    {
                        output.Add(GetObjectFromUser(type, name));
                    }
                }
                return output.ToArray();
            }

            private object GetObjectFromUser(string typeName, string objectName)
            {
                _console.Navigate();
                object result = null;
                var type = Type.GetType(typeName);
                if (type == null)
                {
                    type = GetType().Assembly.GetTypes().Where(type => type.GetInterface(typeName) != null).FirstOrDefault();
                    if (type == null)
                    {
                        var ending = typeName.Split('.').Last();
                        var assemblyName = typeName.Replace($".{ending}", "");
                        var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);

                        type = assembly.GetTypes().Where(type => type.GetInterface(typeName) != null).FirstOrDefault();
                    }
                }
                    //var interfaceType = System.Reflection..GetInterface(typeName);
                    //var types = AppDomain.CurrentDomain.GetAssemblies()
                    //    .SelectMany(s => s.GetTypes())
                    //    .Where(p => type.IsAssignableFrom(p));

                if (result == null)
                {
                    if (type == typeof(string))
                    {
                        var prompt = $"Enter string value for '{objectName}'";
                        result = _console.GetStringFromUser(prompt);
                    }
                    else if (type == typeof(int))
                    {
                        var prompt = $"Enter positive integer value for '{objectName}'";
                        result = _console.GetPositiveIntFromUser(prompt);
                    }
                    else if (type == typeof(bool))
                    {
                        var prompt = $"Set bool value for '{objectName}' to 'true'? (y/n)";
                        result = _console.GetBoolFromUser(prompt);
                    }
                    else if (
                        type == typeof(decimal) ||
                        type == typeof(double))
                    {
                        var prompt = $"Enter decimal value for '{objectName}'";
                        result = _console.GetDecimalFromUser(prompt);
                    }
                    else if (type == typeof(DateTime))
                    {
                        var prompt = $"Enter datetime value for '{objectName}'";
                        result = _console.GetDateTimeFromUser(prompt);
                    }
                    else
                    {
                        _console.Navigate(objectName);
                        var publicMembers = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        object output;
                        try
                        {
                            output = Activator.CreateInstance(type);
                        }
                        catch
                        {
                            output = FormatterServices.GetUninitializedObject(type);
                        }
                        foreach (var publicMember in publicMembers)
                        {
                            if (publicMember.CanWrite)
                            {

                                var value = GetObjectFromUser(publicMember.PropertyType.FullName, publicMember.Name);
                                PropertyInfo propertyInfo = output.GetType().GetProperty(publicMember.Name);
                                try
                                {
                                    propertyInfo.SetValue(output, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                                }
                                catch
                                {
                                    //do nothing
                                }
                            }
                        }

                        result = output;
                    }
                }

                if (result == null)
                {
                    throw new NotImplementedException();
                }

                return result;
            }

            public void Run()
            {
                var selection = _console.GetOptionFromUser("Which endpoint do you want to interact with?", optionsList);
                if (!string.IsNullOrEmpty(selection))
                {
                    _console.Navigate(selection);
                    var result = funcList[selection].Invoke(_manager);
                    _console.Navigate(selection);
                    _console.PrintObject(result);
                }
            }
        }
        #endregion
    }

}