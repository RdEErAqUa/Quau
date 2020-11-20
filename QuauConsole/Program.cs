using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuauConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<double> sz = new List<double> { -1, 0, 3, 3, 7, 9, 10, 10, 10, 15, 18, 20, 25 };
            var temp = sz.GroupBy(x => x).OrderBy(g => g.Count()).Select(g => g.Key);

            List<double> Rank = new List<double> { };

            var newSample = sz;

            newSample.Sort();
            List<double> tempValue = new List<double> { };
            for (int i = 0; i < newSample.Count; i++)
            {
                if (!tempValue.Contains(newSample[i]))
                {
                    tempValue.Add(newSample[i]);
                    double countSize = 0;
                    double z = 0;
                    int k = 0;
                    for (k = i; k < newSample.Count; k++)
                    {
                        if (newSample[k] == newSample[i])
                        {
                            countSize += 1.0;
                            z += ((double)k + 1.0);
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int f = i; f < k; f++)
                    {
                        Rank.Add((z / countSize));
                    }
                }
            }

            foreach (var el in Rank)
                Console.WriteLine(el);
        }
    }
}
