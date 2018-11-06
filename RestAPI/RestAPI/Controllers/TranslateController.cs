using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Translation.V2;
using RestAPI.Models;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;

namespace RestAPI.Controllers
{
    public class TranslateController : ApiController
    {
        #region NugetGoogleAPI
        /// <summary>
        /// GoogleTranslateNugetPackage를 사용한 번역 API호출
        /// </summary>
        /// <param name="query"></param>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Nuget")]
        public IHttpActionResult Nuget(string query,string target, string source)
        {
            TranslationClient client = TranslationClient.Create(GoogleCredential.FromFile("D:\\OpenAPi\\googlekey\\My Project-350ac8291096.json"));

            var response = client.TranslateText(query, target, source);

            TranslateModel tm = new TranslateModel();

            tm.query = response.OriginalText;
            tm.target = response.TargetLanguage;
            if (response.SpecifiedSourceLanguage == null)
            {
                tm.source = response.DetectedSourceLanguage;
            }
            else
            {
                tm.source = response.SpecifiedSourceLanguage;
            }
            tm.result = response.TranslatedText;

            return Json(tm);
        }
        #endregion

        #region RestTranslateAPI
        /// <summary>
        /// GoogleTranslate RESTAPI 호출
        /// </summary>
        /// <param name="q"></param>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("GetTranslate")]
        public IHttpActionResult GetTranslate(string q, string target,string source)
        {
            //RestAPI 호출
            WebRequest request = null;
            string serviceKey = ConfigurationSettings.AppSettings["googleTranslateApiKey"];

            request = WebRequest.Create("https://translation.googleapis.com/language/translate/v2?q=" + q + "&target=" + target + "&source=" + source+ "&key=" + serviceKey);

            //RestAPI 응답 메시지 
            Stream dataStream = null;

            var response = request.GetResponse();
            dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            return Json(result);
        }

        #endregion
    }
}
