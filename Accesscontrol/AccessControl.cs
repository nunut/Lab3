using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace VMS_SURE.Accesscontrol
{


    class AccessControl
    {

        public static class AccessTransType
        {
            public const int ADD_CARD = 1;
            public const int DEL_CARD = 0;
            public const int ACCESS_DOOR = 9;
        }
        //-------------------------------------------------------------------------------------------------------------------
        //Function สำหรับต่อฐานข้อมูล โดยส่งเพียง SQL มาและคืนค่ากลับเป็น Datatable สำหรับใช้งานต่อได้เลย
        public static DataTable AccessControlDatabaseConnect(string _sqlCommand)
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
        //-------------------------------------------------------------------------------------------------------------------
        //สำหรับเพิ่มบัตรเข้าเครื่อง เป็นโปรแกรมหลักที่เช็คเงือนไขต่าง ๆ สำหรับการกำหนดสิทธิ์การเข้า
        // _UsingPolicyDepartment = ใช้นโยบายตามแผนก(สั่งงานได้หลายเครื่อง ได้ทุกนโยบายที่แผนกนั้น ๆ อยู่่)
        // _UsingDepartment = ใช้แผนก (จะได้เพียง 1 เครื่องที่ระบุแผนกไว้ตรงกัน)
        public static string AccessControlPolicyDepartmentAddCard(string _idCard, string _department, bool _UsingPolicyDepartment = false, bool _UsingDepartment = false, bool _UsingPolicy = false)
        {
            /* Check policy 
             *  Condition work flow 
             *  start -> department&policy -> AccessControlPolicyAddCard()
             *           department only   -> AccessControlDepartmentAddCard()
             *           policy only       -> AccessControlPolicyAddCard() // ยังไม่ใช้งาน
             *           
             * */
            //ยังไม่ได้ ตรวจสอบการทำงาน
            DataTable dtPolicy;
            DataTable dtDepartment;
            string MessageReturn = "";

            bool UsingPolicy = _UsingPolicy, UsingDepartment = _UsingDepartment, UsingPolicyDepartment = _UsingPolicyDepartment;

            string sqlDepartment = string.Format("select * from AccessControl , AccessPolicy where AccessPolicy.department_name = '{0}' and  AccessControl.AccessId = AccessPolicy.AccessId", _department);
            dtDepartment = AccessControlDatabaseConnect(sqlDepartment);

            //ถ้าใช้ทั้ง แผนกและนโยบายให้สั่งเขียนทุก ๆ นโยบายของแผนกนั้น ๆ 
            if ((UsingPolicyDepartment == true) && (dtDepartment.Rows.Count >= 1))
            {
                for (int i = 0; i < dtDepartment.Rows.Count; i++)
                {
                    AccessControlPolicyAddCard(_idCard, dtDepartment.Rows[i]["AccessPolicyId"].ToString());
                    MessageReturn += string.Format("เพิ่มบัตรหมายเลข {0} ให้มีสิทธิเข้าพื้นที่ควบคุม {1} เรียบร้อย\n", _idCard, _department);
                }
            }
            else if (UsingPolicyDepartment == false && UsingDepartment == true) //ถ้าไม่ใช้ นโยบายร่วมกับแผนก แต่มีการใช้แผนกเท่านั้น ก็ให้ทุก ๆ เครื่องที่มีแผนกกำหนดไว้ทำงานได้
            {
                AccessControlDepartmentAddCard(_idCard, _department);
            }

            //ถ้าไม่ใช้นโยบายร่วมกับแผนก และก็ไม่ได้ใช้แผนก แต่ใช้นโยบาย อย่างเดียวสั่งทุกเครื่อง ตามนโยบายที่ระบุแผนกไว้
            if (UsingPolicy == true && UsingPolicyDepartment == false)
            {

                string sqlPolicy = string.Format("select * from  AccessPolicy where AccessPolicy.department_name = '{0}'", _department);
                dtPolicy = AccessControlDatabaseConnect(sqlPolicy);
                if (dtPolicy.Rows.Count >= 1)
                {
                    for (int i = 0; i < dtPolicy.Rows.Count; i++)
                    {
                        //AccessControlPolicyOnlyAddCard(_idCard, dtPolicy.Rows[i]["AccessPolicyRef"].ToString());
                        AccessControlPolicyAddCard(_idCard, dtPolicy.Rows[i]["AccessPolicyRef"].ToString());

                    }
                }
            }

            return "";
        }
        //-------------------------------------------------------------------------------------------------------------------
        //สั่งทุกเครื่องที่มีแผนกตรงกับที่ต้องการ <======= (function หลักในการทำงาน สำหรับแบบมาตราฐาน !)
        public static string AccessControlDepartmentAddCard(string _idCard, string _department, string _name = "",string _visitorId="0")
        {

            DataTable dt;
            string MessageReturn = "";

            try
            {
                string sql = string.Format("select * from AccessControl where AccessControl.department_name = '{0}'", _department);

                dt = AccessControlDatabaseConnect(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                        string AccessId = dt.Rows[i]["AccessId"].ToString();
                        //พยายมแก้ไขปัญหา bug กรณีติดต่่อเครื่องไม่ได้ และสั่งโปรแกรมพยาม ติดต่อ 3 ครั้ง
                        for (int retry = 0; retry <=3;retry++ )
                        {
                            if (AccessControlDeviceActive(AccessId))
                            {
                                RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                                IntPtr h = RFIDAccessZK.RFConnect(rd);
                                int iretry = RFIDAccessZK.RFReaderWriteCard(h, _idCard, _idCard, "", "", "", _name);
                                if (iretry == 1) retry = 10;
                                MessageReturn += string.Format("เพิ่มบัตรหมายเลข {0} ให้มีสิทธิเข้าพื้นที่ควบคุม {1} เรียบร้อย\n", _idCard, _department);
                                AccessControlTransAddCard(_idCard, dt.Rows[i]["AccessId"].ToString(), _visitorId, _department);
                                RFIDAccessZK.Disconnect(h);
                            }
                        }// end for 
                    }

                }
                
                _name = ""; // for clear name for add to e-stamp
                AccessControlDepartmentAddCard2(_idCard, _department, _name, _visitorId);// เพิ่มบัตรที่แลกเพิ่มไปที่เครื่องด้วย
            }
            catch
            {
                return "";
            }

            return MessageReturn;
        }
        //-------------------------------------------------------------------------------------------------------------------
        //function นี้ใช้สำหรับเพิ่มบัตรไปที่เครื่องตามแผนก ใช้ในกรณีมีผู้ติดตามแลกบัตรเพิ่มไว้
        public static string AccessControlDepartmentAddCard2(string _idCard, string _department, string _name = "", string _visitorId="0")
        {
            
            DataTable dtVisitor2;
            DataTable dt;
            string MessageReturn = "";

            try
            {
                string sqlvisitor2 = string.Format(@"select * from 
                                                    (select visitor2.idcode+'-'+visitor2.id as visitoridcode,
                                                            visitor2.name,visitor2.surname,visitor2.department,
                                                            visitor2.visitorcardno,visitorcard.cardno
                                                            from visitor2,visitorcard where visitor2.visitorcardno = visitorcard.cardname) as visitor2data
                                                            where visitor2data.visitoridcode = '{0}'", _visitorId);

                dtVisitor2 = AccessControlDatabaseConnect(sqlvisitor2);
                if (dtVisitor2.Rows.Count > 0)
                {
                    string sql = string.Format("select * from AccessControl where AccessControl.department_name = '{0}'", _department);

                    dt = AccessControlDatabaseConnect(sql);


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                        string AccessId = dt.Rows[i]["AccessId"].ToString();
                        if (AccessControlDeviceActive(AccessId))
                        {
                            if (dtVisitor2.Rows.Count > 0)
                            {
                                for (int i2 = 0; i2 < dtVisitor2.Rows.Count; i2++)
                                {
                                    string idCard = "";
                                    idCard = dtVisitor2.Rows[i2]["cardno"].ToString();

                                    RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                                    IntPtr h = RFIDAccessZK.RFConnect(rd);
                                    RFIDAccessZK.RFReaderWriteCard(h, idCard, idCard, "", "", "", _name);
                                    MessageReturn += string.Format("เพิ่มบัตรหมายเลข {0} ให้มีสิทธิเข้าพื้นที่ควบคุม {1} เรียบร้อย\n", _idCard, _department);
                                    AccessControlTransAddCard(_idCard, dt.Rows[i]["AccessId"].ToString(), _visitorId, _department);
                                    RFIDAccessZK.Disconnect(h);
                                }//end for i2
                            }//end if i2

                        }// end if
                    }// end for i
 

                }// end if 
            }
            catch
            {
                return "";
            }

            return MessageReturn;

        }
        //-------------------------------------------------------------------------------------------------------------------
        //function นี้ใช้สำหรับเพิ่มบัตรไปที่เครื่องตามแผนก ใช้ในกรณีมีผู้ติดตามแลกบัตรเพิ่มไว้ ยังไม่เสร็จ
        public static string AccessControlDepartmentAddCardFollow()
        {
            
            return "";
        } 
        //-------------------------------------------------------------------------------------------------------------------
        public static string AccessControlPolicyAddCard(string _idCard, string _policy)
        {

            DataTable dt;
            string MessageReturn = "";


            try
            {
                string sql = string.Format("select * from AccessControl , AccessPolicy where AccessPolicy.AccessPolicyId = '{0}' and AccessControl.AccessId = AccessPolicy.AccessId", _policy);
                dt = AccessControlDatabaseConnect(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                        RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                        IntPtr h = RFIDAccessZK.RFConnect(rd);
                        RFIDAccessZK.RFReaderWriteCard(h, _idCard, _idCard, "", "", "", "");
                        MessageReturn += string.Format("เพิ่มบัตรหมายเลข {0} ให้มีสิทธิเข้าพื้นที่ควบคุม {1} เรียบร้อย\n", _idCard, _policy);
                        AccessControlTransAddCard(_idCard, dt.Rows[i]["AccessId"].ToString(), _idCard, _idCard);
                        RFIDAccessZK.Disconnect(h);
                    }
                    SURELog.LogErrorWrite(MessageReturn);
                }
            }
            catch
            {
                return "";
            }

            return MessageReturn;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static string AccessControlPolicyOnlyAddCard(string _idCard, string _policy)
        {

            DataTable dt;

            try
            {
                string sql = string.Format("select * from  AccessPolicy where AccessPolicy.AccessPolicyRef = '{0}'", _policy);
                dt = AccessControlDatabaseConnect(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessPolicyRef"].ToString();
                        RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                        IntPtr h = RFIDAccessZK.RFConnect(rd);
                        RFIDAccessZK.RFReaderWriteCard(h, _idCard, _idCard);
                        RFIDAccessZK.Disconnect(h);
                    }
                }
            }
            catch
            {
                return "";
            }

            return "OK";
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static string AccessControlPolicyDelCard(string _idCard, string _policy)
        {

            DataTable dt;


            try
            {
                string sql = string.Format("select * from AccessControl , AccessPolicy where AccessPolicy.AccessPolicyId = '{0}' and AccessControl.AccessId = AccessPolicy.AccessId", _policy);
                dt = AccessControlDatabaseConnect(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                        RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                        IntPtr h = RFIDAccessZK.RFConnect(rd);
                        RFIDAccessZK.RFReaderDeleteCard(h, _idCard);
                        RFIDAccessZK.Disconnect(h);
                    }
                }
            }
            catch
            {
                return "";
            }

            return "OK";
        }
        //----------------------------------------------------------------------------------------------------
        //ลบบัตรออกจากเครื่องทุกเครื่อง
        public static bool AccessControlRemoveCardAll(string _cardId)
        {
            DataTable dt;

            try
            {
                string sql = string.Format("select AccessDeviceIP from AccessControl group by AccessDeviceIP");
                dt = AccessControlDatabaseConnect(sql);
                if (dt.Rows.Count >= 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                        RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                        IntPtr h = RFIDAccessZK.RFConnect(rd);
                        RFIDAccessZK.RFReaderDeleteCard(h, _cardId);
                        RFIDAccessZK.Disconnect(h);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        //----------------------------------------------------------------------------------------------------
        //ลบบัตผู้ติดตามที่แลกมาพร้อมกัน ออกจากเครื่องทุกเครื่อง
        public static bool AccessControlRemoveCardAll2(string _visitorId)
        {
            DataTable dt,dt2;

            try
            {
                /*string sql2 = string.Format(@"select visitorcard.cardno from visitor,visitor2,visitorcard
                                             where visitor.visitorcardno = '{0}'
                                             and visitor.status = 1
                                             and visitor.id = visitor2.id and visitor.idcode = visitor2.idcode
                                             and visitor2.visitorcardno = visitorcard.cardname", _cardId);
                 * */
                string sql2 = string.Format(@"select visitorcard.cardno from visitor2,visitorcard,visitor
                                              where visitorcard.cardname = visitor2.visitorcardno
                                              and visitor.visitorid = '{0}' and visitor2.id = visitor.id
                                              and visitor2.idcode = visitor.idcode", _visitorId);

                dt2 = AccessControlDatabaseConnect(sql2);


                        string sql = string.Format("select AccessDeviceIP from AccessControl group by AccessDeviceIP");
                        
                        dt = AccessControlDatabaseConnect(sql);
                        if (dt.Rows.Count >= 1)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                
                                string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                                RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                                IntPtr h = RFIDAccessZK.RFConnect(rd);

                                if (dt2.Rows.Count >= 1)
                                {
                                    for (int i2 = 0; i2 < dt2.Rows.Count; i2++)
                                    {
                                        string cardid = dt2.Rows[i2]["cardno"].ToString();
                                        RFIDAccessZK.RFReaderDeleteCard(h, cardid);
                                    }
                                }
                                RFIDAccessZK.Disconnect(h);
                            }
                        }

            }
            catch
            {
                return false;
            }
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool TAccessControlRemoveCardAll(string _cardId)
        {
            try
            {
                Thread t = new Thread(() => AccessControlRemoveCardAll(_cardId));
                t.Start();
                t.Join(0);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool TAccessControlRemoveCardAll2(string _cardId)
        {
            try
            {
                Thread t = new Thread(() => AccessControlRemoveCardAll2(_cardId));
                t.Start();
                t.Join(0);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool TAccessControlDepartmentAddCard(string _idCard, string _department, string _name = "",string _visitorId="0")
        {
            try
            {
                Thread t = new Thread(() => AccessControlDepartmentAddCard(_idCard, _department, _name,_visitorId));
                t.Start();
                t.Join(0);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool TAccessControlPolicyDepartmentAddCard(string _idCard, string _department, bool _UsingPolicyDepartment = false, bool _UsingDepartment = false, bool _UsingPolicy = false)
        {
            try
                
            {
                _UsingPolicyDepartment = true;
                Thread t = new Thread(() => AccessControlPolicyDepartmentAddCard(_idCard, _department, _UsingPolicyDepartment, _UsingDepartment, _UsingPolicy));
                t.Start();
                t.Join(0);
                return true;
            }
            catch
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool AccessControlAutoOpenDoor(string _accessId = "0",int _delay=5)
        {
            if (AccessControlDeviceActive(_accessId))
            {
                DataTable dt;

                try
                {
                    string sql = string.Format("select AccessDeviceIP from AccessControl where AccessControlAutoOpen=1 and AccessId = '{0}'  group by AccessDeviceIP", _accessId);
                    dt = AccessControlDatabaseConnect(sql);
                    if (dt.Rows.Count >= 1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                            RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                            IntPtr h = RFIDAccessZK.RFConnect(rd);
                            RFIDAccessZK.RFControlOpenDoor(h, _delay);
                            RFIDAccessZK.Disconnect(h);
                        }
                    }
                }
                catch
                {
                    return false;
                }
                return true;
            }
            else
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static bool AccessControlAutoCloseDoor(string _accessId = "0")
        {
            if(AccessControlDeviceActive(_accessId))
            {
            DataTable dt;

            try
            {
                //AccessControlGateOut
                string sql = string.Format("select AccessDeviceIP from AccessControl where AccessControlAutoClose=1 and AccessId = '{0}'  group by AccessDeviceIP", _accessId);
                dt = AccessControlDatabaseConnect(sql);
                if (dt.Rows.Count >= 1)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string DeviceIp = dt.Rows[i]["AccessDeviceIP"].ToString();
                        RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                        IntPtr h = RFIDAccessZK.RFConnect(rd);
                        RFIDAccessZK.RFControlCloseDoor(h);
                        RFIDAccessZK.Disconnect(h);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
            }
            else
                return false;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public static bool AccessControlTransLogCard(string _transType, string _idCard, string _accessId, string _visitorId = "0", string _department = "")
        {
            try
            {
                SqlConnection objConn = new SqlConnection();
                objConn.ConnectionString = config.GlobalString;
                if (objConn.State == ConnectionState.Closed)
                {
                    objConn.Open();
                }

                DataTable tData = AccessControlDatabaseGetAccessControl(_accessId);
                if (tData != null)
                {
                    for (int i = 0; i < tData.Rows.Count; i++)
                    {
                        string AccessDeviceId = tData.Rows[i]["AccessDeviceId"].ToString();
                        string AccessDeviceName = tData.Rows[i]["AccessDeviceName"].ToString();
                        string AccessDeviceIP = tData.Rows[i]["AccessDeviceIP"].ToString();
                        string AccessDeviceGroupId = tData.Rows[i]["AccessDeviceGroupId"].ToString();
                        SqlCommand objCmd = new SqlCommand();
                        string sql = "Insert Into AccessControlTrans  (AccessId,AccessDeviceId,visitorcardno,visitorid,AccessTransType,DateStamp,TimeStamp,AccessDeviceName,AccessDeviceIP,AccessDeviceGroupId) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')";
                        String StrSQL = string.Format(sql, _accessId, AccessDeviceId, _idCard, _visitorId, _transType, DateTime.Today.ToString(), DateTime.Now.ToString(), AccessDeviceName, AccessDeviceIP, AccessDeviceGroupId);

                        objCmd = new SqlCommand();
                        objCmd.Connection = objConn;
                        objCmd.CommandText = StrSQL;
                        objCmd.CommandType = CommandType.Text;

                        objCmd.ExecuteNonQuery();
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        //-----------------------------------------------------------------------------------------------
        public static bool AccessControlTransAddCard(string _idCard, string _accessId, string _visitorId = "0", string _department = "")
        {
            AccessControlTransLogCard(AccessTransType.ADD_CARD.ToString(), _idCard, _accessId, _visitorId, _department);
            return true;
        }
        //-----------------------------------------------------------------------------------------------
        public static bool AccessControlTransRemoveCard(string _idCard, string _accessId, string _visitorId = "0", string _department = "")
        {
            AccessControlTransLogCard(AccessTransType.DEL_CARD.ToString(), _idCard, _accessId, _visitorId, _department);
            return true;
        }
        //-----------------------------------------------------------------------------------------------
        public static bool AccessControlTransEntryCard(string _idCard, string _accessId, string _visitorId = "0", string _department = "")
        {
            AccessControlTransLogCard(AccessTransType.ACCESS_DOOR.ToString(), _idCard, _accessId, _visitorId, _department);
            return true;
        }
        //-----------------------------------------------------------------------------------------------
        public static DataTable AccessControlDatabaseGetAccessControl(string _accessId)
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

                sqlcommand = string.Format("select * from AccessControl where AccessId = '{0}'", _accessId);
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
        //-----------------------------------------------------------------------------------------------------------------
        public static DataTable AccessControlDatabaseGetAccessControlList()
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

                sqlcommand = string.Format("select * from AccessControl");
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
        //-------------------------------------------------------------------------------------------------------------------
        public static bool AccessControlDeviceActive(string _accessId) //ใช้สำหรับตรวจสอบว่ามี Licen หรือไม่
        {
            return true; // by pass code don't check licen
            try
            {
                DataTable dt = new DataTable("AccessControlDevice");
                dt = AccessControlDatabaseGetAccessControl(_accessId);


                string DeviceIp = dt.Rows[0]["AccessDeviceIP"].ToString();
                RFIDAccessZK.RFReader rd = RFIDAccessZK.RFReaderInit(DeviceIp);
                IntPtr h = RFIDAccessZK.RFConnect(rd);
                string SerialId = RFIDAccessZK.GetSerialNumber(h);
                string Key = "224236248251026122714281629";
                char[] charKey = Key.ToCharArray();
                char[] charSeial = SerialId.ToCharArray();

                string LicenKey;

                char[] result = new char[SerialId.Length];
                for (int i = 0; i < SerialId.Length; i++)
                {
                    if (charKey[i] > charSeial[i])
                        result[i] = charKey[i];
                    else
                        result[i] = charSeial[i];
                }
                RFIDAccessZK.RFDisConnect(h);

                LicenKey = new string(result);
                if (LicenKey == dt.Rows[0]["AccessControlLicen"].ToString())
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }

        }
        //-------------------------------------------------------------------------------------------------------------------
    }
}
