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

namespace VMS_SURE.Accesscontrol
{
    public partial class AccessControlTransView : DevExpress.XtraEditors.XtraForm
    {
        SqlConnection objConn = new SqlConnection();
        SqlCommand objCmd = new SqlCommand();
        String strConnString;

        private SqlConnection sqlConnection = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private SqlCommandBuilder sqlCommandBuilder = null;
        private DataTable dataTable = null;
        private BindingSource bindingSource = null;
        private String selectQueryString = null;
        public bool FormShowAllData = false;


        string VisitorId;
        string[,] Field = { { "AccessDeviceId", "หมายเลขเครื่อง" }, { "AccessDeviceName", "ชื่อเครื่อง" }, { "department_name", "แผนก" }, { "TimeStamp", "เวลา" }, { "DateStamp", "วัน" }, { "visitorcardno", "หมายเลขบัตร" }, { "visitorid", "รหัสใบผ่าน" } };
        public AccessControlTransView()
        {
            InitializeComponent();

            loaddata();
        }

        private void AccessControlTransView_Load(object sender, EventArgs e)
        {

        }
        public void loaddata()
        {
            strConnString = config.GlobalString;
            sqlConnection = new SqlConnection(strConnString);
            selectQueryString = "select * from AccessControlTrans";

            sqlConnection.Open();
            sqlDataAdapter = new SqlDataAdapter(selectQueryString, sqlConnection);
            sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
            dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            bindingSource = new BindingSource();
            bindingSource.DataSource = dataTable;
            gridControl1.DataSource = bindingSource;
            gridView1.ShowFindPanel();

            int count=0;
            if(Field.Rank >0) count = Field.Length/Field.Rank;
            for (int i = 0; i < count; i++)
            {
                gridView1.Columns[Field[i,0]].Caption = Field[i,1];
                gridView1.Columns[Field[i, 0]].Visible = true;
               
            }


            for (int i = 0; i < gridView1.Columns.Count(); i++)
            {
                gridView1.Columns[i].BestFit();
                gridView1.Columns[i].Visible = false;
            }

            for (int i = 0; i < count; i++)
            {
                gridView1.Columns[Field[i, 0]].Visible = true;
            }

           // gridView1.Columns["ItemOutQty"].Visible = true;





        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabPage2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridControl2_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}