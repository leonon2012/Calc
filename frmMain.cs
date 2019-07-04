using System;
using System.Windows.Forms;

namespace Calc
{
    public partial class frmMain : Form
    {
        Calculator calculator;

        public frmMain()
        {
            InitializeComponent();
            calculator = new Calculator();
            RefreshLabel();
        }

        void RefreshLabel()
        {
            this.lblCalc.Text = calculator.ToString();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            calculator.AddKey((Calculator.KeyType)(sender as Button).Tag);
            RefreshLabel();
        }
    }
}
