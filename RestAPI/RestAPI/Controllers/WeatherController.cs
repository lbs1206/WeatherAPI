using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Configuration;
using System.Xml;
using RestAPI.Models;
using RestAPI.Common;
using Newtonsoft.Json;

namespace RestAPI.Controllers
{
    public class WeatherController : ApiController
    {
        //미세먼지 api Sample (데이터 가공안한 API Sample)
        #region 미세먼지
        /// <summary>
        /// 미세먼지API 호출
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetFineDust")]
        public IHttpActionResult GetFineDust(string sido)
        {
            string result = string.Empty;
            AirFineDustModel afd = new AirFineDustModel();

            try
            {
                //RestAPI 데이터 요청
                WebRequest request = null;
                string serviceKey = ConfigurationSettings.AppSettings["airServiceKey"];

                //Api 호출 sido : 시도명 (경기,충남,충북,전남,전북,울산,대전,대구,부산,인천,서울,강원)
                request = WebRequest.Create("http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getCtprvnRltmMesureDnsty?" +
                    "serviceKey=" + serviceKey + "&numOfRows=1000&pageSize=10&pageNo=1&startPage=1&sidoName=" + sido + "&ver=1.3");

                //데이터 읽어오기
                Stream dataStream = null;

                //API 호출 값을 문자열로 받아오기
                var response = request.GetResponse();
                dataStream = response.GetResponseStream();
                var reader = new StreamReader(dataStream);
                result = reader.ReadToEnd();

                //xml -> json 가공
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(result);
                afd = AirXmlToJson(xml);

                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception");
            }


            return Json(afd);
        }

        /// <summary>
        /// 미세먼지 xml 파일 json형태로 변환
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public AirFineDustModel AirXmlToJson(XmlDocument xml)
        {
            //미세먼지 모델 
            AirFineDustModel afd = new AirFineDustModel();

            XmlNodeList nodes = xml.SelectNodes("/response/body/items/item");

            List<AirFineDust> list = new List<AirFineDust>();

            //xml 값 List<AirFineDust> list에 값 넣기
            for (int i = 0; i < nodes.Count; i++)
            {
                AirFineDust item = new AirFineDust();

                item.stationname = nodes.Item(i)["stationName"].InnerText;
                item.mangname = nodes.Item(i)["mangName"].InnerText;
                item.datatime = nodes.Item(i)["dataTime"].InnerText;
                item.so2value = nodes.Item(i)["so2Value"].InnerText;
                item.covalue = nodes.Item(i)["coValue"].InnerText;
                item.o3value = nodes.Item(i)["o3Value"].InnerText;
                item.no2value = nodes.Item(i)["no2Value"].InnerText;
                item.pm10value = nodes.Item(i)["pm10Value"].InnerText;
                item.pm10value24 = nodes.Item(i)["pm10Value24"].InnerText;
                item.pm25value = nodes.Item(i)["pm25Value"].InnerText;
                item.pm25value24 = nodes.Item(i)["pm25Value24"].InnerText;
                item.khaivalue = nodes.Item(i)["khaiValue"].InnerText;
                item.khaigrade = nodes.Item(i)["khaiGrade"].InnerText;
                item.so2grade = nodes.Item(i)["so2Grade"].InnerText;
                item.cograde = nodes.Item(i)["coGrade"].InnerText;
                item.o3grade = nodes.Item(i)["o3Grade"].InnerText;
                item.no2grade = nodes.Item(i)["no2Grade"].InnerText;
                item.pm10grade = nodes.Item(i)["pm10Grade"].InnerText;
                item.pm25grade = nodes.Item(i)["pm25Grade"].InnerText;
                item.pm10grade1h = nodes.Item(i)["pm10Grade1h"].InnerText;
                item.pm25grade1h = nodes.Item(i)["pm25Grade1h"].InnerText;

                //json.items.item.Add(item);
                list.Add(item);

            }
            afd.item = list;
            return afd;
        }
        #endregion //미세먼지api sample

        //좌표 날씨 api Sample (데이터 가공안한 API Sample)
        #region 위도경도 날씨
        /// <summary>
        /// 동네예보날씨 호출
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetSpaceWeather")]
        public IHttpActionResult GetSpaceWeather(string x, string y)
        {
            string result = string.Empty;
            WebRequest request = null;
            string serviceKey = ConfigurationSettings.AppSettings["spaceWeatherServiceKey"];
            string date = DateTime.Now.ToString("yyyyMMdd");//오늘날짜
            string time = "0230";//예보시간

            //위경도 그리드좌표로 변환
            GridXY xy = new GridXY();
            GridXYModel gridxy = xy.change(Double.Parse(x), Double.Parse(y));

            //api요청 base_date : 예보날짜, base_time : 예보시간, nx : 격자좌표X , ny : 격자좌표 Y, numOfRows : 출력 수
            request = WebRequest.Create("http://newsky2.kma.go.kr/service/SecndSrtpdFrcstInfoService2/ForecastSpaceData?" +
                "serviceKey=" + serviceKey + "&base_date=" + date + "&base_time=" + time + "&nx=" + gridxy.gridx + "&ny=" + gridxy.gridy + "&numOfRows=400");

            Stream dataStream = null;

            //요청한 데이터 string으로 받아오기
            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            result = reader.ReadToEnd();

            //xml데이터 json 형태로 가공
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);
            SpaceWeatherModel swm = SpaceWeatherXmlToJson(xml);


            reader.Close();
            dataStream.Close();
            response.Close();

            return Json(swm);
        }

        /// <summary>
        /// 동네예보xml파일 json 으로 변환
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public SpaceWeatherModel SpaceWeatherXmlToJson(XmlDocument xml)
        {
            SpaceWeatherModel sw = new SpaceWeatherModel();


            XmlNodeList nodes = xml.SelectNodes("/response/body/items/item");

            List<SpaceWeather> list = new List<SpaceWeather>();

            for (int i = 0; i < nodes.Count; i++)
            {
                SpaceWeather item = new SpaceWeather();

                item.baseDate = nodes.Item(i)["baseDate"].InnerText;
                item.baseTime = nodes.Item(i)["baseTime"].InnerText;
                //WeatherCategoryChange() => category 코드 한글로 변환
                item.category = WeatherCategoryChange(nodes.Item(i)["category"].InnerText);
                item.fcstDate = nodes.Item(i)["fcstDate"].InnerText;
                item.fcstTime = nodes.Item(i)["fcstTime"].InnerText;
                item.fcstValue = nodes.Item(i)["fcstValue"].InnerText;
                item.nx = nodes.Item(i)["nx"].InnerText;
                item.ny = nodes.Item(i)["ny"].InnerText;

                //아침 최저기온과 최고기온 낮 최고 기온인 값만 출력
                if (item.category.Equals("아침 최저기온") || item.category.Equals("낮 최고기온"))
                {
                    list.Add(item);
                }
                //오전 6시 하늘상태와 오후 3시 하늘상태의 값 출력
                if (item.fcstTime.Equals("1500") || item.fcstTime.Equals("0600"))
                {
                    if (item.category.Equals("하늘상태"))
                    {
                        item.fcstValue = SKY(item.fcstValue);
                        list.Add(item);
                    }
                }
                //위 if 문들 삭제하시면 모든 값들이 출력 됩니다. 강수확률은 item.category.Equals("POP")입니다.
            }
            sw.item = list;
            return sw;
        }

        /// <summary>
        /// 하늘상태 코드 => 한글로 변환
        /// </summary>
        /// <param name="sky"></param>
        /// <returns></returns>
        public string SKY(string sky)
        {
            string result = string.Empty;
            int temp = Int16.Parse(sky);

            if (0 <= temp && temp <= 2) { result = "맑음"; }
            else if (3 <= temp && temp <= 5) { result = "구름 조금"; }
            else if (6 <= temp && temp <= 8) { result = "구름 많음"; }
            else if (9 <= temp && temp <= 10) { result = "흐림"; }

            return result;
        }


        /// <summary>
        /// 날씨카테고리코드 한글로 변환
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public string WeatherCategoryChange(string category)
        {
            string result = string.Empty;

            switch (category)
            {
                case "POP": return "강수확률";
                case "PTY": return "강수형태";
                case "R06": return "6시간 강수량";
                case "REH": return "습도";
                case "S06": return "6시간 신적설";
                case "SKY": return "하늘상태";
                case "T3H": return "3시간 기온";
                case "TMN": return "아침 최저기온";
                case "TMX": return "낮 최고기온";
                case "UUU": return "(동서)풍속";
                case "VVV": return "(남북)풍속";
                case "WAV": return "파고";
                case "VEC": return "풍향";
                case "WSD": return "풍속";
            }

            return result;
        }
        #endregion

        //현위치 좌표로 날씨 미세먼지 출력 API (Views > index.cshtml에서 테스트가능)
        #region 현위치

        [HttpPost]
        [ActionName("GetLocationWeather")]
        public IHttpActionResult GetLocationWeather(string x, string y)
        {
            LocationWeatherModel lwm = new LocationWeatherModel();

            string result = string.Empty;

            //위경도 그리드좌표로 변환
            GridXY xy = new GridXY();
            GridXYModel gridxy = xy.change(Double.Parse(x), Double.Parse(y));

            //좌표 -> 측정소 주소(grid.json 위치는 다운받은 grid.json 파일 위치로 변경해주셔야합니다.)
            StreamReader r = new StreamReader("D:\\OpenAPi\\RestAPI\\RestAPI\\Common\\grid.json");
            //grid.json값을 string json 으로 값넘기기
            string json = r.ReadToEnd();

            //json값 리스트 형태로 값 넘기기
            List<gridJson> items = JsonConvert.DeserializeObject<List<gridJson>>(json);

            //그리드 좌표값을 통해 주소와 sido ,gugun 값을 가져옵니다.(Common > grid.json파일 참조)
            List<gridJson> temp = items.Where(order => order.gridX == gridxy.gridx.ToString()).
                                        Where(order => order.gridY == gridxy.gridy.ToString()).Select(z => z).ToList();

            string depth1 = temp[0].sido; //시도
            string depth2 = temp[0].gugun; //구군
            //
            
            //날씨 API 호출하여 xml 값 받기
            XmlDocument xml1 = GetWeather(gridxy.gridx.ToString(), gridxy.gridy.ToString());
            //미세먼지 API 호출하여 xml 값 받기
            XmlDocument xml2 = GetAirDust(depth1);

            //받아온 xml들을 json 형태로 변환
            lwm = XmlsToJson(xml1, xml2,depth2);

            return Json(lwm);
        }




        #endregion

        //주소로 날씨 미세먼지 출력 API (Views > index.cshtml에서 테스트가능)
        #region 지역
        [HttpPost]
        [ActionName("GetRegionWeather")]
        public IHttpActionResult GetRegionWeather(string sido, string gugun)
        {
            LocationWeatherModel lwm = new LocationWeatherModel();
            
            //grid.json 파일 읽어오기(grid.json 파일 경로 다시 맞게 변경해주셔야 합니다.)
            StreamReader r = new StreamReader("D:\\OpenAPi\\RestAPI\\RestAPI\\Common\\grid.json");
            
            //grid.json 파일 문자열로 변환
            string json = r.ReadToEnd();

            //grid.json 문자열 리스트 형태로 변환
            List<gridJson> items = JsonConvert.DeserializeObject<List<gridJson>>(json);

            //주소를 통해 격자 좌표와 시도, 구군 값 가져오기
            List<gridJson> temp = items.Where(order => order.depth1 == sido).Where(order => order.depth2 == gugun).Select(x => x).ToList();

            string gridx = temp[0].gridX;//격자 좌표 X
            string gridy = temp[0].gridY;//격자 좌표 Y
            string depth1 = temp[0].sido;//시도
            string depth2 = temp[0].gugun;//구군

            //날씨API 호출하여 xml 값 받아오기
            XmlDocument xml1 = GetWeather(gridx, gridy);
            //미세먼지API 호출하여 xml 값 받아오기
            XmlDocument xml2 = GetAirDust(depth1);

            //xml들을 Json 형태로 변환
            lwm = XmlsToJson(xml1, xml2,depth2);

            return Json(lwm);
        }
        #endregion

        #region 중기예보 날씨 API
        #region 중기 하늘상태 예보 API
        /// <summary>
        /// 2일~10일 후 하늘상태 예보 API
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetMiddleLandWeather")]
        public IHttpActionResult GetMiddleLandWeather(string sido)
        {
            string result = string.Empty;

            WebRequest request = null;

            string serviceKey = ConfigurationSettings.AppSettings["spaceWeatherServiceKey"];
            string date = DateTime.Now.ToString("yyyyMMdd"); //오늘 날짜
            date = date + "0600"; //예보시간은 최근 24시간 0600,1800 일 2회만 발표합니다.
            string regId = LandChange(sido);//지역명을 지역코드로 변환

            //API 호출 regId : 지역코드(LandChange 메소드를 통해 지역명을 지역코드로 변환), tmFc : 예보 발표시간 (오전 6시와 오후 6시에 발표)
            request = WebRequest.Create("http://newsky2.kma.go.kr/service/MiddleFrcstInfoService/getMiddleLandWeather?" +
                "serviceKey=" + serviceKey + "&regId=" + regId + "&tmFc=" + date + "&numOfRows=400");

            Stream dataStream = null;

            //요청한 데이터 문자열로 받아오기
            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            result = reader.ReadToEnd();

            //xml데이터 
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            reader.Close();
            dataStream.Close();
            response.Close();

            result = JsonConvert.SerializeXmlNode(xml);
            //xml 설명
            //wf3Am 3일후 오전날씨예보  wf3Pm 3일후 오후날씨예보 wf4Am 4일후 오전날씨예보  
            //wf4Pm 4일후 오후날씨예보  wf5Am 5일후 오전날씨예보 wf5Pm 5일후 오후날씨예보 
            //wf6Am 6일후 오전날씨예보  wf6Pm 6일후 오후날씨예보 wf7Am 7일후 오전날씨예보
            //wf7Pm 7일후 오후날씨예보  wf8 8일후 날씨예보  wf9 9일후 날씨예보  wf10 10일후 날씨예보
            return Json(result);
        }

        public string LandChange(string sido)
        {
            string result = string.Empty;

            switch (sido)
            {
                case "서울":
                case "인천":
                case "경기도": result = "11B00000"; break;
                case "강원도영서": result = "11D10000"; break;
                case "강원도영동": result = "11D20000"; break;
                case "대전":
                case "세종":
                case "충청남도": result = "11C20000"; break;
                case "충청북도": result = "11C10000"; break;
                case "광주":
                case "전라남도": result = "11F20000"; break;
                case "전라북도": result = "11F10000"; break;
                case "대구": 
                case "경상북도": result = "11H10000"; break;
                case "부산":
                case "울산":
                case "경상남도": result = "11H20000"; break;
                case "제주도": result = "11G0000"; break;
            }
            

            return result;
        }
        #endregion

        #region 중기 기온 예보 API
        /// <summary>
        /// 2일~10일 후 기온 예보 API
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetMiddleTemperature")]
        public IHttpActionResult GetMiddleTemperature(string sido)
        {
            string result = string.Empty;

            WebRequest request = null;

            string serviceKey = ConfigurationSettings.AppSettings["spaceWeatherServiceKey"];
            string date = DateTime.Now.ToString("yyyyMMdd"); //오늘 날짜
            date = date + "0600"; //예보시간은 최근 24시간 0600,1800 일 2회만 발표합니다.
            string regId = TemplateLandChange(sido);//지역명을 지역코드로 변환

            //API 호출 regId : 지역코드(LandChange 메소드를 통해 지역명을 지역코드로 변환), tmFc : 예보 발표시간 (오전 6시와 오후 6시에 발표)
            request = WebRequest.Create("http://newsky2.kma.go.kr/service/MiddleFrcstInfoService/getMiddleTemperature?" +
                "serviceKey=" + serviceKey + "&regId=" + regId + "&tmFc=" + date + "&numOfRows=400");

            Stream dataStream = null;

            //요청한 데이터 문자열로 받아오기
            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            result = reader.ReadToEnd();

            //xml데이터 
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            reader.Close();
            dataStream.Close();
            response.Close();

            result = JsonConvert.SerializeXmlNode(xml);
            //xml 설명
            //tamin3 3일후 예상최저기온(℃) tamax3 3일후 예상최고기온(℃) 
            //tamin4 4일후 예상최저기온(℃) tamax4 4일후 예상최고기온(℃) 
            //tamin5 5일후 예상최저기온(℃) tamax5 5일후 예상최고기온(℃)
            //tamin6 6일후 예상최저기온(℃) tamax6 6일후 예상최고기온(℃)
            //tamin7 7일후 예상최저기온(℃) tamax7 7일후 예상최고기온(℃) 
            //tamin8 8일후 예상최저기온(℃) tamax8 8일후 예상최고기온(℃) 
            //tamin9 9일후 예상최저기온(℃) tamax9 9일후 예상최고기온(℃) 
            //tamin10 10일후 예상최저기온(℃) tamax10 10일후 예상최고기온(℃)
            return Json(result);
        }

        public string TemplateLandChange(string sido)
        {
            string result = string.Empty;

            switch (sido)
            {
                case "서울": return "11B10101";
                case "서귀포": return "11G00401";
                case "인천": return "11B20201"; 
                case "광주": return "11F20501"; 
                case "수원": return "11B20601"; 
                case "목포": return "21F20801"; 
                case "파주": return "11B20305"; 
                case "여수": return "11F20401"; 
                case "춘천": return "11D10301"; 
                case "전주": return "11F10201"; 
                case "원주": return "11D10401"; 
                case "군산": return "21F10501"; 
                case "강릉": return "11D20501"; 
                case "부산": return "11H20201"; 
                case "대전": return "11C20401"; 
                case "울산": return "11H20101"; 
                case "서산": return "11C20101"; 
                case "창원": return "11H20301"; 
                case "세종": return "11C20404"; 
                case "대구": return "11H10701"; 
                case "청주": return "11C10301"; 
                case "안동": return "11H10501"; 
                case "제주": return "11G00201";
                case "포항": return "11H10201"; 
            }

            return result;
        }
        #endregion
        #endregion

        //미세먼지, 날씨 API -> XML
        #region ApiToXml

        /// <summary>
        /// 미세먼지 API에서 XML 받아오기
        /// </summary>
        /// <param name="sido"></param>
        /// <returns></returns>
        public XmlDocument GetAirDust(string sido)
        {
            string result = string.Empty;
            WebRequest request = null;

            string serviceKey = ConfigurationSettings.AppSettings["spaceWeatherServiceKey"];

            //API 호출
            request = WebRequest.Create("http://openapi.airkorea.or.kr/openapi/services/rest/ArpltnInforInqireSvc/getCtprvnRltmMesureDnsty?" +
                "serviceKey=" + serviceKey + "&numOfRows=1000&pageSize=10&pageNo=1&startPage=1&sidoName=" + sido + "&ver=1.3");

            Stream dataStream = null;

            //요청한 데이터 문자열로 받아오기
            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            result = reader.ReadToEnd();

            //xml데이터 
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            reader.Close();
            dataStream.Close();
            response.Close();

            return xml;
        }

        /// <summary>
        /// 날씨  API 에서 xml 값 받아오기
        /// </summary>
        /// <param name="gridx"></param>
        /// <param name="gridy"></param>
        /// <returns></returns>
        public XmlDocument GetWeather(string gridx, string gridy)
        {
            string result = string.Empty;
            WebRequest request = null;

            string serviceKey = ConfigurationSettings.AppSettings["spaceWeatherServiceKey"];
            string date = DateTime.Now.ToString("yyyyMMdd"); //오늘 날짜
            string time = "0230"; //예보시간

            //API 호출
            request = WebRequest.Create("http://newsky2.kma.go.kr/service/SecndSrtpdFrcstInfoService2/ForecastSpaceData?" +
                "serviceKey=" + serviceKey + "&base_date=" + date + "&base_time=" + time + "&nx=" + gridx + "&ny=" + gridy + "&numOfRows=400");

            Stream dataStream = null;

            //요청한 데이터 문자열로 받아오기
            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            result = reader.ReadToEnd();

            //xml데이터 
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            reader.Close();
            dataStream.Close();
            response.Close();

            return xml;
        }
        #endregion

        //미세먼지, 날씨 XML -> Json
        #region XmlToJson
        /// <summary>
        /// 날씨 미세먼지 xml 을 json 하나로 통합하여 출력
        /// </summary>
        /// <param name="xml1"></param>
        /// <param name="xml2"></param>
        /// <returns></returns>
        public LocationWeatherModel XmlsToJson(XmlDocument xml1, XmlDocument xml2,string gugun)
        {
            LocationWeatherModel lwm = new LocationWeatherModel();

            XmlNodeList nodes1 = xml1.SelectNodes("/response/body/items/item");
            XmlNodeList nodes2 = xml2.SelectNodes("/response/body/items/item");

            List<LocationWeather> list = new List<LocationWeather>();
            LocationWeather item = new LocationWeather();

            string today = DateTime.Now.ToString("yyyyMMdd"); //오늘 날짜
            string tomorrow = DateTime.Now.AddDays(1).ToString("yyyyMMdd"); // 내일 날짜

            //날씨 데이터 가공
            for (int i = 0; i < nodes1.Count; i++)
            {
                //오늘
                if (nodes1.Item(i)["category"].InnerText.Equals("TMN") && nodes1.Item(i)["fcstDate"].InnerText.Equals(today))
                {
                    item.morning1 = nodes1.Item(i)["fcstValue"].InnerText;//아침 최저기온
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("SKY") && nodes1.Item(i)["fcstDate"].InnerText.Equals(today) && nodes1.Item(i)["fcstTime"].InnerText.Equals("0600"))
                {
                    item.mSky1 = SKY(nodes1.Item(i)["fcstValue"].InnerText);//아침 하늘상태
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("POP") && nodes1.Item(i)["fcstDate"].InnerText.Equals(today) && nodes1.Item(i)["fcstTime"].InnerText.Equals("0600"))
                {
                    item.mRain1 = nodes1.Item(i)["fcstValue"].InnerText;//아침 강수확률
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("TMX") && nodes1.Item(i)["fcstDate"].InnerText.Equals(today))
                {
                    item.dayTime1 = nodes1.Item(i)["fcstValue"].InnerText;//낮 최고기온
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("SKY") && nodes1.Item(i)["fcstDate"].InnerText.Equals(today) && nodes1.Item(i)["fcstTime"].InnerText.Equals("1500"))
                {
                    item.dSky1 = SKY(nodes1.Item(i)["fcstValue"].InnerText);//낮 하늘상태
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("POP") && nodes1.Item(i)["fcstDate"].InnerText.Equals(today) && nodes1.Item(i)["fcstTime"].InnerText.Equals("1500"))
                {
                    item.dRain1 = nodes1.Item(i)["fcstValue"].InnerText;//낮 강수확률
                }

                //내일
                if (nodes1.Item(i)["category"].InnerText.Equals("TMN") && nodes1.Item(i)["fcstDate"].InnerText.Equals(tomorrow))
                {
                    item.morning2 = nodes1.Item(i)["fcstValue"].InnerText;//아침 최저기온
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("SKY") && nodes1.Item(i)["fcstDate"].InnerText.Equals(tomorrow) && nodes1.Item(i)["fcstTime"].InnerText.Equals("0600"))
                {
                    item.mSky2 = SKY(nodes1.Item(i)["fcstValue"].InnerText);//아침 하늘상태
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("POP") && nodes1.Item(i)["fcstDate"].InnerText.Equals(tomorrow) && nodes1.Item(i)["fcstTime"].InnerText.Equals("0600"))
                {
                    item.mRain2 = nodes1.Item(i)["fcstValue"].InnerText;//아침 비올확률
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("TMX") && nodes1.Item(i)["fcstDate"].InnerText.Equals(tomorrow))
                {
                    item.dayTime2 = nodes1.Item(i)["fcstValue"].InnerText;//낮 최고기온
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("SKY") && nodes1.Item(i)["fcstDate"].InnerText.Equals(tomorrow) && nodes1.Item(i)["fcstTime"].InnerText.Equals("1500"))
                {
                    item.dSky2 = SKY(nodes1.Item(i)["fcstValue"].InnerText);//낮 하늘상태
                }
                if (nodes1.Item(i)["category"].InnerText.Equals("POP") && nodes1.Item(i)["fcstDate"].InnerText.Equals(tomorrow) && nodes1.Item(i)["fcstTime"].InnerText.Equals("1500"))
                {
                    item.dRain2 = nodes1.Item(i)["fcstValue"].InnerText;//낮 강수확률
                }
            }

            //미세먼지 데이터 가공 
            for (int i = 0; i < nodes2.Count; i++)
            {
                if (nodes2.Item(i)["stationName"].InnerText.Equals(gugun))
                {
                    item.pm10 = nodes2.Item(i)["pm10Value"].InnerText;//미세먼지 농도
                    item.pm25 = nodes2.Item(i)["pm25Value"].InnerText;//초미세먼지 농도
                    item.pm10Grade = GradeToString(nodes2.Item(i)["pm10Grade"].InnerText);//미세먼지 지수
                    item.pm25Grade = GradeToString(nodes2.Item(i)["pm25Grade"].InnerText);//초미세먼지 지수
                }
            }

            list.Add(item);
            lwm.item = list;

            return lwm;
        }

        /// <summary>
        /// 미세먼지 등급
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public string GradeToString(string grade)
        {
            string result = string.Empty;

            switch (grade)
            {
                case "1": return "좋음";
                case "2": return "보통";
                case "3": return "나쁨";
                case "4": return "매우나쁨";
            }

            return result;
        }
        #endregion
    }
}
