using CommLib.BdPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Web;
using System.Collections;
using System.Data;
using Newtonsoft.Json;
using Google.ProtocolBuffers;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;
using com.igetui.api.openservice.payload;
using System.Threading;
using com.igetui.api.openservice.igetui.template.style;

namespace GETUIcsharp
{
    public class Struct
    {
        public string result { get; set; }
        public string contentId { get; set; }
    }

    class Program
    {
        //參數設置 <-----參數需要重新設置----->
        //http的域名
        //private static String HOST = "http://api.getui.com/apiex.htm";

        //https的域名
        //private static String HOST = "https://api.getui.com/apiex.htm";

        //定義常量, appId、appKey、masterSecret 採用本文檔 "第二步 獲取訪問憑證 "中獲得的應用配置
        //private static String APPID = "ZWVZShy9lQ9Ep1JCPoZgU9";
        //private static String APPKEY = "Rw7DIReuqy5gZ9Mz0jmm39";
        //private static String MASTERSECRET = "LUWaJWPp8m7HeNZVIXyan7";

        private static String APPID = "83UZl8yHf69E1mSMzSWdL5";
        private static String APPKEY = "J0Zgy7QQhq9IHviWUrGId";
        private static String MASTERSECRET = "DYCLie8PqU9cbYyKy9xMj1";

        //您獲取的clientID
        //private static String CLIENTID = "44215cb948f7ff34c447e53beaf6c097";//iphone 8+
        //private static String CLIENTID = "ddb5eab1f93b57b62a87bf5a15bda1e3";//iphone 8

        private static String CLIENTID = "1456126f5c75b8d5aaf1bda43551724f";//iphone 8

        //别名推送方式
        private static String ALIAS = "";

        //HOST：OpenService接口地址
        private static String HOST = "http://api.getui.com/apiex.htm";

        public static void Main(string[] args)
        {
            //toList接口每個用户狀態返回是否開啟，可選
            //Console.OutputEncoding = Encoding.GetEncoding(936);
            Environment.SetEnvironmentVariable("gexin_pushList_needDetails", "true");

            //全推
            //pushMessageToApp();

            //單推
            //PushMessageToSingle();

            //批量單推
            PushMessageToList();

            //bindAlias();//綁別名

            //setTag("3c7b7d64cd178ec0a299f2", "user"); //設標籤

            Console.ReadLine();// 使畫面停住
            Console.ReadKey();  //可按任意鍵結束畫面
        }

        ///綁別名
        public static void bindAlias()
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            String ret = push.bindAlias(APPID, ALIAS, CLIENTID);
            System.Console.WriteLine(ret);
        }

        /// <summary>
        /// 設標籤
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="GroupName"></param>
        public static void setTag(String clientID, String GroupName)
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            List<String> list = new List<String>();
            list.Add(GroupName);
            String ret = push.setClientTag(APPID, clientID, list);
            System.Console.WriteLine(ret);
        }

        //pushMessageToApp接口測試代碼
        private static void pushMessageToApp(string i)
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            // 定義"AppMessage"類型消息對象，設置消息内容模板、發送的目標App列表、是否支持離線發送、以及離線消息有效期(單位毫秒)
            AppMessage message = new AppMessage();

            //TransmissionTemplate template = TransmissionTemplateDemo();//透傳
            NotificationTemplate template = NotificationTemplateDemo();//通知

            message.IsOffline = true;                         // 用户當前不在線時，是否離線存儲,可選
            message.OfflineExpireTime = 1000 * 3600 * 12;     // 離線有效時間，單位為毫秒，可選
            message.Data = template;
            //判断是否客户端是否wifi環境下推送，2:4G/3G/2G,1為在WIFI環境下，0為無限制環境
            message.PushNetWorkType = 0; 
            //message.Speed = 1000;

            List<String> appIdList = new List<string>();
            appIdList.Add(APPID);

            List<String> phoneTypeList = new List<string>();   //通知接收者的手機操作系統類型
            phoneTypeList.Add("ANDROID");
            phoneTypeList.Add("IOS");

            List<String> provinceList = new List<string>();    //通知接收者所在省份
            //provinceList.Add("台灣");
            //provinceList.Add("上海");
            //provinceList.Add("北京");

            List<String> tagList = new List<string>();
            tagList.Add("user");

            message.AppIdList = appIdList;
            message.PhoneTypeList = phoneTypeList;
            message.ProvinceList = provinceList;
            message.TagList = tagList;


            String pushResult = push.pushMessageToApp(message);
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服務端返回结果：" + pushResult);
        }

        private static void PushMessageToSingle(string i)
        {

            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            //消息模板：
            NotificationTemplate template = NotificationTemplateDemo();
            //透傳模板：
            //TransmissionTemplate template = TransmissionTemplateDemo();

            // 單推消息模型
            SingleMessage message = new SingleMessage();
            message.IsOffline = true;                         // 用户當前不在線時，是否離線存儲儲,可選
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 離線有效时間，單位為毫秒，可選選
            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
            target.appId = APPID;
            target.clientId = CLIENTID;
            //target.alias = ALIAS;
            try
            {
                String pushResult = push.pushMessageToSingle(message, target);

                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
            }
            catch (RequestException e)
            {
                String requestId = e.RequestId;
                //发送失败后的重发
                String pushResult = push.pushMessageToSingle(message, target, requestId);
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("-----------------------------------------------");
                System.Console.WriteLine("----------------服务端返回结果：" + pushResult);
            }
        }

        //PushMessageToList接口測試代碼
        private static void PushMessageToList()
        {
            // 推送主類（方式1，不可與方式2共存）
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);
            // 推送主類（方式2，不可與方式1共存）此方式可通過獲取服務端地址列表判断最快域名後進行消息推送，每10分鐘檢查一次最快域名
            //IGtPush push = new IGtPush("",APPKEY,MASTERSECRET);
            ListMessage message = new ListMessage();

            //NotificationTemplate template = NotificationTemplateDemo("1");
            TransmissionTemplate template = TransmissionTemplateDemo();

            // 用户當前不在線时，是否離線存儲,可選
            message.IsOffline = true;
            // 離線有效时間，單位為毫秒，可選
            message.OfflineExpireTime = 1000 * 3600 * 12;
            message.Data = template;
            //message.PushNetWorkType = 0;        //判断是否客户端是否wifi環境下推送，1為在WIFI環境下，0為不限制網路環境。
            //設置接收者
            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();
            com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();
            target1.appId = APPID;
            target1.clientId = CLIENTID;
            target1.alias = ALIAS;

            //// 如需要，可以設置多個接收者
            //com.igetui.api.openservice.igetui.Target target2 = new com.igetui.api.openservice.igetui.Target();
            //target2.appId = APPID;
            //target2.clientId = CLIENTID2;
            ////target2.alias = ALIAS2;

            targetList.Add(target1);
            //targetList.Add(target2);

            String contentId = push.getContentId(message);
            String pushResult = push.pushMessageToList(contentId, targetList);
            Struct data = JsonConvert.DeserializeObject<Struct>(pushResult);

            if (data.result.Equals("ok"))
            {
                System.Console.WriteLine(data.result.ToString());
            }
            System.Console.WriteLine("-----------------------------------------------");
            System.Console.WriteLine("服务端返回结果:" + pushResult);
        }

        //通知透傳模板動作内容
        public static NotificationTemplate NotificationTemplateDemo()
        {
            DateTime dt = DateTime.Now;

            NotificationTemplate template = new NotificationTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;

            //通知欄標題
            template.Title = "個推測試" + dt.ToString();
            //通知欄内容     
            template.Text = "推播內容" + dt.ToString();
            //通知欄顯示本地圖片
            //template.Logo = "";
            //通知欄顯示網路圖標
            //template.LogoURL = "";
            //應用啟動類型，1：强制應用啟動  2：等待應用啟動
            template.TransmissionType = 1;
            //透傳内容  
            //template.TransmissionContent = "請填寫透傳内容"; //有需要可以加
            //接收到消息是否響鈴，true：響鈴 false：不響鈴   
            template.IsRing = true;
            //接收到消息是否震動，true：震動 false：不震動   
            template.IsVibrate = true;
            //接收到消息是否可清除，true：可清除 false：不可清除    
            template.IsClearable = true;

            return template;
        }

        //透傳模板動作内容
        public static TransmissionTemplate TransmissionTemplateDemo()
        {
            DateTime dt = DateTime.Now;

            DataTable dtable = new DataTable();
            dtable.Columns.Add("title");
            dtable.Columns.Add("content");
            dtable.Rows.Add("iOS測試"+ dt.ToString(), "測試的內容");


            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = APPID;
            template.AppKey = APPKEY;
            //應用啟動類型，1：强制應用啟動 2：等待應用啟動
            template.TransmissionType = 1;
            //透傳内容  
            template.TransmissionContent = JsonConvert.SerializeObject(dtable, Formatting.Indented); //"要傳的透傳內容" + dt.ToString();

            //iOS簡單推送
            //APNPayload apnpayload = new APNPayload();
            //SimpleAlertMsg alertMsg = new SimpleAlertMsg("測試看看");
            //apnpayload.AlertMsg = alertMsg;
            //apnpayload.Badge = 11;
            //apnpayload.ContentAvailable = 1;
            //apnpayload.Category = "";
            //apnpayload.Sound = "";
            //apnpayload.addCustomMsg("", "");
            //template.setAPNInfo(apnpayload);

            //iOS APN高级推送
            APNPayload apnpayload = new APNPayload();
            DictionaryAlertMsg alertMsg = new DictionaryAlertMsg();
            alertMsg.Body = "透傳內容" + dt.ToString();
            alertMsg.ActionLocKey = "ActionLocKey";
            alertMsg.LocKey = "LocKey";
            alertMsg.addLocArg("LocArg");
            alertMsg.LaunchImage = "LaunchImage";
            //iOS8.2支持字段
            alertMsg.Title = "透傳標題" + dt.ToString();
            alertMsg.TitleLocKey = "TitleLocKey";
            alertMsg.addTitleLocArg("TitleLocArg");

            apnpayload.AlertMsg = alertMsg;
            //apnpayload.Badge = 10;
            apnpayload.ContentAvailable = 1;
            //apnpayload.Category = "";
            //apnpayload.Sound = "test1.wav";
            apnpayload.addCustomMsg("payload", "payload");
            ////多媒體
            //MultiMedia multiMedia = new MultiMedia();
            //multiMedia.rid = "xxx-1";
            //multiMedia.url = "";
            //multiMedia.setIsOnlyWifi(false);
            //multiMedia.type = MultiMedia.MediaType.pic;

            //List<MultiMedia> list = new List<MultiMedia>();
            //list.Add(multiMedias);
            //apnpayload.MultiMedias = list;

            template.setAPNInfo(apnpayload);

            return template;
        }
        
    }
}
