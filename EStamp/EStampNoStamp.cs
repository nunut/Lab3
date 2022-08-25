using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;

namespace VMS_SURE.EStamp
{
    public partial class EStampNoStamp : DevExpress.XtraEditors.XtraForm
    {
        public EStampNoStamp()
        {
            InitializeComponent();
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textEdit2.Text =  EStamp.EStampControl.EStampCancleFindVisitorId(textEdit1.Text);
                if (textEdit2.Text != "")
                {
                    textEdit1.Enabled = false;
                    simpleButton1.Enabled = true;
                }
            }
        }

        private void EStampNoStamp_Load(object sender, EventArgs e)
        {
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (EStamp.EStampControl.EStampCancle(textEdit1.Text))
                this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}