using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace ObserverPattern
{

    class Printer : IDisposable
    {
        private string message;

        public void Print(string message)
        {
            this.message = message;

            Log(message);

            // throw new Exception();
        }

        private static void Log(string message)
        {
            File.AppendAllText("temp.txt", message);
        }

        private void Release()
        {
            File.Delete("temp.txt");
        }

        public void Dispose()
        {
            Release();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // DisposableTest();

            Console.WriteLine("Hello Observer Pattern!");

            // Covid19Test();

            CpuTest();

            //  WheaterForecastTest();
        }

        private static void DisposableTest()
        {
            using (Printer printer = new Printer())
            {
                printer.Print("Hello World!");
            } // <-- printer.Dispose();
        }

        private static void WheaterForecastTest()
        {
            WheaterForecast wheaterForecast = new WheaterForecast();

            IWheaterForecastObserver observer1 = new CurrentWheaterForecastObserver();
            IWheaterForecastObserver observer2 = new DatabaseWheaterForecastObserver();


            wheaterForecast.Attach(observer1);
            wheaterForecast.Attach(observer2);
            wheaterForecast.Attach(new AlertWheaterForecastObserver(20));

            int counter = 0;

            while (true)
            {
                wheaterForecast.GetChanges();

                Thread.Sleep(TimeSpan.FromSeconds(1));

                counter++;

                if (counter > 3)
                {
                    wheaterForecast.Detach(observer1);
                }
            }
        }

        #region COVID
        private static void Covid19Test()
        {
            IObservationService observationService = new FakeObservationService();

            var observations = observationService.Get();

            foreach (Observation observation in observations)
            {
                Console.WriteLine(observation);

                if (observation.Country == "Poland" && observation.Confirmed > 30)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Poland ALERT");
                    Console.ResetColor();
                }

                if (observation.Country == "Germany" && observation.Confirmed > 10)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Germany ALERT");
                    Console.ResetColor();
                }
            }
        }



        public class Observation
        {
            public string Country { get; set; }
            public int Confirmed { get; set; }
            public int Recovered { get; set; }
            public int Deaths { get; set; }

            public override string ToString()
            {
                return $"{Country} {Confirmed}/{Recovered}/{Deaths}";
            }

        }

        public interface IObservationService
        {
            IEnumerable<Observation> Get();
        }

        public class FakeObservationService : IObservationService
        {
            public IEnumerable<Observation> Get()
            {
                yield return new Observation { Country = "China", Confirmed = 2 };
                yield return new Observation { Country = "Germany", Confirmed = 1 };
                yield return new Observation { Country = "China", Confirmed = 20 };
                yield return new Observation { Country = "Germany", Confirmed = 60, Recovered = 4, Deaths = 2 };
                yield return new Observation { Country = "Poland", Confirmed = 10, Recovered = 5 };
                yield return new Observation { Country = "China", Confirmed = 30 };
                yield return new Observation { Country = "Poland", Confirmed = 50, Recovered = 15 };
                yield return new Observation { Country = "US", Confirmed = 10, Recovered = 5, Deaths = 1 };
                yield return new Observation { Country = "US", Confirmed = 11, Recovered = 3, Deaths = 4 };
                yield return new Observation { Country = "Poland", Confirmed = 45, Recovered = 25 };
                yield return new Observation { Country = "Germany", Confirmed = 52, Recovered = 4, Deaths = 1 };
            }
        }

        #endregion

        #region CPU
        private static void CpuTest()
        {
            // dotnet add package System.Reactive

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");


            IObservable<float> source = Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(_ => cpuCounter.NextValue());
         
            source
                .Buffer(TimeSpan.FromSeconds(10))
                .Select(m=>m.Average())
                .Where(cpu => cpu < 30)
                .Subscribe(cpu =>
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine($"CPU {cpu} %");
                    Console.ResetColor();
                });

            source
                .Where(cpu => cpu > 50)
                .Subscribe(cpu =>
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"CPU {cpu} %");
                    Console.ResetColor();
                });

            source
                .Subscribe(cpu => 
                {
                    Console.WriteLine($"CPU {cpu} %");
                });

            Console.WriteLine("Press any key to exit.");

            Console.ReadKey();

        }

        #endregion

        #region WheaterForecast

        // Abstract Subject
        public interface ISubject
        {
            void GetChanges();
        }

        // Concrete Subject
        public class WheaterForecast : ISubject
        {
            private Random random = new Random();

            public int Temperature { get; set; }
            public int Preasure { get; set; }
            public double Humidity { get; set; }

            private ICollection<IWheaterForecastObserver> observers = new List<IWheaterForecastObserver>();

            public void Attach(IWheaterForecastObserver observer)
            {
                this.observers.Add(observer);
            }

            public void Detach(IWheaterForecastObserver observer)
            {
                this.observers.Remove(observer);
            }

            private int GetTemperature()
            {
                return random.Next(-20, 40);
            }

            private int GetPreasure()
            {
                return random.Next(900, 1200);
            }


            private double GetHumidity()
            {
                return random.NextDouble();
            }

            public void GetChanges()
            {
                this.Temperature = GetTemperature();
                this.Preasure = GetPreasure();
                this.Humidity = GetHumidity();

                foreach (var observer in observers)
                {
                    observer.Update(this);
                }
            }

            private void SaveCurrent(int temperature, int preasure, double humidity)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                // throw new Exception();
                System.Console.WriteLine($"Save to database: {temperature}C {preasure}hPa {humidity:P2}");
            }

            private void DisplayCurrrent(int temperature, int preasure, double humidity)
            {
                System.Console.WriteLine($"Current Wheather: {temperature}C {preasure}hPa {humidity:P2}");
            }

            private void DisplayForecast(int temperature, int preasure, double humidity)
            {
                System.Console.WriteLine($"Forecast Wheather: {temperature}C {preasure}hPa {humidity:P2}");
            }

            private void DisplayStatistics(int temperature, int preasure, double humidity)
            {
                System.Console.WriteLine($"Statistics Wheather: {temperature}C {preasure}hPa {humidity:P2}");
            }


        }

        #endregion

        // Abstract Observer
        public interface IWheaterForecastObserver
        {
            void Update(WheaterForecast subject);
        }

        // Concrete Observer
        public class CurrentWheaterForecastObserver : IWheaterForecastObserver
        {
            public void Update(WheaterForecast subject)
            {
                Console.WriteLine($"Current Wheather: {subject.Temperature}C {subject.Preasure}hPa {subject.Humidity:P2}");
            }
        }

        // Concrete Observer
        public class AlertWheaterForecastObserver : IWheaterForecastObserver
        {
            private readonly int threshold;

            public AlertWheaterForecastObserver(int threshold)
            {
                this.threshold = threshold;
            }

            public void Update(WheaterForecast subject)
            {
                if (subject.Temperature > threshold)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ALERT Wheather: {subject.Temperature}C {subject.Preasure}hPa {subject.Humidity:P2}");
                    Console.ResetColor();
                }
            }
        }

        // Concrete Observer
        public class DatabaseWheaterForecastObserver : IWheaterForecastObserver
        {
            public void Update(WheaterForecast subject)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                // throw new Exception();
                Console.WriteLine($"Save to database: {subject.Temperature}C {subject.Preasure}hPa {subject.Humidity:P2}");
            }
        }

        /*

        // Abstract Observer
        public interface IObserver<in T>
        {
            // Poinformowanie obserwatorów o zakończeniu nadawania
            void OnCompleted();

            // Poinformowanie obserwatorów o wystąpieniu błędu
            void OnError(Exception error);

            // Update/Notify
            void OnNext(T value);
        }

        // Abstract Subject
        public interface IObservable<out T>
        {
            // Attach
            IDisposable Subscribe(IObserver<T> observer);

            // Detach -> IDisposable.Dispose()
            // ???
        }

        */

    }

}
