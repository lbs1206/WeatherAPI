using RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Common
{
    public class GridXY
    {
        /// <summary>
        /// 위경도 좌표를 격자 좌표로 변환
        /// </summary>
        /// <param name="latX"></param>
        /// <param name="lngY"></param>
        /// <returns></returns>
        public GridXYModel change(double latX, double lngY)
        {
            GridXYModel rs = new GridXYModel();

            double RE = 6371.00877; //지구 반경(km)
            double GRID = 5.0; //격자 간격(km)
            double SLAT1 = 30.0; //투영 위도1(degree)
            double SLAT2 = 60.0; //투영 위도2(degree)
            double OLON = 126.0; //기준점 경도(degree)
            double OLAT = 38.0; //기준점 위도(degree)
            double XO = 43; //기준점 X좌표(GRID)
            double YO = 136; //기준점 Y좌표(GRID)

            //
            // LCC DFS 좌표변환 ( code : "TO_GRID"(위경도->좌표, lat_X:위도,  lng_Y:경도), "TO_GPS"(좌표->위경도,  lat_X:x, lng_Y:y) )
            //


            double DEGRAD = Math.PI / 180.0;
            double RADDEG = 180.0 / Math.PI;

            double re = RE / GRID;
            double slat1 = SLAT1 * DEGRAD;
            double slat2 = SLAT2 * DEGRAD;
            double olon = OLON * DEGRAD;
            double olat = OLAT * DEGRAD;

            double sn = Math.Tan(Math.PI * 0.25 + slat2 * 0.5) / Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sn = Math.Log(Math.Cos(slat1) / Math.Cos(slat2)) / Math.Log(sn);
            double sf = Math.Tan(Math.PI * 0.25 + slat1 * 0.5);
            sf = Math.Pow(sf, sn) * Math.Cos(slat1) / sn;
            double ro = Math.Tan(Math.PI * 0.25 + olat * 0.5);
            ro = re * sf / Math.Pow(ro, sn);


            rs.gridx = latX;
            rs.gridy = lngY;
            double ra = Math.Tan(Math.PI * 0.25 + (latX) * DEGRAD * 0.5);
            ra = re * sf / Math.Pow(ra, sn);
            double theta = lngY * DEGRAD - olon;
            if (theta > Math.PI) theta -= 2.0 * Math.PI;
            if (theta < -Math.PI) theta += 2.0 * Math.PI;
            theta *= sn;
            rs.gridx = Math.Floor(ra * Math.Sin(theta) + XO + 0.5);
            rs.gridy = Math.Floor(ro - ra * Math.Cos(theta) + YO + 0.5);

            return rs;
        }

        


    }
}