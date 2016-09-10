using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoreLinq;

namespace LetterCountingFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            List<long>[] longestChain = new List<long>[4];
            Task[] counter = new Task[4];

            Console.WriteLine("Checking all values from " + int.MinValue + " to " + int.MaxValue + "...");
            Console.WriteLine("Even using four threads, this is probs gonna take a while...");

            counter[0] = Task.Run(() => {
                longestChain[0] = FindLongestLetterCountingChain(int.MinValue, int.MinValue / 2);
            });

            counter[1] = Task.Run(() => {
                longestChain[1] = FindLongestLetterCountingChain(int.MinValue / 2 + 1, 0);
            });

            counter[2] = Task.Run(() => {
                longestChain[2] = FindLongestLetterCountingChain(1, int.MaxValue / 2);
            });

            counter[3] = Task.Run(() => {
                longestChain[3] = FindLongestLetterCountingChain(int.MaxValue / 2, int.MaxValue);
            });

            while(!counter.All(s => s.Status == TaskStatus.RanToCompletion))
            {
                Thread.Yield();
            }

            Console.WriteLine("OMG! Done!");

            List<long> singleLongestChain = longestChain.MaxBy(l => l.Count);

            Console.WriteLine("Longest chain has length of " + singleLongestChain.Count + ", and is:");
            Console.WriteLine(string.Join(" -> ", singleLongestChain));
            Console.WriteLine(string.Join(" -> ", singleLongestChain.Select(i => i.SpelledOut())));

            Console.WriteLine();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey(true);
        }

        public static List<long> LetterCountingChain(long value)
        {
            List<long> chain = new List<long>();

            while (!chain.Contains(value))
            {
                chain.Add(value);

                value = value.SpelledOut().Count(char.IsLetter);
            }

            return chain;
        }

        public static List<long> FindLongestLetterCountingChain(long min, long max)
        {
            List<long> longestChain = null;

            for (long i = min; i <= max; i++)
            {
                List<long> chain = LetterCountingChain(i);

                if (longestChain == null || longestChain.Count < chain.Count)
                    longestChain = chain;
            }

            return longestChain;
        }
    }
}
