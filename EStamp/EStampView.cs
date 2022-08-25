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

namespace VMS_SURE.EStamp
{
    public partial class EStampView : DevExpress.XtraEditors.XtraForm
    {
        public EStampView()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (textEditVisitorIdCode.Text != "")
            {
                SearchEStampDataByVisitorIdCode(textEditVisitorIdCode.Text);
            }
        }
        private void SearchEStampDataByVisitorIdCode(string _visitorIdCode)
        {

            SqlConnection objConn = new SqlConnection();
            SqlCommand objCmd = new SqlCommand();
            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            SqlCommandBuilder sqlCommandBuilder = null;
            DataTable dataTableEdit = null;
            BindingSource bindingSource = null;
            String selectQueryString = null;

            string strConnString;
            strConnString = config.GlobalString;

            try
            {


                //  gridControl3.UseEmbeddedNavigator = true;
                sqlConnection = new SqlConnection(strConnString);
                selectQueryString = @"select * from 
                                             (select EStampPoint.EStampCardName,EStampPoint.EStampCardNo,EStampPoint.EStampDate,
                                             EStampPoint.EStampTime,EStampPoint.EStampDoorId,EStampPoint.EStampVisitorName,
                                             EStampPoint.EStampDeviceId,EStampPoint.EStampDeviceName,EStampPoint.EStampDeviceLocation,
                                             visitor.idcode+'-'+visitor.id as visitoridcode
                                             from EstampPoint,visitor where
                                             EStampPoint.EStampVisitorId = visitor.visitorid ) as visitordata
                                             where visitordata.visitoridcode = '"+_visitorIdCode+"'";
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter(selectQueryString, sqlConnection);
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataTableEdit = new DataTable();
                sqlDataAdapter.Fill(dataTableEdit);
                bindingSource = new BindingSource();
                bindingSource.DataSource = dataTableEdit;
                gridControl4.DataSource = bindingSource;
                gridView4.Columns["EStampCardName"].Caption = "บัตรเลขที่";
                gridView4.Columns["EStampCardNo"].Caption = "รหัสบัตร";
                gridView4.Columns["EStampDate"].Caption = "ประทับวันที่";
                gridView4.Columns["EStampTime"].Caption = "ประทับเวลา";
                gridView4.Columns["EStampDoorId"].Caption = "ช่อง";
                gridView4.Columns["EStampVisitorName"].Caption = "ชื่อ";
                gridView4.Columns["EStampDeviceId"].Caption = "รหัสจุดประทับ";
                gridView4.Columns["EStampDeviceName"].Caption = "ชื่อจุดประทับ";
                gridView4.Columns["EStampDeviceLocation"].Caption = "บริเวณจุดประทับ";


            }
            catch { }
        }

        private void SearchEStampDataByVisitorCardNo(string _visitorCardNo)
        {

            SqlConnection objConn = new SqlConnection();
            SqlCommand objCmd = new SqlCommand();
            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            SqlCommandBuilder sqlCommandBuilder = null;
            DataTable dataTableEdit = null;
            BindingSource bindingSource = null;
            String selectQueryString = null;

            string strConnString;
            strConnString = config.GlobalString;

            try
            {


                //  gridControl3.UseEmbeddedNavigator = true;
                sqlConnection = new SqlConnection(strConnString);
                selectQueryString = @"select * from 
                                             (select EStampPoint.EStampCardName,EStampPoint.EStampCardNo,EStampPoint.EStampDate,
                                             EStampPoint.EStampTime,EStampPoint.EStampDoorId,EStampPoint.EStampVisitorName,
                                             EStampPoint.EStampDeviceId,EStampPoint.EStampDeviceName,EStampPoint.EStampDeviceLocation,
                                             visitor.idcode+'-'+visitor.id as visitoridcode,visitor.visitorcardno
                                             from EstampPoint,visitor where
                                             EStampPoint.EStampVisitorId = visitor.visitorid ) as visitordata
                                             where visitordata.visitorcardno = '" + _visitorCardNo + "'";
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter(selectQueryString, sqlConnection);
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataTableEdit = new DataTable();
                sqlDataAdapter.Fill(dataTableEdit);
                bindingSource = new BindingSource();
                bindingSource.DataSource = dataTableEdit;
                gridControl4.DataSource = bindingSource;
                gridView4.Columns["EStampCardName"].Caption = "บัตรเลขที่";
                gridView4.Columns["EStampCardNo"].Caption = "รหัสบัตร";
                gridView4.Columns["EStampDate"].Caption = "ประทับวันที่";
                gridView4.Columns["EStampTime"].Caption = "ประทับเวลา";
                gridView4.Columns["EStampDoorId"].Caption = "ช่อง";
                gridView4.Columns["EStampVisitorName"].Caption = "ชื่อ";
                gridView4.Columns["EStampDeviceId"].Caption = "รหัสจุดประทับ";
                gridView4.Columns["EStampDeviceName"].Caption = "ชื่อจุดประทับ";
                gridView4.Columns["EStampDeviceLocation"].Caption = "บริเวณจุดประทับ";


            }
            catch { }
        }

        private void SearchEStampRegisterByVisitorIdCode(string _visitorIdCode)
        {
            SqlConnection objConn = new SqlConnection();
            SqlCommand objCmd = new SqlCommand();
            SqlConnection sqlConnection = null;
            SqlDataAdapter sqlDataAdapter = null;
            SqlCommandBuilder sqlCommandBuilder = null;
            DataTable dataTableEdit = null;
            BindingSource bindingSource = null;
            String selectQueryString = null;

            string strConnString;
            strConnString = config.GlobalString;

            try
            {


                //  gridControl3.UseEmbeddedNavigator = true;
                sqlConnection = new SqlConnection(strConnString);
                selectQueryString = @"select AccessId,AccessDeviceId,AccessDeviceName,TimeStamp,AccessDeviceGroupId,visitorid,visitorcardno
                                             from AccessControlTrans 
                                             where AccessControlTrans.visitorid = '" + _visitorIdCode + "'";
                sqlConnection.Open();
                sqlDataAdapter = new SqlDataAdapter(selectQueryString, sqlConnection);
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataTableEdit = new DataTable();
                sqlDataAdapter.Fill(dataTableEdit);
                bindingSource = new BindingSource();
                bindingSource.DataSource = dataTableEdit;
                gridControl1.DataSource = bindingSource;
                gridView1.Columns["AccessId"].Caption = "รหัสเครื่อง";
                gridView1.Columns["AccessDeviceName"].Caption = "ชื่อเครื่อง";
                gridView1.Columns["TimeStamp"].Caption = "เวลาประทับ";
                gridView1.Columns["AccessDeviceGroupId"].Caption = "แผนก";



            }
            catch { }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SearchEStampRegisterByVisitorIdCode(textEdit1.Text);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SearchEStampDataByVisitorCardNo(textEditVisitorCardNo.Text);
        }

        private void EStampView_Load(object sender, EventArgs e)
        {
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
        }

        private void textEditVisitorCardNo_Enter(object sender, EventArgs e)
        {
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
        }

        private void textEditVisitorIdCode_Enter(object sender, EventArgs e)
        {
            CultureInfo TypeOfLanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = TypeOfLanguage;
            InputLanguage l = InputLanguage.FromCulture(TypeOfLanguage);
            InputLanguage.CurrentInputLanguage = l;
        }
    }
}