﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Monte_Carlo_Rebuild
{
    public class PriceCalculator
    {

        public bool Var_Reduc { get; set; }
        public double[] optionPrices{get; set;}
        public double[] neg_optionPrices { get; set; }

   

        Other calc = new Other();

        public PriceCalculator()
        {

            Var_Reduc = IO.Var_Reduc;

            optionPrices = new double[IO.Trials+ 1];
            neg_optionPrices = new double[IO.Trials + 1];

        }

        public (double option_price, double[] optionPrices) computePrices()
        {

            Underlying under = new Underlying();
            Option option = new Option();


            int steps = IO.N_Steps;
            int trials = IO.Trials;
            double option_price = 0;

            if(IO.Var_Reduc == true)
            {

                optionPrices = calcOptionPrices(under, Simulator.simPaths, option, steps, trials);
                neg_optionPrices = calcOptionPrices(under, Simulator.neg_simPaths, option, steps, trials);

                double avg_prices = calc.calcAverage(optionPrices, IO.Trials);
                double avg_neg_prices = calc.calcAverage(neg_optionPrices, IO.Trials);

                option_price = 0.5 * (avg_prices + avg_neg_prices);

                // left off here ---- creating a void to do the logic for if the var reduction is needed

            }
            else if(IO.Var_Reduc == false)
            {
                optionPrices = calcOptionPrices(under, Simulator.simPaths, option, steps, trials);

                double avg_prices = calc.calcAverage(optionPrices, IO.Trials);

                option_price  = avg_prices;

            }

            return (option_price, optionPrices);

        }








        // Logic for CV variance reduction inside this method below -- need to test if works
        public double[] calcOptionPrices(Underlying under, double[,] pricePaths, Option option, int steps, int trials)
        {

            double[,] assetSimPrices = pricePaths;

            double[] optionPrices = new double[trials + 1];

            double[] negCorr_optionPrices = new double[trials + 1];


            double r = under.Rate;
            double T = option.Tenor;
            double K = option.Strike;
            string optype = option.OptType;
            bool isput = option.IsPut;


            double payoff = 0;
            double cv_adj = 0;
            double discfactor = Math.Pow(Math.E, -r * T);

            // Variables for if control variate required
            if (IO.CV_Var_Reduc == true)
            {
                ControlVariate cvar = new ControlVariate();

                double beta = cvar.calcControlVariate(under, option, pricePaths, trials, steps).beta;
                double cv = cvar.calcControlVariate(under, option, pricePaths, trials, steps).cv;

                cv_adj = beta * cv;

                //Console.WriteLine(" CV beta =   " + beta);
                //Console.WriteLine(" CV beta =   " + cv);


            }
            else if (IO.CV_Var_Reduc == false)
            {
                cv_adj = 0;

            }


            for (int i = 0; i <= trials; i++)
            {

                double asset_px = assetSimPrices[i, steps];

                //Console.WriteLine("Trial #:  " + i);
               // Console.WriteLine(" !!! ---- assetSimPaths[i, j]  =  " + assetSimPrices[i, steps] + "  asset_px =  " + asset_px);

                if (optype == "European")
                {
                    European euro = new European();
                    payoff = euro.calcPayoff(asset_px, K, isput) + cv_adj;


                }
                else if (optype == "American")
                {

                    // American option class and payoff function don't exist in this program yet
                    //American amer = new American();
                    // payoff = amer.calcPayoff(asset_px, K, isput) + cv_adj;
                    Console.WriteLine("Pricing of an American style option not yet supported");
                    


                }
                else if(optype == "Asian")
                {

                    Asian asn = new Asian();
                    payoff = asn.calcPayoff(assetSimPrices, K, isput, i) + cv_adj;


                }
                else if(optype == "Digital")
                {

                    Digital dig = new Digital();
                    payoff = dig.calcPayoff(under, K, T, isput) + cv_adj; ;


                }
                else if (optype == "Lookback")
                {

                    Lookback look = new Lookback();
                    payoff = look.calcPayoff(assetSimPrices, K, isput, i) + cv_adj; ;


                }
                else if (optype == "Range")
                {

                    Range rng = new Range();
                    payoff = rng.calcPayoff(assetSimPrices, K, isput, i) + cv_adj; ;


                }
                else if (optype == "Barrier")
                {

                    Barrier bar = new Barrier();
                    payoff = bar.calcPayoff(assetSimPrices, K, isput, i) + cv_adj; ;


                }

                //Console.WriteLine(" Payoff =  " + payoff);

                optionPrices[i] = payoff * discfactor;

            }


            return optionPrices;


        }



    }
}
