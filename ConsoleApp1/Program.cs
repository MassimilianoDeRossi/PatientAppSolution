using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
  class Program
  {
    readonly static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);


    static void Main(string[] args)
    {
      Test().Wait();
    }


    static async Task Test()
    {
      int index = 0;
      while (true)
      {
        if (Console.KeyAvailable)
        {
          var key = Console.ReadKey();

          switch (key.Key)
          {
            case ConsoleKey.Enter:
              index++;
              Console.WriteLine("Starting work " + index);
              await DoWork(index);
              break;
          }
        }
      }
    }

    static async Task DoWork(int index)
    {
      Console.WriteLine("waiting at semaphore "+index);
      await semaphoreSlim.WaitAsync().ConfigureAwait(false); 
      Console.WriteLine("Doing work "+index);
      await Task.Delay(3000).ConfigureAwait(false);
      Task.Delay(3000);
      Console.WriteLine("Work done "+index);
      semaphoreSlim.Release();
      Console.WriteLine("Semaphore released "+index);

    }
  }
}
