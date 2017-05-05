using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AzureWebAPI1.Models {
    public class Spots {
        [Key]
        public string SpotID { get; set; }
        public string SpotName { get; set; }
        public string SensorID { get; set; }
        public string ImageType { get; set; }
        public string XPos { get; set; }
        public string YPos { get; set; }

        public Spots() {

        }

        public Spots(Spots copySpot) {
            this.SpotID = copySpot.SpotID;
            this.SpotName = copySpot.SpotName;
            this.SensorID = copySpot.SensorID;
            this.ImageType = copySpot.ImageType;
            this.XPos = copySpot.XPos;
            this.YPos = copySpot.YPos;
        }

        public Spots(string SpotID, string SpotName, string SensorID, string ImageType, string XPos, string YPos) {
            this.SpotID = SpotID;
            this.SpotName = SpotName;
            this.SensorID = SensorID;
            this.ImageType = ImageType;
            this.XPos = XPos;
            this.YPos = YPos;
        }
    }
}