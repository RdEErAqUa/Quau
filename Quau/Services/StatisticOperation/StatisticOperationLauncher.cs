using Quau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation
{
    static class StatisticOperationLauncher
    {
        static public void StartStatisticOperation(StatisticSample valueSample)
        {
            StatisticSampleRanking.SampleRanking(valueSample);

            StatisticDivisionInClass.DivisionInClass(valueSample);
        }
    }
}
