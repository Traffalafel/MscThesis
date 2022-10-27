using MscThesis.Core;
using MscThesis.Runner;
using MscThesis.Runner.Specification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.CLI
{
    internal class Program
    {
        private static string _resultExtension = "txt";

        static void Main(string[] args)
        {
            // Load settings
            var settings = new Settings
            {
                TSPLibDirectoryPath = ConfigurationManager.AppSettings["TSPLibDirectoryPath"],
                
            };

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: <specification file path> <results directory path>");
                return;
            }

            // Load arguments
            var specificationFilePath = args[0];
            if (!File.Exists(specificationFilePath))
            {
                Console.WriteLine($"Could not find specification file: {specificationFilePath}");
                return;
            }
            var resultsDirPath = args[1];            
            if (!Directory.Exists(resultsDirPath))
            {
                Console.WriteLine($"Results directory path does not exist: {resultsDirPath}");
                return;
            }
            var fileName = Path.GetFileNameWithoutExtension(specificationFilePath);
            var outputFilePath = Path.Join(resultsDirPath, $"{fileName}.{_resultExtension}");
            if (File.Exists(outputFilePath))
            {
                Console.WriteLine($"Results file already exists: {outputFilePath}");
                return;
            }

            // Build test
            var runner = new TestProvider(settings);
            var specJson = File.ReadAllText(specificationFilePath);
            var spec = JsonConvert.DeserializeObject<TestSpecification>(specJson);
            var test = runner.Build(spec);
            using var source = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                Console.WriteLine($"Cancelling test execution");
                source.Cancel();
                e.Cancel = true;
            };

            // Run test
            Console.WriteLine($"Running test from spec file {fileName}");
            var task = Task.Run(async () => await test.Execute(source.Token));
            task.Wait();
            if (task.IsFaulted)
            {
                Console.WriteLine($"An error occurred when running the test(s). Exception message:\n{task.Exception.Message}");
                Console.WriteLine($"Stack trace:\n{task.Exception.StackTrace}");
                return;
            }

            // Save results
            var resultContent = ResultExporter.Export(test, spec);
            File.WriteAllText(outputFilePath, resultContent);
        }

    }
}
