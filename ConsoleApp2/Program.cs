using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;

namespace ConsoleApp2
{
    public class Program
    {
        public void Main(string[] args)
        {
            Console.WriteLine("Hello World");

            
            var tick = Observable.Interval(TimeSpan.FromSeconds(3)).Timestamp();

            var observer = new Screw();
            tick.Subscribe(observer);

            var input = "";
            while (input != "x")
            {
                input = Console.ReadLine();

                if (input == "b")
                {
                    tick.Subscribe(observer);
                }
            }
        }

        private void Foo()
        {
            Console.WriteLine("JEE");
        }
    }
}
