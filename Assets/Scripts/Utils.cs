using System;
using System.Collections.Generic;
using System.Linq;


public static class Utils 
{
	public static class Randomizer
	{
		static Random _random = new Random();
		
		public static List<T> RandomizeList<T>(List<T> inputList)
		{
			List<KeyValuePair<int, T>> list = new List<KeyValuePair<int, T>>();
		
			foreach (T i in inputList)
			{
				list.Add(new KeyValuePair<int, T>(_random.Next(), i));
			}
		
			// Sort the list by the random number
			var sorted = from item in list
				orderby item.Key
					select item;
			// Allocate new string array
			List<T> result = new List<T>(inputList.Count);

			// Copy values to array
			foreach (KeyValuePair<int, T> pair in sorted)
			{
				result.Add (pair.Value);
			}
			// Return copied array
			return result;
		}
	}
}
