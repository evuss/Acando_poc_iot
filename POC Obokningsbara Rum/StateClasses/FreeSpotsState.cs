using System;

namespace POC_Obokningsbara_Rum.StateClasses {
    class FreeSpotsState {
        public string PendingStatus = "";    // State to take after smoothing
        public DateTime PendingStatusTime = new DateTime(0);
        public DateTime LastSendTime = new DateTime(0);
        public Boolean LEDGreenOn = true;

        public FreeSpotsState() { }
    }
}
