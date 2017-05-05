using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AzureWebAPI1.Models {
    public class SpotState {
        [Key]
        public string SpotID { get; set; }
        public string SensorStatus { get; set; }
        public string ImageType { get; set; }
        public string XPos { get; set; }
        public string YPos { get; set; }
    }
}