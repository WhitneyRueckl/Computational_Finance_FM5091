﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monte_Carlo_Pricer
{
    public class SDE
    {
        public double GBM(double delta_t, double drift, double vol, double randnum)
        {

            double nextPriceMultiplier = Math.Exp((drift - Math.Pow(vol, 2) / 2) * delta_t + vol * Math.Sqrt(delta_t) * randnum);

            return nextPriceMultiplier;

        }

        // double randnum)

        // method to calc standard error
        public double calcStandardError(double[] prices, double[] antiprices, int trials)
        {

            double std_err_result;
            double sum_prices = 0;
            double sum_dev = 0;
            double avg_prices = 0;
            double avg_dev = 0;

            double sum_antiprices = 0;


            if (InputOutput.Var_Reduc == true)
            {
                // sum of simulated prices
                for (long i = 0; i < trials; i++)
                {
                    sum_prices += prices[i];
                    sum_antiprices += antiprices[i];

                }

                avg_prices = (sum_prices + sum_antiprices) / (trials);
              

                // calc std deviation
                for (long i = 0; i < trials; i++)
                {
                    sum_dev += Math.Pow(((prices[i] + antiprices[i]) - avg_prices), 2);

                }

                // calc average deviation
                avg_dev = Math.Sqrt(sum_dev / (trials - 1));

            }
            else if (InputOutput.Var_Reduc == false)
            {

                // sum of simulated prices
                for (long i = 0; i < trials; i++)
                {
                    sum_prices += prices[i];

                }

                avg_prices = sum_prices / trials;

                // calc std deviation
                for (long i = 0; i < trials; i++)
                {
                    sum_dev += Math.Pow((prices[i] - avg_prices), 2);

                }

                // calc average deviation
                avg_dev = Math.Sqrt(sum_dev / (trials - 1));

            }
            


            // std error
            std_err_result = avg_dev / (2 * Math.Sqrt(trials));

            return std_err_result;


        }


    }
}
