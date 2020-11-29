using Quau.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Services.StatisticOperation
{
    static class StatisticSampleRanking
    {
        static public void SampleRanking(StatisticSample valueSample)
        {
            //
            ICollection<SampleRanking> SampleRankingData = new List<SampleRanking> { };

            List<double> dataSample = removeEqualse(valueSample.Sample);

            dataSample.Sort();
            //

            List<int> dataSampleFrequency = findDataFrequency(dataSample, valueSample.Sample);
            //

            List<double> DataRelativeFrequency = findDataRelativeFrequency(dataSampleFrequency);

            for(int i = 0; i < dataSample.Count; i++) SampleRankingData.Add(
                new Models.SampleRanking { SampleData = Math.Round(dataSample[i], valueSample.RoundValue), 
                SampleDataFrequency = dataSampleFrequency[i] , 
                SampleDataRelativeFrequency = Math.Round(DataRelativeFrequency[i], valueSample.RoundValue)});

            valueSample.SampleDataRanking = new ObservableCollection<SampleRanking>(SampleRankingData);
        }

        static private List<double> removeEqualse(ICollection<double> value)
        {

            List<double> returnValue = new List<double> { };

            foreach(var el in value)
            {
                if (!returnValue.Contains(el)) returnValue.Add(el);
            }
            return returnValue;
        }

        static private List<int> findDataFrequency(ICollection<double> value, ICollection<double> valueIN)
        {
            List<int> dataFrequency = new List<int> { };
            foreach(var el in value)
            {
                int count = 0;
                foreach(var el2 in valueIN)
                {
                    if (el == el2) count++; 
                }
                dataFrequency.Add(count);
            }

            return dataFrequency;
        }

        static private List<double> findDataRelativeFrequency(ICollection<int> dataSampleFrequency)
        {
            List<double> DataRelativeFrequency = new List<double> { };
            double size = 0;

            foreach (var el in dataSampleFrequency) size += el;

            foreach (var el in dataSampleFrequency) DataRelativeFrequency.Add(el / size);

            return DataRelativeFrequency;
        }
    }
}
