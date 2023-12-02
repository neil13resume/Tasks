using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {

        ConversionServiceRef.ConvServiceClient _ConvService;
        public Form1()
        {
            InitializeComponent();

            //Specifying the binding TCP from app.config file 
            _ConvService = new ConversionServiceRef.ConvServiceClient("NetTcpBinding_IConvService");

            ttDollar.SetToolTip(txtInput,"0-1000000000");

        }

        private void BtnCalc_Click(object sender, EventArgs e)
        {            
           // txtResult.Text = _ConvService.GetData(txtInput.Text);//working

            string enteredValue = txtInput.Text;
            //check and remove space
            string strwithoutWhitespace = RemoveWhitespace(enteredValue);
            //check whether dollar value is zero atleast, else insert 0 - Taken care in Api


            txtResult.Text = _ConvService.GetData(strwithoutWhitespace);
        }

        private string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
           .Where(c => !Char.IsWhiteSpace(c))
           .ToArray());
        }

        private long Maxvalue = 1000000000;
        private void txtInput_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput.Text))
            {
                if (txtInput.Text.Contains(','))
                {
                    string[] str = txtInput.Text.Split(',');
                    if (!string.IsNullOrEmpty(str[0]))
                        CompareWithMax(long.Parse(str[0]));//since it will not be less than 0, which we are adding as default
                }
                else
                {
                    CompareWithMax(long.Parse(txtInput.Text));
                }
            }

        }

        private void CompareWithMax(long part1)
        {
            if (part1 > Maxvalue)
            {
                MessageBox.Show("Value is greater than 1000000000.");
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }

            if (!char.IsControl(e.KeyChar))
            {

                TextBox textBox = (TextBox)sender;

                if (textBox.Text.IndexOf(',') > -1 &&
                         textBox.Text.Substring(textBox.Text.IndexOf(',')).Length >= 3)
                {
                    e.Handled = true;
                }

            }
        }
    }
}
