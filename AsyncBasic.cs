using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;

namespace AsyncBasic
{
    class AsyncDemo
    {
        const int PrimeToFind = 1000000;
        const int ExamplePrime = 7654321;
        const int MaxPrimeToPrint = 32452843;
        const int ThreadsToUse = 4;

        const string ExampleUrl = "http://fedoraloves.net";

        public static async Task Run()
        {
            // Start concurrent work on background threads (thread pool.) using `Task.Run()`
            Task printTask = PrimeNumbers.PrintPrimeNumbersConcurrent(MaxPrimeToPrint, ThreadsToUse);

            Console.WriteLine($"Started to print all the prime numbers up to {MaxPrimeToPrint} " +
                              $"*concurrently* (using up to {ThreadsToUse} different threads) ...");


            // Start asynchronous I/O task on the main thread by awaiting an `async` method.
            Console.WriteLine("=== HttpClient().GetStringAsync() started! ===");
            Task<string> webTask = new HttpClient().GetStringAsync(ExampleUrl);

            Console.WriteLine($"Started to download web content from {ExampleUrl} *asynchronously* (on the main thread) ...");


            Console.WriteLine($"While waiting for the website data, we can calculate what would be the " +
                              $"next prime number after {ExamplePrime} fully *synchronously* (blocking the main thread) ...");

            // Start an async Task and run it completely synchronously blocking the main thread - Avoid doing this!
            Task<int> primeTask = PrimeNumbers.PrintPrimeNumbersAndGetNextAsync(ExamplePrime, false);
            int foundPrime = primeTask.Result;
            //int foundPrime = primeTask.GetAwaiter().GetResult(); // This is incorrect use for us, as the GetAwaiter
            // is meant to be used only by the compiler and not by the programmer.
            // However it will achieve the same thing - get a result of the asynchronous method, synchronously,
            // and they both block the main thread. Avoid using `.Result` as well, and correctly `await` everything :]

            Console.WriteLine($"Found the prime number: {foundPrime}");


            // Wait for the I/O task to finish, and get it's result.
            string webContent = await webTask;
            Console.WriteLine("=== HttpClient().GetStringAsync() done! ===");
            Console.WriteLine($"Downloaded web content (1st line): {webContent.Split('\n')[0]}"); //writing only the first line...


            // Wait for the concurrent CPU task to finish (it does not have a result.)
            await printTask;


            Console.WriteLine($"All done!");
        }

        public static async Task RunTest()
        {
            Console.WriteLine($"Looking for {PrimeToFind} prime numbers *synchronously* (1 main thread) ...");
            Stopwatch watch = Stopwatch.StartNew();

            // Synchronous code.
            int prime = PrimeNumbers.FindNthPrime(PrimeToFind);

            watch.Stop();
            Console.WriteLine($"Synchronously found the {PrimeToFind}th prime number {prime} in {watch.ElapsedMilliseconds}ms");


            Console.WriteLine($"Looking for {PrimeToFind} prime numbers *concurrently* ({ThreadsToUse} threads) ...");
            watch.Restart();

            // Immediately awaited asynchronous code executing multiple threads concurrently.
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
            // Executing PrintPrimeNumbersAndGetNextAsync on a "threadpool" - on some pre-allocated thread, in a loop.
            // In order to check all the numbers from 1 to infinity, by using, for example 4 `threads`,
            // we have to somehow separate this list of numbers. We can do so by the number of threads,
            // simply by starting each thread with an offset of it's number, and then incrementing the tested number
            // by the total number of threads.

            await Task.WhenAll(tasks);    // Yields until all of the threads are finished.
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
