using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
       
        public static string XmlPath = "C:/Users/Administrator/source/repos/WebXML/WebApplication1/XmlFile/XmlList.xml";
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult  XmlList(UserList user)
        {
            //创建添加
            XmlDocument xmlDoc = new XmlDocument();
            //根据地址判定有无XML文件
            if (!System.IO.File.Exists(XmlPath))
            {
                //加入XML的声明段落 
                XmlNode xmlnode = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlnode);
                //加入一个根元素 
                XmlElement xmlelem = xmlDoc.CreateElement("UserList");
                XmlText xmltext = xmlDoc.CreateTextNode("");
                xmlelem.AppendChild(xmltext);
                xmlDoc.AppendChild(xmlelem);
                //加入一个子元素 
                XmlElement xmlelem1 = xmlDoc.CreateElement("User");
                xmltext = xmlDoc.CreateTextNode("");
                xmlelem1.AppendChild(xmltext);
                //为子元素增加两个属性 
                xmlelem1.SetAttribute("UserName", user.Name);
                xmlelem1.SetAttribute("UserPsd", user.Pwd);
                xmlelem1.SetAttribute("登录次数", "1");
                xmlDoc.ChildNodes.Item(1).AppendChild(xmlelem1);
                xmlDoc.Save(XmlPath);
            }
            else
            {//再根元素下添加子元素 
                XDocument xdocument = XDocument.Load(XmlPath); //加载xml文件
                XElement UserUpdate = xdocument.Element("UserList").Elements().Where(x
                    =>x.Attribute("UserName").Value== user.Name).FirstOrDefault();
                //查看是否存在对应子节点
                if (UserUpdate != null)
                {
                    UserUpdate.Attribute("登录次数").Value = (Convert.ToInt32(UserUpdate.Attribute("登录次数").Value) + 1).ToString();
                    xdocument.Save(XmlPath);
                }
                else {
                    //添加节点
                    xmlDoc.Load(XmlPath);
                    XmlNode rootXml = xmlDoc.SelectSingleNode("UserList");
                    XmlElement element = xmlDoc.CreateElement("User");
                    element.SetAttribute("UserName", user.Name);
                    element.SetAttribute("UserPsd", user.Pwd);
                    element.SetAttribute("登录次数", "1");
                    rootXml.AppendChild(element);
                    xmlDoc.Save(XmlPath);
                }
            }
            XDocument xd = XDocument.Load(XmlPath); //加载xml文件
            XElement loginN = xd.Element("UserList").Elements().Where(x
                => x.Attribute("UserName").Value == user.Name).FirstOrDefault();
            string name= loginN.Attribute("UserName").Value;
            string num= loginN.Attribute("登录次数").Value;
            return Json(new{ UserName=name, LoginNum=num });
        }
        public JsonResult Login(string Name, string Pwd)
        {
            string url = @"http://localhost:1122/api/Login/LoginVerification";
            string context = "{\"sign\"=1,\"userName\"=1,\"passWord\"=1}";
            string result = HttpApi(url, context);
            return Json(new { UserName = "", LoginNum = "" });
        }
        /// <summary>
        /// 调用api返回json
        /// </summary>
        /// <param name="url">api地址</param>
        /// <param name="context">接收参数</param>
        /// <returns></returns>
        public string HttpApi(string uri, string context)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Proxy = null;
            request.Method = "POST";

            //request.Accept = "*/*";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "text/html;charset=gbk";
            byte[] buffer = Encoding.Default.GetBytes(context);
            request.ContentLength = buffer.Length;
            //WebRequest.GetSystemWebProxy();
            request.GetRequestStream().Write(buffer, 0, buffer.Length);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null) return null;
            StreamReader sr = new StreamReader(response.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

    }
    public class UserList
    {
        public string Name { get; set; }
        public string Pwd { get; set; }

    }
   

}