using System;
using System.Threading.Tasks;
using OOPBasic;
using AsyncBasic;

namespace AsyncBasic
{
	static class Program
	{
		static async Task Main(string[] args)
		{
			//Console.WriteLine("Runnig object-oriented code example...\n");
			//BottleDemo.Run();

			//Console.WriteLine("Running async code time test...\n");
			//await AsyncDemo.RunTest();

			Console.WriteLine("Running async code example...\n");
			await AsyncDemo.Run();
		}
	}
}
