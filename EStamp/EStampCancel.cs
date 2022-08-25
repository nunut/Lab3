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
    public partial class EStampCancel : DevExpress.XtraEditors.XtraForm
    {
        string readcardid;
        public EStampCancel()
        {
            InitializeComponent();
        }

        private void EStampCancel_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == (char)13)
            {
                if (readcardid != null)
                {
                    
                    if (EStamp.EStampControl.EStampCancle(readcardid))
                        alert.ErrorMsg(this, "ยกเลิกประทับตราสำเร็จ", "แจ่งสถานะการทำงาน");
                    Accesscontrol.AccessControl.AccessControlRemoveCardAll(readcardid);
                    this.Close();
                }
            }
            else
            {
                readcardid += e.KeyChar;
            }

        }

        private void EStampCancel_Load(object sender, EventArgs e)
        {
            this.Focus();
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
        }
    }
}