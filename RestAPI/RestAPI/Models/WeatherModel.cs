using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{

    public class WeatherModel
    {

    }
    #region 위도 경도(좌표) 날씨 API 모델
    public class SpaceWeatherModel
    {
        public List<SpaceWeather> item { get; set; }
    }

    public class SpaceWeather
    {
        /// <summary>
        /// 발표 일자
        /// </summary>
        public string baseDate { get; set; } 

        /// <summary>
        /// 발표 시각
        /// </summary>
        public string baseTime { get; set; }

        /// <summary>
        /// 카테고리
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 예보 일자
        /// </summary>
        public string fcstDate { get; set; }

        /// <summary>
        /// 예보 시각
        /// </summary>
        public string fcstTime { get; set; }

        /// <summary>
        /// 예보 값
        /// </summary>
        public string fcstValue { get; set; }

        /// <summary>
        /// 예보지점 X 좌표
        /// </summary>
        public string nx { get; set; }

        /// <summary>
        /// 예보지점 Y 좌표
        /// </summary>
        public string ny { get; set; }
    }
    #endregion

    #region 현위치,지역 날씨 미세먼지 API 모델
    public class LocationWeatherModel
    {
        public List<LocationWeather> item { get; set; }
    }

    public class LocationWeather
    {
        /// <summary>
        /// 오늘 아침 최저 기온
        /// </summary>
        public string morning1 { get; set; }

        /// <summary>
        /// 오늘 아침 하늘 상태
        /// </summary>
        public string mSky1 { get; set; }

        /// <summary>
        /// 오늘 아침 비올 확율
        /// </summary>
        public string mRain1 { get; set; }

        /// <summary>
        /// 오늘 낮 최고 기온
        /// </summary>
        public string dayTime1 { get; set; }

        /// <summary>
        /// 오늘 낮 하늘 상태
        /// </summary>
        public string dSky1 { get; set; }

        /// <summary>
        /// 오늘 낮 비올 확율
        /// </summary>
        public string dRain1 { get; set; }

        /// <summary>
        /// 내일 아침 최저 기온
        /// </summary>
        public string morning2 { get; set; }

        /// <summary>
        /// 내일 아침 하늘 상태
        /// </summary>
        public string mSky2 { get; set; }

        /// <summary>
        /// 내일 아침 비올 확율
        /// </summary>
        public string mRain2 { get; set; }

        /// <summary>
        /// 내일 낮 최고 기온
        /// </summary>
        public string dayTime2 { get; set; }

        /// <summary>
        /// 내일 낮 하늘 상태
        /// </summary>
        public string dSky2 { get; set; }

        /// <summary>
        /// 내일 낮 비올 확율
        /// </summary>
        public string dRain2 { get; set; }

        /// <summary>
        /// 미세먼지 농도
        /// </summary>
        public string pm10 { get; set; }

        /// <summary>
        /// 미세먼지 등급 
        /// </summary>
        public string pm10Grade { get; set; }

        /// <summary>
        /// 초미세먼지 농도
        /// </summary>
        public string pm25 { get; set; }

        /// <summary>
        /// 초미세먼지 등급
        /// </summary>
        public string pm25Grade { get; set; }
    }
    #endregion

    #region grid.json 모델
    public class gridJson
    {
        /// <summary>
        /// 주소 depth1
        /// </summary>
        public string depth1 { get; set; }

        /// <summary>
        /// 주소 depth2
        /// </summary>
        public string depth2 { get; set; }

        /// <summary>
        /// 주소 depth3
        /// </summary>
        public string depth3 { get; set; }

        /// <summary>
        /// 격자 좌표 X
        /// </summary>
        public string gridX { get; set; }

        /// <summary>
        /// 격자 좌표 Y
        /// </summary>
        public string gridY { get; set; }

        /// <summary>
        /// 시도 (ex. 서울특별시, 경기도)
        /// </summary>
        public string sido { get; set; }

        /// <summary>
        /// 구군 (ex. 강남구, 부여군)
        /// </summary>
        public string gugun { get; set; }
        
    }
    #endregion

}