using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services
{
    static class DataConvertor
    {
        static public List<double> DataConvertorStrToDouble(string dataValue)
        {
            try
            {
                return (dataValue.Split(' ').
                        Where(x => !string.IsNullOrWhiteSpace(x)).
                        Select(x => double.Parse(x)).ToArray().ToList());
            }
            catch (System.FormatException)
            {
                return null;
            }
        }
    }
}
