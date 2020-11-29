using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quau.Models
{
    class Quantiles
    {
        public Dictionary<int, double> XI2_a0_5 = new Dictionary<int, double>();
        public Dictionary<int, double> XI2_a0_2 = new Dictionary<int, double>();

        public void XI2Quantiles()
        {
            for (int i = 1; i < 90; i++)
            {
                XI2_a0_5.Add(i, 0.455 + (i - 1.0) * 0.935);
                XI2_a0_2.Add(i, 2.71 + 1.18 * (i - 1));
            }
        }

        public Dictionary<int, double> T_a0_5 = new Dictionary<int, double>();
        public Dictionary<int, double> T_a0_25 = new Dictionary<int, double>();
        public Dictionary<int, double> T_a0_1 = new Dictionary<int, double>();
        public void TQuantiles()
        {
            T_a0_5.Add(10, 0.7);
            T_a0_5.Add(30, 0.683);
            T_a0_5.Add(120, 0.674);

            T_a0_25.Add(10, 1.22);
            T_a0_25.Add(30, 1.17);
            T_a0_25.Add(120, 1.15);

            T_a0_1.Add(10, 1.81);
            T_a0_1.Add(30, 1.7);
            T_a0_1.Add(120, 1.64);

        }
        public Dictionary<int, double> F_v1_10_a0_05 = new Dictionary<int, double>();
        public Dictionary<int, double> F_v1_30_a0_05 = new Dictionary<int, double>();
        public Dictionary<int, double> F_v1_120_a0_05 = new Dictionary<int, double>();
        public void FQuantiles()
        {
            F_v1_10_a0_05.Add(10, 2.98);
            F_v1_10_a0_05.Add(30, 2.16);
            F_v1_10_a0_05.Add(120, 1.83);

            F_v1_30_a0_05.Add(10, 2.7);
            F_v1_30_a0_05.Add(30, 1.84);
            F_v1_30_a0_05.Add(120, 1.52);

            F_v1_120_a0_05.Add(10, 2.58);
            F_v1_120_a0_05.Add(30, 1.68);
            F_v1_120_a0_05.Add(120, 1.22);

        }
    }
}
