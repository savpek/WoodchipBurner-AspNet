using System;
using System.Reactive;

namespace WCB.Web.Lib
{
    public class Screw : IObserver<string>, IObserver<Timestamped<long>>
    {
        public int TickCount { get; set; }

        public void OnNext(string value)
        {
            Console.WriteLine("Updating screw as " + value);
        }

        public void OnNext(Timestamped<long> value)
        {
            Console.WriteLine("JEEJEE: " + value.Timestamp);
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnCompleted()
        {
            Console.WriteLine("Screw exit.");
        }
    }
}