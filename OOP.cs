using System;

namespace OOPBasic
{
	class Bottle								// Class definiton for `Bottle` type objects
	{
		public float Capacity = 1; //liters		// Member variable `Capacity`
		public float Volume = 0;   //liters		// Public member variable `Volume`
		public string Material = "unknown";		// Public member variable `Material`

		public Bottle(string material)			// Constructor, sets the `Material` of the newly created `Bottle` object
		{
			this.Material = material;
		}

		public void Fill(float volume)			// Method, adds a specific `volume` of liquid to the `Volume`...
		{										// ...without overflowing over the maximum `Capacity`
			this.Volume = Math.Min(this.Volume + volume, this.Capacity);
		}

		public void PourInto(Bottle targetBottle, float volume) // Method, fills a target bottle from `this` one...
		{														// ...up to the `Volume` currently held
			volume = Math.Min(volume, this.Volume);		// Is the desired `volume` greater than the currently held?
			targetBottle.Fill(volume);					// Fill the target bottle with it.
			this.Volume -= volume;						// Remove the `volume` from `this` bottle.
		}

		public void Empty()			// Method, sets the current `Volume` to zero.
		{
			this.Volume = 0;
		}
	}



	static class BottleDemo
	{
		public static void Run()
		{
			//Create a `plastic` `Bottle` and fill it with some water set by the user.
			Console.WriteLine("Creating a new Bottle object with Material = plastic.");
			Bottle plasticBottle = new Bottle("plastic");

			Console.WriteLine($"How much water is in it? (Capacity: {plasticBottle.Capacity}l)");
			float newVolume = float.Parse(Console.ReadLine());

			plasticBottle.Fill(newVolume);
			Console.WriteLine($"The {plasticBottle.Material} bottle now has {plasticBottle.Volume} liter(s) of water.");


			//Create a `glass` `Bottle` and fill it with some water set by the user.
			Console.WriteLine("Creating a new Bottle object with Material = glass.");
			Bottle glassBottle = new Bottle("glass");

			Console.WriteLine($"How much water is in it? (Capacity: {glassBottle.Capacity}l)");
			newVolume = float.Parse(Console.ReadLine());

			glassBottle.Fill(newVolume);
			Console.WriteLine($"The {glassBottle.Material} bottle now has {glassBottle.Volume} liter(s) of water.");


			//Pour some of the water from the `plasticBottle` into the `glassBottle`.
			Console.WriteLine($"How much water do you want to pour from the plastic bottle into the glass bottle?");
			newVolume = float.Parse(Console.ReadLine());

			plasticBottle.PourInto(glassBottle, newVolume);
			Console.WriteLine($"The {plasticBottle.Material} bottle now has {plasticBottle.Volume} liter(s) of water.");
			Console.WriteLine($"The {glassBottle.Material} bottle now has {glassBottle.Volume} liter(s) of water.");


			//Empty both of the bottles.
			Console.WriteLine("Spilling the water out.");
			plasticBottle.Empty();
			glassBottle.Empty();
			Console.WriteLine($"The {plasticBottle.Material} bottle now has {plasticBottle.Volume} liter(s) of water.");
			Console.WriteLine($"The {glassBottle.Material} bottle now has {glassBottle.Volume} liter(s) of water.");
		}
	}
}
