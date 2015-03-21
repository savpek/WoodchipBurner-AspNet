using System;
using System.Reactive.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        public void Main(string[] args)
        {
            var source = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1)).Timestamp();

            using (source.Subscribe(x => Console.WriteLine("{0}: {1}", x.Value, x.Timestamp)))
            {
                Console.WriteLine("Press any key to unsubscribe");
            }

            Console.ReadLine();
        }
    }
}
