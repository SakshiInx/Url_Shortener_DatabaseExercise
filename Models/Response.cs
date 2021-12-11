using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Url_Shortener_DatabaseExercise.Models
{
    public class Response
    {
        public string status_code { get; set; }
        public string status_description { get; set; }
        public string short_url { get; set; }
        

    }
}