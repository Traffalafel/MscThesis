using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Tests
{
    public abstract class TestComposite<T> : Test<T> where T : InstanceFormat
    {
        private IEnumerable<ITest<T>> _subTests;
        private int _maxParallel;

        public TestComposite(List<ITest<T>> subTests, int maxParallel)
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
                    var observable = test.Fittest(name);
                    observable.PropertyChanged += (s, e) =>
                    {
                        if (_comparisonStrategy.IsFitter(observable.Value, _fittest[name].Value))
                        {
                            _fittest[name].Value = observable.Value;
                        }
                    };
                }

                test.SetLock(SeriesLock);
            }
        }

        public TestComposite(Func<ITest<T>> generateFunc, int numTests, int maxParallel)
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

            var initial = new List<ITest<T>>();
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

        protected abstract void ConsumeResult(ITest<T> result);

        private IEnumerable<ITest<T>> CreateEnumerable(Func<ITest<T>> generate, int numTests)
        {
            var c = 0;
            while (c < numTests)
            {
                yield return CreateTest(generate);
                c++;
            }
            yield break;
        }

        private ITest<T> CreateTest(Func<ITest<T>> generate)
        {
            var test = generate();
            foreach (var name in test.OptimizerNames)
            {
                var observable = test.Fittest(name);
                observable.PropertyChanged += (s, e) =>
                {
                    if (_comparisonStrategy.IsFitter(observable.Value, _fittest[name].Value))
                    {
                        _fittest[name].Value = observable.Value;
                    }
                };
            }
            test.SetLock(SeriesLock);
            return test;
        }

    }
}
