using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Url_Shortener_DatabaseExercise.Models
{
    public class LogsModel
    {
        public string log_id { get; set; }
        public string short_url { get; set; }
        public DateTime time_date { get; set; }
        public string ip_address { get; set; }
    }
}