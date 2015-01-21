using System;
using System.Collections.Generic;
using System.Linq;


public static class Utils 
{
	public static class Randomizer
	{
		static System.Random _random = new Random();

      	
        public static int GetRandom(int from, int to, float[] probability)
        {
            float rnd01 = UnityEngine.Random.Range(0.0f, 1.0f);
            
            int intervalsCount = to - from + 1;
            

            float sum = 0;
            int i = 0;
            for (; i < intervalsCount; ++i)
            {
                sum += probability[i];
                
                if (rnd01 < sum)
                    break;   
            }
            
            return from + i;
        }

        public static int GetRandom(int from, int to)
        {
            float[] probability = new float[to - from + 1];
            float len = 1.0f / probability.Length;
            Array.ForEach<float>(probability, x => x = len);

            return GetRandom(from, to, probability);
        }


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
