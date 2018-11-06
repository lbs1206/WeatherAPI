using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{
    public class TranslateModel
    {
        /// <summary>
        /// 타겟 언어
        /// </summary>
        public string target { get; set; }

        /// <summary>
        /// 번역할 언어
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 번역할 문장
        /// </summary>
        public string query { get; set; }

        /// <summary>
        /// 결과값
        /// </summary>
        public string result { get; set; }
    }
}