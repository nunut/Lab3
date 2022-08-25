using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace VMS_SURE.EStamp
{
    class EStampControl
    {
        private static bool EStampVisitorIdCheckPass(string _visitorId)
        {
            string SQL = "select * from EStampPoint where EStampPoint.EStampVisitorId ='{0}'";
            string SQLString = string.Format(SQL, _visitorId);


            DataTable dt = EStampControl.EStampDatabaseConnect(SQLString);

            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
//----------------------------------------------------------------------------------------------------
        public static bool EStampVisitorIdCheck(string _visitorId)
        {
            bool Stamped = false, DepartmentStamped = false, PolicyStamped = false;
            if (DataStruct.Hardware.EStamp == 1)
            {
                if (DataStruct.EStampCheck.EStampPass == 1)
                {
                    Stamped = EStamp.EStampControl.EStampVisitorIdCheckPass(_visitorId);
                }
                else
                    Stamped = true;

                if (DataStruct.EStampCheck.EStampPassDepartment == 1)
                {
                    DepartmentStamped = true; //ตอนนี้ยังไม่ได้เขียนตรวจสอบ ตามแผนก ให้ผ่านไปก่อน
                }//end if pass department
                else
                    DepartmentStamped = true;

                if (DataStruct.EStampCheck.EStampPassPolicy == 1)
                {
                    PolicyStamped = true;  //ตอนนี้ยังไม่ได้เขียนตรวจสอบตามนโยบาย ให้ผ่านไปก่อน
                }
                else
                    PolicyStamped = true;
            }
            else
            {
                return true;
            }

            return (Stamped && DepartmentStamped && PolicyStamped );
        }
//----------------------------------------------------------------------------------------------------
        private static DataTable EStampDatabaseConnect(string _sqlCommand)
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

        private static DataTable EStampDatabaseCommand(string _sqlCommand)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection objConn = new SqlConnection();
                SqlCommand objCmd = new SqlCommand();
                String strConnString;
                strConnString = config.GlobalString;
                objConn.ConnectionString = strConnString;

                string sqlcommand;

                sqlcommand = _sqlCommand;
                objConn = new SqlConnection(strConnString);

                objCmd = new SqlCommand();
                objCmd.Connection = objConn;
                objCmd.CommandText = sqlcommand;
                objCmd.CommandType = CommandType.Text;

                objConn.Open();
                objCmd.ExecuteNonQuery();

                objConn.Close();
            }
            catch
            {
                return null;
            }

            return dt;
        }

        public static string EStampCancleFindVisitorId(string _cardId)
        {
            string SQL = "select * from visitor where visitor.visitorcardno = '{0}' and visitor.status = '1'";
            SQL = string.Format(SQL,_cardId);
            DataTable dt = EStampDatabaseConnect(SQL);
            if (dt.Rows.Count > 0)
            {
                string sIdCode = dt.Rows[0]["visitorid"].ToString();
                string sCandId = dt.Rows[0]["visitorcardno"].ToString();
                string sVisitorCode = dt.Rows[0]["idcode"].ToString()+"-"+dt.Rows[0]["id"].ToString();
                string msg = string.Format("รหัส {1} บัตร {0} ยกเว้นการประทับตรา เพราะ",sVisitorCode,sCandId);
                return msg;
            }
            return "";
        }

        public static bool EStampCancle(string _cardId)
        {
            string SQL = @"select visitor.id,visitor.name,visitor.visitorcardno,visitor.visitorid,visitor.idcode,visitorcard.cardname
                                         from visitor ,visitorcard where 
                                         visitor.visitorcardno = visitorcard.cardno and visitor.visitorcardno = '{0}' and
                                         visitor.status = '1'";
            SQL = string.Format(SQL, _cardId);
            DataTable dt = EStampDatabaseConnect(SQL);
            if (dt.Rows.Count > 0)
            {
                string sIdCode = dt.Rows[0]["visitorid"].ToString();
                string sCandId = dt.Rows[0]["visitorcardno"].ToString();
                string sVisitorCode = dt.Rows[0]["idcode"].ToString() + "-" + dt.Rows[0]["id"].ToString();
                string sVisitorId = dt.Rows[0]["id"].ToString();
                string sVisitorCardName = dt.Rows[0]["cardname"].ToString();

                string SQLInsert = "INSERT INTO EStampPoint (EStampCardNo,EStampVisitorId,EStampVisitorCode,EStampDeviceLocation,EStampCardName) VALUES ('{0}', '{1}','{2}','ยกเว้นการประทับ','{3}')";
                SQLInsert = string.Format(SQLInsert, sCandId, sIdCode, sVisitorCode, sVisitorCardName);
                EStampDatabaseCommand(SQLInsert);
            }

            return true;
        }
    }
}
