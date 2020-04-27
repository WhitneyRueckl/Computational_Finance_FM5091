﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Monte_Carlo_Option_Pricer
{
    class European: Option
    {



        Func<double, double, double, double, double, double> black_scholes_d1 = (S, K, r, vol, T) => (Math.Log(S / K)) + (r + 0.5 * Math.Pow(vol, 2) * T / (vol * Math.Sqrt(T)));

        Other calc = new Other();


        public double calcPayoff(double S, double K, bool isput)
        {
            // may want to add n_steps back in if want to see payoff at differente steps


            double payoff = 0;
            bool put_call_switch = isput;

            // call = 0, put = 1
            if (put_call_switch == false)
            {
                payoff = Math.Max(0, S - K);
            }

            else if (put_call_switch == true)
            {
                payoff = Math.Max(0, K - S);
            }

            return payoff;


        }





        // double[,] pricePaths,
        public double[] calcOptionPrices(Underlying under,  double K, double T, int steps, int trials, bool isput)
        {

            // double[,] assetSimPrices = new double[trials + 1, steps + 1];
            //NormalDist norm = new NormalDist();


            double[] optionPrices = new double[trials + 1];
            double[] negCorr_optionPrices = new double[trials + 1];

            double[,] assetSimPrices = getSimPaths(under, T, steps, trials, Simulator.randMatrix);

         

            //double[,] assetSimPrices = sims;

            double r = under.Rate;

            double discfactor = Math.Pow(Math.E, -r * T);


            for (int i = 0; i <= trials; i++)
            {
               
                double asset_px = assetSimPrices[i, steps];
                double payoff = calcPayoff(asset_px, K, isput);


                optionPrices[i] = payoff * discfactor;

            }



            double optionPx = calc.calcAverage(optionPrices, trials);
            


            return (optionPrices);
            //return (optionPrices, optionPx);

        }

        // Overloaded version of calcOptionPrices() --------
        
        public double[] calcOptionPrices(Underlying under, double[,] pricePaths, double K, double T, int steps, int trials, bool isput)
        {

            //Console.WriteLine(" New price calc, paths:  " + pricePaths[1, 1]);

            // double[,] assetSimPrices = getSimPaths(under, T, steps, trials);

            double[,] assetSimPrices = pricePaths;

            double[] optionPrices = new double[trials + 1];

            //double[,] assetSimPrices = sims;

            double r = under.Rate;

            double discfactor = Math.Pow(Math.E, -r * T);

            for (int i = 0; i <= trials; i++)
            {

                double asset_px = assetSimPrices[i, steps];
                double payoff = calcPayoff(asset_px, K, isput);


                optionPrices[i] = payoff * discfactor;

            }


            return optionPrices;

        }

    


        



        //Func<double, double, double, double, double, double> black_scholes_d1 = (S, K, r, vol, T) => (Math.Log(S / K)) + (r + 0.5 * Math.Pow(vol, 2) * T / (vol * Math.Sqrt(T)));


        

        

    }





}
