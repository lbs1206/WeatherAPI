using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace RestAPI.Models
{
    #region 미세먼지 API 모델
    public class AirFineDustModel
    {      
        public List<AirFineDust> item { get; set; }
    }

    public class AirFineDust
    {
        /// <summary>
        /// 측정소 명
        /// </summary>
        public string stationname { get; set; }

        /// <summary>
        /// 측정망 정보
        /// </summary>
        public string mangname { get; set; }
        
        /// <summary>
        /// 측정 일시
        /// </summary>
        public string datatime { get; set; }
        
        /// <summary>
        /// 아황산가스 농도
        /// </summary>
        public string so2value { get; set; }
        
        /// <summary>
        /// 일산화탄소 농도
        /// </summary>
        public string covalue { get; set; }
        
        /// <summary>
        /// 오존 농도
        /// </summary>
        public string o3value { get; set; }
        
        /// <summary>
        /// 이산화질소 농도
        /// </summary>
        public string no2value { get; set; }
        
        /// <summary>
        /// 미세먼지 농도
        /// </summary>
        public string pm10value { get; set; }
        
        /// <summary>
        /// 미세먼지 24시간 예측이동 농도
        /// </summary>
        public string pm10value24 { get; set; }
        
        /// <summary>
        /// 초미세먼지 농도
        /// </summary>
        public string pm25value { get; set; }
        
        /// <summary>
        /// 초미세먼지 24시간 예측이동 농도
        /// </summary>
        public string pm25value24 { get; set; }
        
        /// <summary>
        /// 통합대기환경수치
        /// </summary>
        public string khaivalue { get; set; }
        
        /// <summary>
        /// 통합대기 환경지수
        /// </summary>
        public string khaigrade { get; set; }
        
        /// <summary>
        /// 아황산가스 지수
        /// </summary>
        public string so2grade { get; set; }
        
        /// <summary>
        /// 일산화탄소 지수
        /// </summary>
        public string cograde { get; set; }
        
        /// <summary>
        /// 오존 지수
        /// </summary>
        public string o3grade { get; set; }
        
        /// <summary>
        /// 이산화질소 지수
        /// </summary>
        public string no2grade { get; set; }
        
        /// <summary>
        /// 미세먼지 24시간 등급
        /// </summary>
        public string pm10grade { get; set; }
        
        /// <summary>
        /// 초미세먼지 24시간 등급
        /// </summary>
        public string pm25grade { get; set; }
        
        /// <summary>
        /// 미세먼지 1시간 등급
        /// </summary>
        public string pm10grade1h { get; set; }
        
        /// <summary>
        /// 초미세먼지 1시간 등급
        /// </summary>
        public string pm25grade1h { get; set; }
    }
    #endregion
}