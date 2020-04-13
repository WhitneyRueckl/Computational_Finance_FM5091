﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Monte_Carlo_Pricer
{
    public partial class Form1 : Form
    {

        
        int progress = 0;

        public delegate void IncrementProgress();
        public IncrementProgress myDelegate;

        Thread thd1;
        


        public Form1()
        {
            InitializeComponent();

            myDelegate = new IncrementProgress(IncrementProgressMethod);


            // set default input values (this should price the option ~ $10.89)
            textBox_Spot.Text = "50";
            textBox_Strike.Text = "50";
            textBox_Rate.Text = "0.05";
            textBox_Tenor.Text = "1";
            textBox_Volatility.Text = "0.50";
            textBox_Drift.Text = "0.05";
            textBox_Steps.Text = "2";
            textBox_Trials.Text = "10000";
            textBox_Cores.Text = "8";

            radioButton_Call.Checked = true;
            checkBox_VarReduc.Checked = true;
            checkBox_CV_VarReduc.Checked = true;
            checkBox_Multithread.Checked = true;
         


        }

        
        public void IncrementProgressMethod()
        {

            progress++;
            label_Progress.Text = progress.ToString();
            progressBar1.Value = progress;


        }
        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox_Spot_TextChanged(object sender, EventArgs e)
        {



        }

        private void button_GO_Click(object sender, EventArgs e)
        {

            // idea - all of this where you set the InputOutput fields - Convert. blahblah.... you should put those
            // assignments in the classes where the methods are that use those values
            //Pass inputs from textboxes to InputOutput class
            InputOutput.SpotPrice = Convert.ToDouble(textBox_Spot.Text);
            InputOutput.StrikePrice = Convert.ToDouble(textBox_Strike.Text);
            InputOutput.Rate = Convert.ToDouble(textBox_Rate.Text);
            InputOutput.Vol = Convert.ToDouble(textBox_Volatility.Text);
            InputOutput.Drift = Convert.ToDouble(textBox_Drift.Text);
            InputOutput.Tenor = Convert.ToDouble(textBox_Tenor.Text);

            InputOutput.N_Steps = Convert.ToInt32(textBox_Steps.Text);
            InputOutput.Trials = Convert.ToInt32(textBox_Trials.Text);

            InputOutput.Num_Cores = Convert.ToInt32(textBox_Cores.Text);
          

            // Determine if put or call 
            if (radioButton_Call.Checked)
            {

                InputOutput.PutCall = 0;

            }
            else if (radioButton_Put.Checked)
            {
                InputOutput.PutCall = 1;

            }



            // Determine if varaiance reduction is requested and assign bool to Var_Reduc
            // note - if I remove this code below it doesn't seem to matter as long as I have the private void
            // called checkBox_VarReduc_CheckedChanged (below).....???
            if (checkBox_VarReduc.Checked)
            {

                InputOutput.Var_Reduc = true;

            }
            else
            {
                InputOutput.Var_Reduc = false;

            }


            Thread C = new Thread(new ThreadStart(InputOutput.getResults));
            C.Start();
            //InputOutput.getResults();

            textBox_OptionPrice.Text = Convert.ToString(InputOutput.O_Price);
            textBox_Delta.Text = Convert.ToString(InputOutput.O_Delta);
            textBox_Gamma.Text = Convert.ToString(InputOutput.O_Gamma);
            textBox_Theta.Text = Convert.ToString(InputOutput.O_Theta);
            textBox_Vega.Text = Convert.ToString(InputOutput.O_Vega);
            textBox_Rho.Text = Convert.ToString(InputOutput.O_Rho);

            textBox_StdErr.Text = Convert.ToString(InputOutput.O_StdErr);

            textBox_Timer.Text = Convert.ToString(InputOutput.Timer);



        }
        
        private void checkBox_VarReduc_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox_VarReduc.Checked)
            {

                InputOutput.Var_Reduc = true;

            }
            else //if (checkBox_VarReduc.Checked)
            {
                InputOutput.Var_Reduc = false;

            }


        }
        

        private void checkBox_CV_VarReduc_CheckedChanged(object sender, EventArgs e)
        {


            if (checkBox_CV_VarReduc.Checked)
            {

                InputOutput.CV_Var_Reduc = true;

            }
            else //if (checkBox_CV_VarReduc.Checked)
            {
                InputOutput.CV_Var_Reduc = false;

            }


        }


        private void checkBox_Multithread_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox_Multithread.Checked)
            {
                InputOutput.Multithread = true;
                InputOutput.Num_Cores = Convert.ToInt32(textBox_Cores.Text);
            }
            else
            {
                InputOutput.Multithread = false;
                InputOutput.Num_Cores = 1;
            }

        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        
    }

}
