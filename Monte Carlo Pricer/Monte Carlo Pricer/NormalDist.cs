﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Pricer
{
    class NormalDist
    {

        public double NormDistCDF(double x)
        {
            // constants
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            // Save the sign of d1
            int sign = 1;
            if (x < 0)
                sign = -1;

            x = Math.Abs(x) / Math.Sqrt(2.0);

            // 
            double t = 1.0 / (1.0 + p * x);
            double erf = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-x * x);

            double output = 0.5 * (1.0 + sign * erf);
            //double a = 5.2;

            return output;


        }




    }
}
