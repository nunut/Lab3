using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zkemkeeper;

namespace VMS_SURE.Accesscontrol
{
    class RFIDAccessZKStandAlone
    {
        public class RFReader
        {
            public string Protocal = "TCP";
            public string IPaddress = null;
            public string PortNumber = null;
            public string Name = null;
            public string User = null;
            public string Password = null;
            public int Timeout = 2000;
            public bool Status = false;
            public int DeviceId = 1;
        }

        public static RFReader RFReaderInit(string _ipAddress = "192.168.1.100", string _passWord = "", string _portNumber = "4370", int _timeOut = 2000)
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
                
                return rfReader;
            }
        }
//--------------------------------------------------------------------------------------------------------------
        public static bool RFReaderConnect(RFReader _reader)
        {
            RFReader Reader = new RFReader();
            Reader = _reader;
            int Port = Convert.ToInt16(Reader.PortNumber);
            CZKEM device = new CZKEM();
            if (device.Connect_Net(Reader.IPaddress, Port))
            {
                device.EnableDevice(10, true);
                device.set_STR_CardNumber(10, "0006085334");
                device.SSR_SetUserInfo(10, "10", "TEST", "", 1, false);
            }


            
            return true;
        }
//--------------------------------------------------------------------------------------------------------------
        public static bool RFReaderConnect2(RFReader _reader)
        {


            zkemkeeper.CZKEM ax = new zkemkeeper.CZKEM();

            bool t = ax.Connect_Net("192.168.1.130", 4370);
            if(!t)
            t = ax.Connect_Net("192.168.1.130", 4372);
            ax.EnableDevice(10, true);
            ax.set_STR_CardNumber(10, "7136855");
            ax.SSR_SetUserInfo(10, "7136855", "TEST", "", 1, true);

            return true;
            
            //ax.Connect_Net(reader.IPaddress, Convert.ToInt16(reader.PortNumber));
           /*
            Axzkemkeeper.AxCZKEM ax = new Axzkemkeeper.AxCZKEM();
            bool t = ax.Connect_Net("192.168.1.220", 4370);
            ax.EnableDevice(10, true);
            ax.set_STR_CardNumber(10, "6085334");
            ax.SSR_SetUserInfo(10, "6085334", "TEST", "", 1, true);
            * */
        }
    }
}
