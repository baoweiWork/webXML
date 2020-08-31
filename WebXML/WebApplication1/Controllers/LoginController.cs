using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// 检查域名有效性
        /// </summary>
        /// <param name="domain">域名，例如：qq.com、baidu.com</param>
        /// <returns>true 有效，false 无效</returns>
        public bool IsAvailableIP(string domain)
        {
            var IPs = Dns.GetHostAddresses(domain).Where(x => !IPAddress.IsLoopback(x)).ToList();
            if (IPs.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static void KKK()
        {
            //int[] arr = new int[16];
            //for (int i = 0; i < 16; i++)
            //{
            //    arr[i] = i + 1;
            //}

        }

    }
    public class UserList
    {
        public string Name { get; set; }
        public string Pwd { get; set; }

    }

   
}