using MscThesis.Core.Formats;
using MscThesis.Runner.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MscThesis.Runner.Tests
{
    public abstract class TestComposite<T> : Test<T> where T : InstanceFormat
    {
        private List<ITest<T>> _results;
        private int _maxParallel;

        public TestComposite(List<ITest<T>> results, int maxParallel)
        {
            _results = results;
            _maxParallel = maxParallel;
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            var initial = _results.Take(_maxParallel);
            var remaining = _results.Skip(_maxParallel).ToList();

            var tasks = initial.Select(async result =>
            {
                await result.Execute(cancellationToken);
                return result;
            }).ToList();

            while (tasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(tasks);
                var result = completedTask.Result;

                if (cancellationToken.IsCancellationRequested)
                {
                    return; // stop execution
                }

                TryUpdateFittest(result.Fittest?.Value);

                ConsumeResult(result);

                tasks.Remove(completedTask);
                if (remaining.Any())
                {
                    var first = remaining.First();
                    remaining.Remove(first);
                    tasks.Add(Task.Run(async () =>
                    {
                        await first.Execute(cancellationToken);
                        return first;
                    }));
                }
            }

            _isTerminated = true;
        }

        protected abstract void ConsumeResult(ITest<T> result);
    }
}
