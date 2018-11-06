using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace RestAPI.Controllers
{
    public class CalendarController : ApiController
    {
        /// <summary>
        /// 특일정보 공휴일 조회 서비스
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>//01,02,03....11,12의 형태를 갖는다.
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetHoliday")]
        public IHttpActionResult GetHoliday(string year,string month)
        {
            //RestAPI 호출
            WebRequest request = null;
            string serviceKey = ConfigurationSettings.AppSettings["holidayServiceKey"];

            request = WebRequest.Create("http://apis.data.go.kr/B090041/openapi/service/SpcdeInfoService/getRestDeInfo?solYear=" + year + "&solMonth=" + month + "&ServiceKey=" + serviceKey);

            //RestAPI 응답 메시지 
            Stream dataStream = null;

            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var result = reader.ReadToEnd();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            //xml -> json 변환
            string jsonresult = JsonConvert.SerializeXmlNode(xml);

            reader.Close();
            dataStream.Close();
            response.Close();

            return Json(jsonresult);
        }

        [HttpPost]
        [ActionName("HolidayTest")]
        public IHttpActionResult HolidayTest(string holi)
        {

            //RestAPI 호출
            WebRequest request = null;
            string serviceKey = ConfigurationSettings.AppSettings["holidayServiceKey"];
            string year = DateTime.Now.ToString("yyyy");
            string[] month = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

            foreach (var m in month)
            {
                request = WebRequest.Create("http://apis.data.go.kr/B090041/openapi/service/SpcdeInfoService/getRestDeInfo?solYear=" + year + "&solMonth=" + month + "&ServiceKey=" + serviceKey);

                //RestAPI 응답 메시지 
                Stream dataStream = null;

                var response = request.GetResponse();
                dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                var result = reader.ReadToEnd();

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);

                reader.Close();
                dataStream.Close();
                response.Close();
            }

            return Json("");
        }
    }
}
