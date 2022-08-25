using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VMS_SURE.LoginServer
{
    class Login
    {
        //-------------------------------------------------------------------------------------------------------------------
        //Function สำหรับต่อฐานข้อมูล โดยส่งเพียง SQL มาและคืนค่ากลับเป็น Datatable สำหรับใช้งานต่อได้เลย
        public static DataTable LoginlDatabaseConnect(string _sqlCommand)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection objConn = new SqlConnection();
                SqlCommand objCmd = new SqlCommand();
                String strConnString;
                strConnString = config.GlobalString;
                objConn.ConnectionString = strConnString;

                SqlConnection connection;
                SqlDataAdapter dataadapter;

                string sqlcommand;

                sqlcommand = _sqlCommand;
                connection = new SqlConnection(strConnString);
                dataadapter = new SqlDataAdapter(sqlcommand, connection);
                dataadapter.Fill(dt);
                connection.Close();
            }
            catch
            {
                return null;
            }

            return dt;
        }

        public int CheckLogin(string user,string pass)
        {
            string sqlLogin = "select * from UserLogin where UserName = '{0}' and Pass= '{1}' ";
            DataTable TableLogin;
            sqlLogin = string.Format(sqlLogin,user,pass);
            TableLogin = LoginlDatabaseConnect(sqlLogin);
            if (TableLogin.Rows.Count > 0)
                return int.Parse(TableLogin.Rows[0]["LevelAccess"].ToString());
            else
                return 0;
            //return 1;
        }

        public static int CheckServerLogin()
        {
            string sql = "select * from ServerSetup";
            DataTable TableServerSetup;

            
            TableServerSetup = LoginlDatabaseConnect(sql);
            if (TableServerSetup != null)
            {
                if (TableServerSetup.Rows.Count > 0)
                {
                    int ServerLoginValue;
                    ServerLoginValue = int.Parse(TableServerSetup.Rows[0]["ServerLogin"].ToString());
                    switch (ServerLoginValue)
                    {
                        case 1:
                            return 1;
                            break;

                            return 0;
                            break;
                    }// end switch
                }// end if
            } //end if
            else
            {
                return -1;
            }

            return 0;
        }
    }
}
