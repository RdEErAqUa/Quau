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
            char[] separator = new char[] {' ', '\n', '\r' };
            try
            {
                dataValue = dataValue.Replace('\n',' ');
                dataValue = dataValue.Replace('.', ',');
                var returnValue = dataValue.Split(' ').
                        Where(x => !string.IsNullOrWhiteSpace(x)).
                        Select(x => double.Parse(x)).ToArray().ToList();
                returnValue.Sort();

                var returnValue2 = dataValue.Split(separator);

                return returnValue;
            }
            catch (System.FormatException)
            {
                return null;
            }
        }
    }
}
