using System;
using System.Threading.Tasks;

namespace TelegramHistory
{
    public static class AsyncTask
    {
        public static bool RunAndWait(Func<Task> operation)
        {
            try
            {
                Task.Run(operation).Wait();
                return true;
            }
            // ReSharper disable once RedundantCatchClause
            catch (AggregateException ex)
            {
#if DEBUG
                throw;
#else
                foreach (var inner in ex.InnerExceptions)
                {
                    Console.WriteLine("Unhandled exception: " + inner.Message);
                }

                Console.WriteLine("Press any key to quit");
                Console.ReadKey();

                return false;
#endif
            }
        }
    }
}