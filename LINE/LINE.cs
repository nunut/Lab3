using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace VMS_SURE
{
    class LINE
    {
        public static bool LinePushMessage(string _msg)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
            var postData = string.Format("message={0}", _msg);
            //var postData = string.Format("message={0}&imageThumbnail={1}&imageFullsize={2},"สวัสดี","url_image_Thumbnail","url_image_full");
            var data = Encoding.UTF8.GetBytes(postData);
 
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            //string key = "RysJFpGt66ET662jmX1UzKP6lEESdj7wG8g3kothyr1xA71V+kxrewo91YxnmlIdaAWe7P7Qia7fuVFHCuocea1F/td6V6rKY0zP1X9C0Y2cYMoRA8uMDgImxZNpbfoeBZDpogggJm2eZTFcv+7C1gdB04t89/1O/w1cDnyilFU=";
            string key = "R3sXoQV5mWCd0yXE6IQFiQxVunJInyzXDDH2GT2v1BG";
            request.Headers.Add("Authorization", "Bearer "+key); // from user
            //request.Headers.Add("Authorization", "Bearer 82f226849a350c4a7fc06822513b1998"); // from charnal
            try
            {
            using (var stream = request.GetRequestStream())
            {
            stream.Write(data, 0, data.Length);
            }
           
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch
            {
            }
            return true;
        }
        public static bool LinePushMessageId(string _name,string _surname, string _linekey)
        {
            string msg = "VMS : ขณะนี้คุณ %name% %surname% อยู่ ณ ป้อม รปภ.เพื่อขอเข้าพบท่านค่ะ";
            if (DataStruct.Basic.LINEMsg != "")
                msg = DataStruct.Basic.LINEMsg;

            if (_name == "") _name = "(ไม่แจ้งข้อมุล)";
            msg = msg.Replace("%name%", _name);
            msg = msg.Replace("%surname%", _surname);
            //msg = string.Format(msg, _name, _surname, _linekey);

            LinePushMessageAlert(msg, _linekey);
            return true;

        }
        public static bool LinePushMessageAlert(string _msg, string _linekey)
        {
            //Fix bug 
            //System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12; 

            string linenotifyurl = "https://notify-api.line.me/api/notify";
            //linenotifyurl = "https://738nop577h.execute-api.us-east-1.amazonaws.com/CPN";


            var request = (HttpWebRequest)WebRequest.Create(linenotifyurl);


            var postData = string.Format("message={0}", _msg);
            //var postData = string.Format("message={0}&imageThumbnail={1}&imageFullsize={2},"สวัสดี","url_image_Thumbnail","url_image_full");
            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            //string key = "RysJFpGt66ET662jmX1UzKP6lEESdj7wG8g3kothyr1xA71V+kxrewo91YxnmlIdaAWe7P7Qia7fuVFHCuocea1F/td6V6rKY0zP1X9C0Y2cYMoRA8uMDgImxZNpbfoeBZDpogggJm2eZTFcv+7C1gdB04t89/1O/w1cDnyilFU=";
            //string key = "R3sXoQV5mWCd0yXE6IQFiQxVunJInyzXDDH2GT2v1BG";
            string key = _linekey;
            request.Headers.Add("Authorization", "Bearer " + key); // from user
            //request.Headers.Add("Authorization", "Bearer 82f226849a350c4a7fc06822513b1998"); // from charnal
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                request.Timeout = 500;
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch
            {
                //SURELog.LogErrorWrite("LPMA-79)");
            }
            return true;
        }
        public static bool LinePushMessageAt(string _msg)
        {
            
            var request = (HttpWebRequest)WebRequest.Create("https://api.line.me/v2/bot/message/push");
            var postData = string.Format("message={0}", _msg);

            var data = Encoding.UTF8.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            
            string key = "R3sXoQV5mWCd0yXE6IQFiQxVunJInyzXDDH2GT2v1BG";
            request.Headers.Add("Authorization", "Bearer " + key); // from user
            //82f226849a350c4a7fc06822513b1998 from charnal
            

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                request.Timeout = 500;
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch
            {
            }

            return true;
        }
    }
}
