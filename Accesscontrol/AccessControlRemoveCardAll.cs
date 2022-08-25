using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.Globalization;

namespace VMS_SURE.Accesscontrol
{
    public partial class AccessControlRemoveCardAll : DevExpress.XtraEditors.XtraForm
    {
        string readcardid;
        SqlConnection objConn = new SqlConnection();
        SqlCommand objCmd = new SqlCommand();
        String strConnString;

        public AccessControlRemoveCardAll()
        {
            InitializeComponent();
        }

        private void timerMifare_Tick(object sender, EventArgs e)
        {
            timerMifare.Enabled = false;
            Mifare.MifareACR1281 mf = new Mifare.MifareACR1281();
            string datacard = mf.ReadMifare();
            if (datacard.Length > 1)
            {
                readcardid = datacard;
                SendKeys.Send("{ENTER}");
            }

            timerMifare.Enabled = true;
        }

        private void AccessControlRemoveCardAll_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == (char)13)
            {
                if (readcardid != null)
                {

                    SqlConnection connection;
                    SqlDataAdapter dataadapter;
                    DataTable dt = new DataTable();
                    String sql = "select * FROM visitorcard  where cardno = '" + readcardid + "' and cardstatus = '1';";

                    connection = new SqlConnection(strConnString);
                    dataadapter = new SqlDataAdapter(sql, connection);
                    dataadapter.Fill(dt);


                    if (dt.Rows.Count > 0)
                    {

                        labelControl1.ForeColor = Color.Green;
                        if (Accesscontrol.AccessControl.AccessControlRemoveCardAll(readcardid))
                        {
                            labelControl1.Text = "ยกเลิกอนุมัตบัตรเข้าพื้นที่แล้ว";
                            this.Close();
                        }
                            labelControl1.Left = (this.ClientSize.Width - labelControl1.Size.Width) / 2;
                       // myParent.txt_cardno.Text = dt.Rows[0]["cardname"].ToString();
                       // myParent.cardid = readcardid;
                       // myParent.updatepaperpass();
                        //readcardid = null;
                    }
                    else
                    {
                        SqlConnection connection2;
                        SqlDataAdapter dataadapter2;
                        DataTable dt2 = new DataTable();
                        String sql2 = "select * FROM visitorcard  where cardno = '" + readcardid + "' and cardstatus = '2';";

                        connection2 = new SqlConnection(strConnString);
                        dataadapter2 = new SqlDataAdapter(sql2, connection2);
                        dataadapter2.Fill(dt2);

                        if (dt2.Rows.Count > 0)
                        {
                            labelControl1.ForeColor = Color.Crimson;
                            //labelControl1.Text = "บัตรถูกใช้แล้ว";
                            labelControl1.Left = (this.ClientSize.Width - labelControl1.Size.Width) / 2;
                            if (Accesscontrol.AccessControl.AccessControlRemoveCardAll(readcardid))
                            {
                                labelControl1.Text = "ยกเลิกอนุมัตบัตรเข้าพื้นที่แล้ว";
                                this.Close();
                            }
                            else
                            {
                                alert.ErrorMsg(this, "ไม่สามารถยกเลิกอนุมัติบัตรเข้าพื้นที่ โปรดตรวจสอบ");
                            }
                            readcardid = null;
                        }
                        else
                        {

                            labelControl1.ForeColor = Color.Red;
                            labelControl1.Text = "ไม่มีบัตรนี้ในระบบ";
                            labelControl1.Left = (this.ClientSize.Width - labelControl1.Size.Width) / 2;
                            readcardid = null;
                        }
                    }



                    /*labelControl1.ForeColor = Color.Green;
                    labelControl1.Text = "อ่านบัตรแล้ว";
                    myParent.txt_cardno.Text = readcardid;
                    readcardid = null;*/
                }

                if (labelControl1.Text == "อ่านบัตรแล้ว")
                {
                    //System.Threading.Thread.Sleep(3000);
                   

                }

            }
            else //if ((e.KeyChar >= (char)48 ) && (e.KeyChar <= (char)57))
            {
                readcardid += e.KeyChar;
            }
        }

        private void AccessControlRemoveCardAll_Load(object sender, EventArgs e)
        {
            this.Focus();
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
            strConnString = config.GlobalString;
            objConn.ConnectionString = strConnString;
            if (objConn.State == ConnectionState.Closed)
                objConn.Open(); 

            if (DataStruct.Hardware.Mifare == 1)
            {
                timerMifare.Enabled = true;
            }


        }
    }
}