using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AzureWebAPI1.Models {
    public class SpotState {
        [Key]
        public string ID { get; set; }
        public string Ip { get; set; }
        public string Status { get; set; }
        public string TS { get; set; }
        public string Change { get; set; }
    }
}