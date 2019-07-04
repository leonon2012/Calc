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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.NumPad0:
                    btn0.PerformClick();
                    return true;
                case Keys.NumPad1:
                    btn1.PerformClick();
                    return true;
                case Keys.NumPad2:
                    btn2.PerformClick();
                    return true;
                case Keys.NumPad3:
                    btn3.PerformClick();
                    return true;
                case Keys.NumPad4:
                    btn4.PerformClick();
                    return true;
                case Keys.NumPad5:
                    btn5.PerformClick();
                    return true;
                case Keys.NumPad6:
                    btn6.PerformClick();
                    return true;
                case Keys.NumPad7:
                    btn7.PerformClick();
                    return true;
                case Keys.NumPad8:
                    btn8.PerformClick();
                    return true;
                case Keys.NumPad9:
                    btn9.PerformClick();
                    return true;
                case Keys.Decimal:
                    btnDot.PerformClick();
                    return true;
                case Keys.Multiply:
                    btnMul.PerformClick();
                    return true;
                case Keys.Divide:
                    btnDiv.PerformClick();
                    return true;
                case Keys.Subtract:
                    btnMinus.PerformClick();
                    return true;
                case Keys.Add:
                    btnPlus.PerformClick();
                    return true;
                case Keys.Enter:
                    btnEqual.PerformClick();
                    return true;
                case Keys.Delete:
                case Keys.Back:
                    btnDel.PerformClick();
                    return true;
                case Keys.Clear:
                case Keys.Escape:
                    btnClear.PerformClick();
                    return true;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }            
        }
    }
}
