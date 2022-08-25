using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace VMS_SURE
{
    class RFIDAccessZK
    {
        public class RFReader
        {
            public string Protocal = "TCP";
            public string IPaddress=null;
            public string PortNumber=null;
            public string Name=null;
            public string User = null;
            public string Password = null;
            public int Timeout = 2000;
            public bool Status=false;
        }

        public class UserData
        {
            //Pin=19999	CardNo=13375401	Password=1
            public string user;
            public string pin;
            public string password;
            public string group;
            public DateTime DateTime;
            public string TimeZone;
            public string Date;
            public string Time;
            public string DoorId;
            public string InOutState;

        }


//----------------------------------------------------------------------------------------------------------------------------

        public static RFReader RFReaderInit(string _ipAddress="192.168.1.100",string _passWord="",string _portNumber="4370",int _timeOut = 2000)
        {
            RFReader rfReader = new RFReader();

            try
            {
                if (rfReader == null)
                    return null;
                else
                {
                    rfReader.IPaddress = _ipAddress;
                    rfReader.Password = _passWord;
                    rfReader.PortNumber = _portNumber;
                    rfReader.Timeout = _timeOut;
                    return rfReader;
                }
            }
            catch
            {
                int ret = PullLastError();
                return rfReader;
            }
        }

//----------------------------------------------------------------------------------------------------------------------------
//Function for connect ZK device from SDK

        [DllImport("plcommpro.dll", EntryPoint = "Connect")]
        public static extern IntPtr Connect(string Parameters);
        [DllImport("plcommpro.dll", EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);
        [DllImport("plcommpro.dll", EntryPoint = "PullLastError")]
        public static extern int PullLastError();
//----------------------------------------------------------------------------------------------------------------------------
        public static IntPtr RFConnect(RFReader _rfReader)
        {

            //Ref:: protocol=TCP,ipaddress=192.168.8.122,port=4370,timeout=2000,passwd=
            
            try
            {
                string sConnection = "";
                RFReader RFReader = _rfReader;

                //string.Format("{0}",(_rfReader.IPaddress == "") ? "" : _rfReader.IPaddress); // code for auto insert string
                //
                if (RFReader == null)
                {
                    _rfReader = RFReaderInit();
                    if (_rfReader == null)
                        return IntPtr.Zero;
                }


                sConnection = string.Format("protocol={0},ipaddress={1},port={2},timeout={3},passwd={4}", RFReader.Protocal, RFReader.IPaddress, RFReader.PortNumber, RFReader.Timeout, RFReader.Password);
                IntPtr h = IntPtr.Zero;
                if (IntPtr.Zero == h)
                {
                    if (sConnection == "")
                    {
                        return IntPtr.Zero;
                    }
                    else
                    {
                        h = Connect(sConnection);
                        if (h != IntPtr.Zero)
                        {
                            return h;
                        }
                    }

                }// end if check IntPtr connected
                return h;
            }
            catch
            {
                int ret = PullLastError();
                return IntPtr.Zero;
            }
        }

//----------------------------------------------------------------------------------------------------------------------------
        public static void RFDisConnect(IntPtr _hRFReader)
        {
            Disconnect(_hRFReader);
        }
//----------------------------------------------------------------------------------------------------------------------------      
        [DllImport("plcommpro.dll", EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string filename, string filter, string options);

//----------------------------------------------------------------------------------------------------------------------------
        public static string[] RFReaderReadCard(IntPtr _hRFReader,string _readMode="all",string _filter="")
        {
            //=================================
            string devtablename = "user";
            string command = "CardNo\tPin\tPassword\tGroup\tStartTime\tEndTime";
            string datafilter = "";
            string options = "";
            //================================

            IntPtr h = IntPtr.Zero;
            int ret = 0;

            int BUFFERSIZE = 1 * 1024 * 1024;
            byte[] buffer = new byte[BUFFERSIZE];
            string[] ReadCard = new string[1024];

            h = _hRFReader;
            if (IntPtr.Zero != h)
            {
                ret = GetDeviceData(h, ref buffer[0], BUFFERSIZE, devtablename, command, datafilter, options);
            }
  

            if (ret >= 0)
            {
                string sReadCard = Encoding.Default.GetString(buffer);
                ReadCard = sReadCard.Split('\n');

                var list = new List<string>(ReadCard);
                list.RemoveAt(ReadCard.Length-1);
                ReadCard = list.ToArray();
            }
            else
            {
                return ReadCard;
            }
            return ReadCard;
        }
//----------------------------------------------------------------------------------------------------------------------------
        [DllImport("plcommpro.dll", EntryPoint = "SetDeviceData")]
        public static extern int SetDeviceData(IntPtr h, string tablename, string data, string options);
        [DllImport("plcommpro.dll", EntryPoint = "DeleteDeviceData")]
        public static extern int DeleteDeviceData(IntPtr h, string tablename, string data, string options);
//----------------------------------------------------------------------------------------------------------------------------
        public static int RFReaderWriteCard(IntPtr _hRFReader, string _card,string _pin = "",string _password ="",string _group ="", string _options = "",string _name="")
        {
            string[] sret = { "" };
            int ret = 0;
            //=================================
            string devtablename = "user";
            string options = _options;
            string data = "";
            IntPtr h = _hRFReader;
            //Pin=19999	CardNo=13375401	Password=1
            //Pin=19999\tCardNo=13375401\tPassword=1\r\nPin=2\tCardNo=14128058\tPassword=1
            //================================
            data = string.Format("Pin={0}\tCardNo={1}\tPassword={2}\tGroup={3}\tStartTime=\tEndTime=\tName={4}", _card, _pin, _password, _group,_name);
            if (data == "")
            {
                return 0;
            }
            else
            {
                if (IntPtr.Zero != h)
                {
                    ret = SetDeviceData(_hRFReader, devtablename, data, options);
                    ret = SetDeviceData(_hRFReader, "userauthorize", string.Format("Pin={0}\tAuthorizeTimezoneId=1\tAuthorizeDoorId=1", _card),"");
                    if (ret >= 0)
                    {
                        return ret;
                    }

                    return ret;
                }
            }
            return 0;
        }
//----------------------------------------------------------------------------------------------------------------------------
        public static int RFReaderDeleteCard(IntPtr _hRFReader, string _card, string _pin = "", string _password = "", string _group = "", string _options = "")
        {
            string[] sret = { "" };
            int ret = 0;
            //=================================
            string devtablename = "user";
            string options = _options;
            string data = "";
            IntPtr h = _hRFReader;
            //Pin=19999	CardNo=13375401	Password=1
            //Pin=19999\tCardNo=13375401\tPassword=1\r\nPin=2\tCardNo=14128058\tPassword=1
            //================================
            data = string.Format("Pin={0}\tCardNo={1}\tPassword={2}\tGroup={3}\tStartTime=\tEndTime=", _card, _pin, _password, _group);
            if (data == "")
            {
                return 0;
            }
            else
            {
                if (IntPtr.Zero != h)
                {
                    ret = DeleteDeviceData(_hRFReader, devtablename, data, options);
                    ret = DeleteDeviceData(_hRFReader, "userauthorize", string.Format("Pin={0}\tAuthorizeTimezoneId=1\tAuthorizeDoorId=1", _card), "");
                    
                    if (ret >= 0)
                    {
                        return ret;
                    }

                    return ret;
                }
            }
            return 0;
        }
//----------------------------------------------------------------------------------------------------------------------------
        public static string[] RFReaderReadTransection(IntPtr _hRFReader, string _cardnumber="",string _filter = "")
        {
            //=================================
            string devtablename = "transaction";
            string command = "Cardno\tPin\tVerified\tDoorID\tEventType\tInOutState\tTime_second";
            string datafilter = "";
            string options = "";
            //================================
            if(_cardnumber != "")
            {
                datafilter = string.Format("Cardno={0}", _cardnumber);
            }


            IntPtr h = IntPtr.Zero;
            int ret = 0;

            int BUFFERSIZE = 1 * 1024 * 1024;
            byte[] buffer = new byte[BUFFERSIZE];
            string[] ReadCard = new string[1024];

            h = _hRFReader;
            if (IntPtr.Zero != h)
            {
                ret = GetDeviceData(h, ref buffer[0], BUFFERSIZE, devtablename, command, datafilter, options);
            }


            if (ret >= 0)
            {
                string sReadCard = Encoding.Default.GetString(buffer);
                ReadCard = sReadCard.Split('\n');

                var list = new List<string>(ReadCard);
                list.RemoveAt(ReadCard.Length - 1);
                ReadCard = list.ToArray();
            }
            else
            {
                return ReadCard;
            }
            return ReadCard;
        }

//----------------------------------------------------------------------------------------------------------------------------
        [DllImport("plcommpro.dll", EntryPoint = "ControlDevice")]
        public static extern int ControlDevice(IntPtr h, int operationid, int param1, int param2, int param3, int param4, string options);

        /* Information for using this function 
         * OperationID [in] Operation contents: 1 for output, 2 for cancel alarm, 3 for restart device, 
         * and 4 for enable/disable normal open state.
         * 
         * Param2 (outputAddrType) [in]: When the OperationID is output operation, this parameter indicates the address
         * type of the output point (1 for door output, 2 for auxiliary output), for details, see Attached table
         * 3. When the OperationID is cancel alarm,, the parameter value is 0 by default. When the OperationID
         * value is 4, that is enable/disable normal open state, the parameter indicates is enable/disable 
         * normal open state (0 for disable, 1 for enable)
         */
//----------------------------------------------------------------------------------------------------------------------------

        public static bool RFControlOpenDoor(IntPtr _hRFReader,int _delayTime=3)
        {
            IntPtr h = _hRFReader;
            int operID = 1;
            int doorOrAuxoutID=1;      //หมายเลขประตู
            int outputAddrType=1;      //   
            int doorAction=_delayTime; //หน่วงเวลา

            ControlDevice(h, operID, doorOrAuxoutID, outputAddrType, doorAction, 0, "");
            return true;
        }

//----------------------------------------------------------------------------------------------------------------------------
        public static bool RFControlCloseDoor(IntPtr _hRFReader, int _delayTime = 3)
        {
            IntPtr h = _hRFReader;
            int operID = 1;
            int doorOrAuxoutID = 1;      //หมายเลขประตู
            int outputAddrType = 1;      //   
            int doorAction = _delayTime; //หน่วงเวลา

            ControlDevice(h, operID, doorOrAuxoutID, outputAddrType, doorAction, 0, "");
            return true;
        }

//----------------------------------------------------------------------------------------------------------------------------
        public static bool Testconnect(string s)
        {
            //string s = "protocol=TCP,ipaddress=192.168.1.209,port=4370,timeout=2000,passwd=";
            IntPtr i = Connect(s);
            int ret = PullLastError();
            return true;
        }
//----------------------------------------------------------------------------------------------------------------------------

        public static DateTime ConvertData2DateTime(string _data)
        {
            DateTime dt;
            string[] cdata = _data.Split(',');
            if (cdata.Length >= 6)
            {
                int stime = Convert.ToInt32(cdata[6]);
                dt = ConvertSecToDateTime(stime);
            }
            else return System.DateTime.MinValue;

            return dt;

        }
//----------------------------------------------------------------------------------------------------------------------------
        public static DateTime Convert2DateTime(Int64 _datetimedata)
        {
            DateTime dt;

            dt = ConvertSecToDateTime(_datetimedata);

            return dt;

        }
//----------------------------------------------------------------------------------------------------------------------------
        public static DateTime Convert2DateTime(string _datetimedata)
        {
            DateTime dt;
            Int64 data ;
            data = Convert.ToInt64(_datetimedata);
            dt = ConvertSecToDateTime(data);

            return dt;

        }
//----------------------------------------------------------------------------------------------------------------------------
        public static List<UserData> ConvertData2UserData(string[] _Data)
        {
            //Cardno\tPin\tVerified\tDoorID\tEventType\tInOutState\tTime_second"; ข้อมูลที่ได้ยังไม่ได้หาอัตโนมัติ
            List <UserData> udata;
            udata = new List<UserData>();

            string[] Data = _Data;
            for(int i=1;i<=Data.Length-1;i++)
            {
            UserData ud = new UserData();
                string[] TransData = Data[i].Split(',');
                ud.DoorId = TransData[3];
                ud.user = TransData[0];
                ud.InOutState = TransData[5];
                ud.DateTime = ConvertData2DateTime(Data[i]);
            udata.Add(ud);
            }

            return udata ;
        }
        public static DateTime ConvertSecToDateTime(Int64 _totalSec)
        {
            DateTime dt = new DateTime();
            Int64 totalsec = _totalSec;
            if (totalsec > 0)
            {
                Int64 s = totalsec % 60;
                Int64 m = (totalsec / 60) % 60;
                Int64 h = (totalsec / 3600) % 24;
                Int64 D = (totalsec / 86400) % 31 + 1;
                Int64 M = (totalsec / 2678400) % 12 + 1;
                Int64 Y = (totalsec / 32140800) + 2000;

                dt = DateTime.Parse(string.Format("{0}-{1}-{2} {3}:{4}", Y, M, D, h, m));

            }
            return dt;
        }
        public static string ConvertSecToDate(Int64 _totalSec)
        {
            
            Int64 totalsec = _totalSec;
            string dt="";
            if (totalsec > 0)
            {
                Int64 s = totalsec % 60;
                Int64 m = (totalsec / 60) % 60;
                Int64 h = (totalsec / 3600) % 24;
                Int64 D = (totalsec / 86400) % 31 + 1;
                Int64 M = (totalsec / 2678400) % 12 + 1;
                Int64 Y = (totalsec / 32140800) + 2000;

                dt = string.Format("{0}-{1}-{2}", Y, M, D);

            }
            return dt;
        }
//---------------------------------------------------------------------------------------------------------------------------
        [DllImport("plcommpro.dll", EntryPoint = "GetDeviceParam")]
        public static extern int GetDeviceParam(IntPtr h, ref byte buffer, int buffersize, string itemvalues);

        public static string GetSerialNumber(IntPtr _hRFReader)
        {
            IntPtr h = _hRFReader;

            if (IntPtr.Zero != h)
            {
                int ret = 0, i = 0;
                int BUFFERSIZE = 10 * 1024 * 1024;
                byte[] buffer = new byte[BUFFERSIZE];
                string str = null;
                string tmp = null;
                string[] value = null;

                str = "~SerialNumber";

                ret = GetDeviceParam(h, ref buffer[0], BUFFERSIZE, str);       //obtain device's param value
                

                if (ret >= 0)
                {
                    tmp = Encoding.Default.GetString(buffer);
                    value = tmp.Split('=');
                    tmp = value[1].Trim('\0');
                    value = tmp.Split(',');
                }
                return value[0];
            }
            return "";
        }
//-----------------------------------------------------------------------------------------
        [DllImport("plcommpro.dll", EntryPoint = "SetDeviceParam")]
        public static extern int SetDeviceParam(IntPtr h, string itemvalues);
//-----------------------------------------------------------------------------------------
        public static bool SyncTime(IntPtr _hRFReader,DateTime _datetime)
        {

            /*  DateTime= ((Year-2000)*12*31 + (Month -1)*31 + (Day-1))*(24*60*60) + Hour* 60 *60 + Minute*60 + Second;
                For example, the now datetime is 2010-10-26 20:54:55, so DateTime= 347748895;
            */

            if (_datetime == null)
            {
                _datetime = DateTime.Now;
               
            }
            _datetime = GetNetworkTime();
            int Year = 0, Month = 1, Day = 1, Hour = 0, Minute = 0, Second = 0;

            _datetime =  _datetime.AddHours(7);

            Year = _datetime.Year;
            Month = _datetime.Month;
            Day = _datetime.Day;
            Hour = _datetime.Hour;
            Minute = _datetime.Minute;
            Second = _datetime.Second;
            int dt = ((Year-2000)*12*31 + (Month -1)*31 + (Day-1))*(24*60*60) + Hour* 60 *60 + Minute*60 + Second;
            /*  int ret = 0;
                items = ("DeviceID=1,Door1SensorType=2,Door1Drivertime=6,Door1Intertime=3")
                ret = SetDeviceParam(h, items);
            */
            IntPtr h = _hRFReader;

            if (IntPtr.Zero != h)
            {
                string sDateTime = string.Format("DateTime={0}",dt);
                SetDeviceParam(_hRFReader, sDateTime);
                Disconnect(_hRFReader);
            }

            return true;
        }
//---------------------------------------------------------------------------------------------------------
        public static DateTime GetNetworkTime()
        {

                const string ntpServer = "time1.nimt.or.th";
                var ntpData = new byte[48];
                ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

                var addresses = Dns.GetHostEntry(ntpServer).AddressList;
                var ipEndPoint = new IPEndPoint(addresses[0], 123);
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();

                ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
                ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);
                return networkDateTime;
            }
            catch
            {
                return DateTime.Now;
            }

            
        }
    }
}
