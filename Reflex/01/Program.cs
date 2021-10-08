using System;
using System.Reflection;

namespace ReflectionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Kevin";
            //var stringType = name.GetType();
            var stringType = typeof(string);
            Console.WriteLine(stringType);

            var currentAssembly = Assembly.GetExecutingAssembly();
            var typesFromCurrentAssembly = currentAssembly.GetTypes();
            foreach (var type in typesFromCurrentAssembly)
            {
                Console.WriteLine("Type: " + type.FullName);
            }

            var oneTypeFromCurrentAssembly = currentAssembly.GetType("ReflectionSample.Person");
            Console.WriteLine(oneTypeFromCurrentAssembly.Name);

            var externalAssembly = Assembly.Load("System.Text.Json");
            var typesFromExternalAssembly = externalAssembly.GetTypes();
            var oneTypeFromExternalAssembly = externalAssembly.GetType("System.Text.Json.JsonProperty");

            var modulesFromExternalAssembly = externalAssembly.GetModules();
            var oneModuleFromExternalAssembly = externalAssembly.GetModule("System.Text.Json.dll");

            var typesFromModuleFromExternalAssembly = oneModuleFromExternalAssembly.GetTypes();
            var oneTypeFromModuleFromExternalAssembly = 
                oneModuleFromExternalAssembly.GetType("System.Text.Json.JsonProperty");

            foreach (var constructor in oneTypeFromCurrentAssembly.GetConstructors())
            {  
                Console.WriteLine("constructor: " + constructor);
            }

            //foreach (var method in oneTypeFromCurrentAssembly.GetMethods())
            //{
            //    Console.WriteLine(method);
            //}

            foreach (var method in oneTypeFromCurrentAssembly.GetMethods(
                 BindingFlags.Public | BindingFlags.NonPublic))
            {
                Console.WriteLine($"{method}, public: {method.IsPublic}");
            }

            foreach (var field in oneTypeFromCurrentAssembly.GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic))
            {
                Console.WriteLine(field);
            }

            Console.ReadLine();
        }
    }
}
