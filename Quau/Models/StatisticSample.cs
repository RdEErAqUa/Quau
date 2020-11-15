﻿using Quau.Models.DistributionConsent;
using Quau.Models.DistributionSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    internal class StatisticSample
    {
        //Одновимірна вибірка
        //M - class count
        public double ClassSize { get; set; }

        public double StepSize { get; set; }
        public ICollection<double> Sample { get; set; }

        public SamplePrimaryStatisticAnalyse SamplePrimaryAnalyse { get; set; }

        public ICollection<SampleRanking> SampleDataRanking { get; set; }

        public ICollection<SamplePrimaryDivisionINClass> SampleDivisionINClass { get; set; }

        public ICollection<unShiftedShiftedQuantitiveCharacteristics> QuantitiveCharactacteristics { get; set; }

        public ICollection<DistributionSamples> DistributionSample { get; set; }

        public ICollection<DistributionSamples> DistributionSampleEmpirical { get; set; }

        public ICollection<DistributionConsentTest> DistributionConsentTests { get; set; }
    }
}
