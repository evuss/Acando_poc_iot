using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AzureWebAPI1.Models {
    public class SpotStatus {
        [Key]
        public string TS { get; set; }
        public string Result { get; set; }
        public IEnumerable<SpotState> SpotStates { get; set; }
    }
}