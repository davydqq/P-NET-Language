using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reactive
{
    public static class ObservableP
    {
        public static void Example1()
        {
            IObservable<int> source = Observable.Range(1, 10);
            IDisposable subscription = source.Subscribe(
                x => Console.WriteLine("OnNext: {0}", x),
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();
            subscription.Dispose();
        }

        public static void Example2()
        {
            IObservable<int> source = Observable.Range(1, 10);
            IObserver<int> obsvr = Observer.Create<int>(
                x => Console.WriteLine("OnNext: {0}", x),
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted"));
            IDisposable subscription = source.Subscribe(obsvr);
            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();
            subscription.Dispose();
        }

        public static void Example3()
        {
            Console.WriteLine("Current Time: " + DateTime.Now);

            var source = Observable.Timer(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1))
                                   .Timestamp();

            using (source.Subscribe(x => Console.WriteLine("{0}: {1}", x.Value, x.Timestamp), () => Console.WriteLine("Completed")))
            {
                Console.WriteLine("Press any key to unsubscribe");
                Console.ReadKey();
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        public static void Example4()
        {
            IEnumerable<int> e = new List<int> { 1, 2, 3, 4, 5 };

            IObservable<int> source = e.ToObservable();
            IDisposable subscription = source.Subscribe(
                                        x => Console.WriteLine("OnNext: {0}", x),
                                        ex => Console.WriteLine("OnError: {0}", ex.Message),
                                        () => Console.WriteLine("OnCompleted"));
            Console.ReadKey();
        }

        public static void Example5()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1));

            IDisposable subscription1 = source.Subscribe(
                            x => Console.WriteLine("Observer 1: OnNext: {0}", x),
                            ex => Console.WriteLine("Observer 1: OnError: {0}", ex.Message),
                            () => Console.WriteLine("Observer 1: OnCompleted"));

            IDisposable subscription2 = source.Subscribe(
                            x => Console.WriteLine("Observer 2: OnNext: {0}", x),
                            ex => Console.WriteLine("Observer 2: OnError: {0}", ex.Message),
                            () => Console.WriteLine("Observer 2: OnCompleted"));

            Console.WriteLine("Press any key to unsubscribe");
            Console.ReadLine();
            subscription1.Dispose();
            subscription2.Dispose();
        }

        public static void Example6()
        {
            Console.WriteLine("Current Time: " + DateTime.Now);
            var source = Observable.Interval(TimeSpan.FromSeconds(1));            //creates a sequence

            IConnectableObservable<long> hot = Observable.Publish<long>(source);  // convert the sequence into a hot sequence

            IDisposable subscription1 = hot.Subscribe(                        // no value is pushed to 1st subscription at this point
                                        x => Console.WriteLine("Observer 1: OnNext: {0}", x),
                                        ex => Console.WriteLine("Observer 1: OnError: {0}", ex.Message),
                                        () => Console.WriteLine("Observer 1: OnCompleted"));

            Console.WriteLine("Current Time after 1st subscription: " + DateTime.Now);
            Thread.Sleep(3000);  //idle for 3 seconds

            hot.Connect();       // hot is connected to source and starts pushing value to subscribers 

            Console.WriteLine("Current Time after Connect: " + DateTime.Now);
            Thread.Sleep(5000);  //idle for 3 seconds

            Console.WriteLine("Current Time just before 2nd subscription: " + DateTime.Now);

            IDisposable subscription2 = hot.Subscribe(     // value will immediately be pushed to 2nd subscription
                                        x => Console.WriteLine("Observer 2: OnNext: {0}", x),
                                        ex => Console.WriteLine("Observer 2: OnError: {0}", ex.Message),
                                        () => Console.WriteLine("Observer 2: OnCompleted"));
            Console.ReadKey();
        }

        public static void Example7()
        {
            Stream inputStream = Console.OpenStandardInput();
            var read = Observable.FromAsyncPattern<byte[], int, int, int>(inputStream.BeginRead, inputStream.EndRead);
            byte[] someBytes = new byte[10];
            IObservable<int> source = read(someBytes, 0, 10);
            IDisposable subscription = source.Subscribe(
                                        x => Console.WriteLine("OnNext: {0}", x),
                                        ex => Console.WriteLine("OnError: {0}", ex.Message),
                                        () => Console.WriteLine("OnCompleted"));
            Console.ReadKey();
        }

        public static void Example8()
        {
            var source1 = Observable.Range(1, 5);
            var source2 = Observable.Range(1, 3);
            source1.Concat(source2)
                   .Subscribe(Console.WriteLine);
            Console.ReadLine();
        }

        public static void Example9()
        {
            var source1 = Observable.Range(1, 5);
            var source2 = Observable.Range(1, 3);
            source1.Merge(source2)
                   .Subscribe(Console.WriteLine);
            Console.ReadLine();
        }

        public static void Example10()
        {
            var source1 = Observable.Range(1, 5);
            var source2 = Observable.Range(1, 3);
            source1.Catch(source2)
                   .Subscribe(Console.WriteLine);
            Console.ReadLine();
        }

        public static void Example11()
        {
            var source1 = Observable.Throw<int>(new Exception("An error has occurred."));
            var source2 = Observable.Range(4, 3);
            source1.OnErrorResumeNext(source2)
                   .Subscribe(Console.WriteLine);
            Console.ReadLine();
        }

        public static void Example12()
        {
            var seqNum = Observable.Range(1, 5);
            var seqString = from n in seqNum
                            select new string('*', (int)n);
            seqString.Subscribe(str => { Console.WriteLine(str); });
            Console.ReadKey();
        }

        public static void Example13()
        {
            var source1 = Observable.Interval(TimeSpan.FromSeconds(5)).Take(2);
            var proj = Observable.Range(100, 3);
            var resultSeq = source1.SelectMany(proj);

            var sub = resultSeq.Subscribe(x => Console.WriteLine("OnNext : {0}", x.ToString()),
                                          ex => Console.WriteLine("Error : {0}", ex.ToString()),
                                          () => Console.WriteLine("Completed"));
            Console.ReadKey();
        }

        public static void Example14()
        {
            IObservable<int> seq = Observable.Generate(0, i => i < 10, i => i + 1, i => i * i);
            IObservable<int> source = from n in seq
                                      where n < 5
                                      select n;
            source.Subscribe(x => { Console.WriteLine(x); });   // output is 0, 1, 4, 9
            Console.ReadKey();
        }

        public static void Example15()
        {
            var seq = Observable.Interval(TimeSpan.FromSeconds(1));
            var bufSeq = seq.Buffer(5);
            bufSeq.Subscribe(values => Console.WriteLine(values.Sum()));
            Console.ReadKey();
        }

        public static void Example16()
        {
            var seq = Observable.Interval(TimeSpan.FromSeconds(1));
            var bufSeq = seq.Buffer(TimeSpan.FromSeconds(3));
            bufSeq.Subscribe(value => Console.WriteLine(value.Sum()));
            Console.ReadKey();
        }
    }
}
