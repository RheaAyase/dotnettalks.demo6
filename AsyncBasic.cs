using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AsyncBasic
{
    class AsyncDemo
    {
        const int PrimeToFind = 1000000;
        const int ExamplePrime = 7654321;
        const int MaxPrimeToPrint = 32452843;
        const int ThreadsToUse = 4;

        public static async Task Run()
        {
            Task printTask = PrimeNumbers.PrintPrimeNumbersConcurrent(MaxPrimeToPrint, ThreadsToUse);

            Console.WriteLine($"Looking for the first prime number higher than {ExamplePrime} *asynchronously* (1 main thread) " +
                              $"while also printing all the prime numbers up to {MaxPrimeToPrint} " +
                              $"*concurrently* ({ThreadsToUse} different threads) ...");

            int foundPrime = await PrimeNumbers.PrintPrimeNumbersAndGetNextAsync(ExamplePrime, false);

            Console.WriteLine($"Found the prime number: {foundPrime}");

            await printTask;

            Console.WriteLine($"All done!");
        }

        public static async Task RunTest()
        {
            Console.WriteLine($"Looking for {PrimeToFind} prime numbers *synchronously* (1 main thread) ...");
            Stopwatch watch = Stopwatch.StartNew();
            int prime = PrimeNumbers.FindNthPrime(PrimeToFind);
            watch.Stop();
            Console.WriteLine($"Synchronously found the {PrimeToFind}th prime number {prime} in {watch.ElapsedMilliseconds}ms");

            Console.WriteLine($"Looking for {PrimeToFind} prime numbers *concurrently* ({ThreadsToUse} threads) ...");
            watch.Restart();
            await PrimeNumbers.PrintPrimeNumbersConcurrent(prime, ThreadsToUse);
            watch.Stop();
            Console.WriteLine($"Concurrently found all the prime numbers up to {prime} in {watch.ElapsedMilliseconds}ms");
        }
    }


    class PrimeNumbers
    {
        /// <summary> Print prime numbers up to specified max, using specified number of threads. </summary>
        public static async Task PrintPrimeNumbersConcurrent(int max, int threads)
        {
            Console.WriteLine("=== PrintPrimeNumbersConcurrent started! ===");
            List<Task> tasks = new List<Task>();
            for( int i = 0; i < threads; i++ )
            {
                if( i > 0 && threads % (i + 1) == 0 )    // Do not execute a thread if it's starting position and increment...
                    continue;                            // ...would never find a single prime number at all.
                tasks.Add(Task.Run(() => PrintPrimeNumbersAndGetNextAsync(max, true, threads, i)));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("=== PrintPrimeNumbersConcurrent done! ===");
        }

        /// <summary> Print prime numbers up to specified max, and return the one above. </summary>
        public static async Task<int> PrintPrimeNumbersAndGetNextAsync(int max, bool print = true, int increment = 1, int offset = 0)
        {
            Console.WriteLine("=== PrintPrimeNumbersAndGetNextAsync started! ===");
            int number = 1 + offset;

            while( true )
            {
                number += increment;

                int denominator = 1;    // Denominator == Jmenovatel (in czech, because I had to translate it!)
                bool isPrime = true;
                while( ++denominator * denominator <= number )
                {
                    if( number % denominator == 0 )
                    {
                        isPrime = false;
                        break;
                    }
                }

                if( isPrime )
                {
                    if( print )
                    {
                        //Console.WriteLine(number.ToString());    // Don't actually print the spam! :D
                    }
                    else
                    {
                        await Task.Yield();                        // Experiment ;)
                    }

                    if( number > max )
                    {
                        Console.WriteLine("=== PrintPrimeNumbersAndGetNextAsync done! ===");
                        return number;
                    }
                }
            }
        }

        /// <summary> Find n-th prime number. </summary>
        public static int FindNthPrime(int n)
        {
            int count = 0;
            int a = 1;
            while( true )
            {
                a++;
                int b = 1;
                bool isPrime = true;
                while( ++b * b <= a )
                {
                    if( a % b == 0 )
                    {
                        isPrime = false;
                        break;
                    }
                }
                if( isPrime )
                {
                    if( ++count == n )
                        break;
                }
            }

            return a;
        }
    }
}
