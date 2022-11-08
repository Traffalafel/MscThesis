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

        static async Task Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: <results directory path> <specification file path>");
                return;
            }

            // Load arguments
            var resultsDirPath = args[0];            
            var specificationFilePath = args[1];

            if (!File.Exists(specificationFilePath))
            {
                Console.WriteLine($"Could not find specification file: {specificationFilePath}");
                return;
            }
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
            var tspLibPath = ConfigurationManager.AppSettings["TSPLibDirectoryPath"];
            var runner = new TestProvider(tspLibPath);
            var specJson = File.ReadAllText(specificationFilePath);
            var spec = JsonConvert.DeserializeObject<TestSpecification>(specJson);
            var test = runner.Build(spec);
            using var source = new CancellationTokenSource();
            Console.CancelKeyPress += async (_, e) =>
            {
                Console.WriteLine($"Cancelling test execution");
                source.Cancel();
                e.Cancel = true;

                await Task.Delay(2000); // sleep 1 second
                
                // Dump results in file
                var resultContent = ResultExporter.Export(test, spec);
                var dumpedFilePath = Path.Join(resultsDirPath, $"{fileName}_dumped.{_resultExtension}");
                File.WriteAllText(dumpedFilePath, resultContent);

                Environment.Exit(-1);
            };

            // Run test
            Console.WriteLine($"Running test from spec file {fileName}");
            try
            {
                await test.Execute(source.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred when running the test(s). Exception message:\n{e.Message}");
                Console.WriteLine($"Stack trace:\n{e.StackTrace}");
                return;
            }

            // Save results
            var resultContent = ResultExporter.Export(test, spec);
            File.WriteAllText(outputFilePath, resultContent);

            Environment.Exit(0);
        }

    }
}
