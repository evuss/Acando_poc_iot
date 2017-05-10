using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System.Text;
using Windows.Devices.Gpio;
using Microsoft.Azure.Devices.Client;
using System.Threading;
using System.Threading.Tasks;
using POC_Obokningsbara_Rum.StateClasses;
//using System.Net.NetworkInformation;
using Windows.Networking.Connectivity;
using Windows.Networking;
using LightBuzz.SMTP;
using Windows.ApplicationModel.Email;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace POC_Obokningsbara_Rum
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        FreeSpotsSettings settings = new FreeSpotsSettings(FreeSpotsSettings.mode.DEBUG1);

        // Current state of application
        FreeSpotsState state = new FreeSpotsState();

        // Message for serialization
        MsgBody msg = new MsgBody();

        static DeviceClient deviceClient;
        static string iotHubUri = "obokbararumsuite3558e.azure-devices.net";
        static string deviceName = "IoTTest";
        static string deviceKey = "tOp7920qQeGtyPjoRRnlOrmvHBXIHqYL6+4phP0KGTY=";

        private const int greenLedPin = 16;
        private const int redLedPin = 20;
        private const int pirPin = 21;
        private int toggle = 0;

        private GpioPin ledRed;
        private GpioPin ledGreen;
        private GpioPin pir;

        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            SendDeviceInfo();

            // Set task to be run in background
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(settings.timerCheckIntervall);
            timer.Tick += Timer_Tick;

            // IoT hub access
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, deviceKey), TransportType.Http1);

            InitGPIO();

            if (ledGreen != null && ledRed != null)
            {
                timer.Start();
            }
        }

        private void InitGPIO()
        {
            // get the GPIO controller
            var gpio = GpioController.GetDefault();

            // return an error if there is no gpio controller
            if (gpio == null)
            {
                ledGreen = null;
                //GpioStatus.Text = "There is no GPIO controller.";
                return;
            }

            // set up the LED on the defined GPIO pin
            // and set it to High to turn off the LED
            ledGreen = gpio.OpenPin(greenLedPin);
            ledGreen.Write(GpioPinValue.Low);
            ledGreen.SetDriveMode(GpioPinDriveMode.Output);

            ledRed = gpio.OpenPin(redLedPin);
            ledRed.Write(GpioPinValue.High);
            ledRed.SetDriveMode(GpioPinDriveMode.Output);

            // set up the PIR sensor's signal on the defined GPIO pin
            // and set it's initial value to Low
            pir = gpio.OpenPin(pirPin);
            pir.SetDriveMode(GpioPinDriveMode.Input);

            //GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private void Timer_Tick(object sender, object e)
        {
            string pirCurrentState = "U";
            MsgBody tmpMsg = new MsgBody(msg);

            try
            {
                // Initialize if needed
                if (pir == null)
                {  // Assume that initialization is needed
                    InitGPIO();
                }

                if (pir != null)
                {  // Only if we are intialized
                    // Get Current IP and MAC (May have changed!)
                    tmpMsg.ID = GetSensorId();
                    tmpMsg.Ip = GetCurrentIpv4Address();

                    // Check PIR Status (High == Movement)
                    // if it is high, then motion was detected
                    if (pir.Read() == GpioPinValue.High)
                    {
                        pirCurrentState = "O";
                    }
                    else
                    {
                        pirCurrentState = "F";
                    }

                    // Handle state changes with smoothing
                    if (pirCurrentState != state.PendingStatus || state.PendingStatus == "")
                    {  // A change. Start counting smoothing time. (Ie time with the same state required for change)
                        state.PendingStatusTime = DateTime.Now;
                        state.PendingStatus = pirCurrentState;
                    }

                    if (state.PendingStatus != tmpMsg.Status &&     // This is a Status change AND....
                                (tmpMsg.Status == "" ||             // this is the first run OR ...
                                (state.PendingStatus == "x  " && (DateTime.Now - state.PendingStatusTime).TotalSeconds >= settings.timerStateOSmoothing) || // Pending status is "Occupied" and O-Smoothing-time has passed OR ...
                                (state.PendingStatus == "F" && (DateTime.Now - state.PendingStatusTime).TotalSeconds >= settings.timerStateFSmoothing)))
                    { // Pending status is "Free" and F-Smoothing-time has passed
                        if (state.PendingStatus == "O")
                        {
                            LedSetState(ledRed, true);  // Turn on RED LED
                            LedSetState(ledGreen, false);
                        }
                        else
                        {
                            LedSetState(ledRed, false); // Turn off RED LED
                            LedSetState(ledGreen, true);
                        }

                        tmpMsg.Status = state.PendingStatus;   // Set the new status to the message
                        tmpMsg.Change = "T"; // This is a change of status (T)rue
                        tmpMsg.TS = DateTime.Now.ToString();

                        SendDeviceToCloudMessagesAsync(tmpMsg);

                        // Save any changes (to this last so that any exception does not save changes.)
                        msg.setMsgBody(tmpMsg);
                        state.LastSendTime = DateTime.Now;

                        // Toggle green led to show a message has been sent
                        //LedToggleGreen();
                    }
                    else if ((DateTime.Now - state.LastSendTime).TotalSeconds > settings.timerKeepAliveIntervall)
                    {  // No Change of status. Is it time fot "is alive"?
                        tmpMsg.Change = "F";   // Status change (F)alse. (Set if we will send (is alive)
                        tmpMsg.TS = DateTime.Now.ToString();

                        // Send message to IoT hub
                        SendDeviceToCloudMessagesAsync(tmpMsg);

                        // Save any changes (to this last so that any exception does not save changes.)
                        msg.setMsgBody(tmpMsg);
                        state.LastSendTime = DateTime.Now;

                        // Toggle green led to show a message has been sent
                        LedToggleGreen();
                    }
                }
            }
            catch (Exception exception)
            {
                //Status not written
            }

            //if (Convert.ToString(ReceiveC2dAsync().Result) == "Received")
                //led.Write(GpioPinValue.Low);


        }

        private static async void SendDeviceToCloudMessagesAsync(MsgBody msgObj)
        {

            var messageString = JsonConvert.SerializeObject(msgObj);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));
      
            await deviceClient.SendEventAsync(message);
        }

        private static async Task<string> ReceiveC2dAsync()
        {
            //Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null)
                {
                    return "Not Received";
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received message: {0}", Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                Console.ResetColor();

                await deviceClient.CompleteAsync(receivedMessage);
                return "Received";

            }
        }

        void LedSetState(GpioPin led, bool lit)
        {
            if (lit)
            {
                led.Write(GpioPinValue.Low);
            }
            else
            {
                led.Write(GpioPinValue.High);
            }
        }

        void LedToggleGreen()
        {
            state.LEDGreenOn = !state.LEDGreenOn;
            LedSetState(ledGreen, state.LEDGreenOn);
        }

        public static string GetCurrentIpv4Address()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp != null && icp.NetworkAdapter != null && icp.NetworkAdapter.NetworkAdapterId != null)
            {
                var name = icp.ProfileName;

                try
                {
                    var hostnames = NetworkInformation.GetHostNames();

                    foreach (var hn in hostnames)
                    {
                        if (hn.IPInformation != null &&
                            hn.IPInformation.NetworkAdapter != null &&
                            hn.IPInformation.NetworkAdapter.NetworkAdapterId != null &&
                            hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId &&
                            hn.Type == HostNameType.Ipv4)
                        {
                            return hn.CanonicalName;
                        }
                    }
                }
                catch (Exception)
                {
                    // do nothing
                    // in some (strange) cases NetworkInformation.GetHostNames() fails... maybe a bug in the API...
                }
            }

            return "Get IP Failed";
        }

        public static string GetSensorId()
        {
            // Possible limitations for a rollout: Is NetworkAdopterID changed when deployed to a new Pi3?
            // Possible future adoptions: Always get Wifi Adapter ID, Enable manual SensorID using Config file, Add IoT GUI to show / Set SensorID.
            var icp = NetworkInformation.GetInternetConnectionProfile();
            if (icp != null && icp.NetworkAdapter != null && icp.NetworkAdapter.NetworkAdapterId != null)
            {
                return icp.NetworkAdapter.NetworkAdapterId.ToString();
            }

            return "Get SensorID failed";
        }

        private async void SendDeviceInfo()
        {
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587, false, "takler14@hotmail.com", "lucke1");
            EmailMessage emailMessage = new EmailMessage();

            emailMessage.To.Add(new EmailRecipient("lucas.nilsson@acando.com"));
            emailMessage.CC.Add(new EmailRecipient("tone.pedersen@acando.com"));
            //emailMessage.Bcc.Add(new EmailRecipient("someone3@anotherdomain.com"));
            emailMessage.Subject = "Info from your Raspberry Pi!";
            emailMessage.Body = "The IP-adress of this Raspberry Pi is: " + GetCurrentIpv4Address() + "\nAnd MAC-adress as: " + GetSensorId();

            await client.SendMailAsync(emailMessage);
            return;
        }

    }
}
