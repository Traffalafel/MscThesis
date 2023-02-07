using MscThesis.Core.Formats;
using MscThesis.Runner.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Tests
{
    public abstract class TestComposite : Test
    {
        private IEnumerable<ITest> _subTests;
        private int _maxParallel;

        public TestComposite(List<ITest> subTests, int maxParallel)
        {
            _subTests = subTests;
            _maxParallel = maxParallel;
            _instanceType = subTests.First().InstanceType;
            _comparisonStrategy = subTests.First().Comparison;

            foreach (var test in subTests)
            {
                var optimizerNames = test.OptimizerNames;
                Initialize(optimizerNames);
                foreach (var name in optimizerNames)
                {
                    var observable = test.BestFitness(name);
                    observable.PropertyChanged += (s, e) =>
                    {
                        if (_comparisonStrategy.IsFitter(observable.Value, _bestFitness[name].Value))
                        {
                            _bestFitness[name].Value = observable.Value;
                        }
                    };
                }

                test.SetLock(SeriesLock);
            }
        }

        public TestComposite(Func<ITest> generateFunc, int numTests, int maxParallel)
        {
            _maxParallel = maxParallel;

            var empty = generateFunc();
            _instanceType = empty.InstanceType;
            _comparisonStrategy = empty.Comparison;
            Initialize(empty.OptimizerNames);

            _subTests = CreateEnumerable(generateFunc, numTests);
        }


        public override async Task Execute(CancellationToken cancellationToken)
        {
            var enumerator = _subTests.GetEnumerator();

            var initial = new List<ITest>();
            for (int i = 0; i < _maxParallel; i++)
            {
                enumerator.MoveNext();
                initial.Add(enumerator.Current);
            }

            var tasks = initial.Select(result => Task.Run(async () =>
            {
                await result.Execute(cancellationToken);
                return result;
            })).ToList();

            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);

                try
                {
                    var result = completedTask.Result;
                    TryUpdateFittest(result);
                    ConsumeResult(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("=== An exception occurred ===");
                    Console.WriteLine($"Type: {e.GetType().Name}");
                    Console.WriteLine($"Exception:\n{e}");
                    while (e.InnerException != null)
                    {
                        Console.WriteLine($"InnerException:{e.InnerException}");
                        e = e.InnerException;
                    }
                    Console.WriteLine("======");
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return; // stop execution
                }

                tasks.Remove(completedTask);
                if (enumerator.MoveNext())
                {
                    var test = enumerator.Current;
                    tasks.Add(Task.Run(async () =>
                    {
                        await test.Execute(cancellationToken);
                        return test;
                    }));
                }
            }

            _isTerminated = true;
        }

        protected abstract void ConsumeResult(ITest result);

        private IEnumerable<ITest> CreateEnumerable(Func<ITest> generate, int numTests)
        {
            var c = 0;
            while (c < numTests)
            {
                yield return CreateTest(generate);
                c++;
            }
            yield break;
        }

        private ITest CreateTest(Func<ITest> generate)
        {
            var test = generate();
            foreach (var name in test.OptimizerNames)
            {
                var observable = test.BestFitness(name);
                observable.PropertyChanged += (s, e) =>
                {
                    if (_comparisonStrategy.IsFitter(observable.Value, _bestFitness[name].Value))
                    {
                        _bestFitness[name].Value = observable.Value;
                    }
                };
            }
            test.SetLock(SeriesLock);
            return test;
        }

    }
}
