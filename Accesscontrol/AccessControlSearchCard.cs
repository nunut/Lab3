using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace VMS_SURE.Accesscontrol
{
    public partial class AccessControlSearchCard : DevExpress.XtraEditors.XtraForm
    {
        public AccessControlSearchCard()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (textEditSearchCard.Text != "")
            {
                DataTable dtAccessControl =  Accesscontrol.AccessControl.AccessControlDatabaseGetAccessControlList();
                if (dtAccessControl.Rows.Count >= 1)
                {
                    DataTable tableAccessControl = new DataTable("AccessControlDevice");
                    tableAccessControl.Columns.Add("หมายเลขเครื่อง", typeof(string));
                    tableAccessControl.Columns.Add("ชื่อเครื่อง", typeof(string));

                    for (int iAccessControl = 0; iAccessControl < dtAccessControl.Rows.Count; iAccessControl++)
                    {                        

                        string IpAddress = dtAccessControl.Rows[iAccessControl]["AccessDeviceIP"].ToString();
                        RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(IpAddress);
                        IntPtr h = RFIDAccessZK.RFConnect(rd);
                        if (h != (IntPtr)0) tableAccessControl.Rows.Add(new object[] { dtAccessControl.Rows[iAccessControl]["AccessDeviceId"].ToString(), dtAccessControl.Rows[iAccessControl]["AccessDeviceName"].ToString() });
                        string[] s = RFIDAccessZK.RFReaderReadTransection(h, textEditSearchCard.Text);
                        if (s.Length > 1)
                        {
                            //RFIDAccessZK.ConvertData2DateTime(s[1]);
                            List<RFIDAccessZK.UserData> list = RFIDAccessZK.ConvertData2UserData(s);

                            DataTable table = new DataTable("TestTable");
                            table.Columns.Add("หมายเลขประตู", typeof(string));
                            table.Columns.Add("หมายเลขบัตร", typeof(string));
                            table.Columns.Add("วัน เวลา บันทึก", typeof(string));
                            for (int i = 0; i < list.Count; i++)
                            {
                                string rowNumber = Convert.ToString(i + 1);
                                table.Rows.Add(new object[] { list[i].DoorId, list[i].user, list[i].DateTime.ToString() });
                            }
                            gridControl1.DataSource = tableAccessControl;
                            gridView1.PopulateColumns();
                            gridControlUserTrans.DataSource = table;
                            gridView2.PopulateColumns();

                        }
                    }
                }
            }
        }

        /*
         *  DataTable table = new DataTable("TestTable");
            table.Columns.Add("Column1", typeof(string));
            table.Columns.Add("Column2", typeof(string));
            table.Columns.Add("Column3", typeof(string));
            for (int i = 0; i < 5; i++)
            {
                string rowNumber = Convert.ToString(i + 1);
                table.Rows.Add(new object[] { "Item1-" + rowNumber, "Item2-" + rowNumber, "Item3-" + rowNumber });
            }
            GridControl gridControl = new GridControl();
            gridControl.DataSource = table;
            GridView view = new GridView(gridControl);
            gridControl.MainView = view;
            view.PopulateColumns();

            Controls.Add(gridControl);
         * 
         * 
         * 
         * 
         * */
    }
}