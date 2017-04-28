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
//using Microsoft.ServiceBus.Messaging;
using System.Threading;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace POC_Obokningsbara_Rum
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        static DeviceClient deviceClient;
        static string iotHubUri = "obokbararumsuite3558e.azure-devices.net";
        static string deviceName = "IoTTest";
        static string deviceKey = "tOp7920qQeGtyPjoRRnlOrmvHBXIHqYL6+4phP0KGTY=";

        private const int ledPin = 16;
        private const int pirPin = 21;
        private int toggle = 0;

        private GpioPin led;
        private GpioPin pir;

        private DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Timer_Tick;

            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceName, deviceKey), TransportType.Http1);

            InitGPIO();

            if (led != null)
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
                led = null;
                //GpioStatus.Text = "There is no GPIO controller.";
                return;
            }

            // set up the LED on the defined GPIO pin
            // and set it to High to turn off the LED
            led = gpio.OpenPin(ledPin);
            led.Write(GpioPinValue.High);
            led.SetDriveMode(GpioPinDriveMode.Output);

            // set up the PIR sensor's signal on the defined GPIO pin
            // and set it's initial value to Low
            //pir = gpio.OpenPin(pirPin);
            //pir.SetDriveMode(GpioPinDriveMode.Input);

            //GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private void Timer_Tick(object sender, object e)
        {
            //TimeInterval.Text = DateTime.Now.ToString();

            // read the signal from the PIR sensor
            // if it is high, then motion was detected
            //if (pir.Read() == GpioPinValue.High)
            if (toggle == 0)
            {
                // turn on the LED
                led.Write(GpioPinValue.Low);

                // update the sensor status in the UI
                //SensorStatus.Text = "Motion detected!";

                SendDeviceToCloudMessagesAsync("Hello World, This is Acando IoT POC sending a message");
                toggle = 1;
            }
            else
            {
                // turn off the LED
                led.Write(GpioPinValue.High);

                // update the sensor status in the UI
                //SensorStatus.Text = "No motion detected.";

                SendDeviceToCloudMessagesAsync("Hello World, This is Acando IoT POC sending a message");
                toggle = 0;
            }
        }

        private static async void SendDeviceToCloudMessagesAsync(string status)
        {

            var telemetryDataPoint = new
            {
                deviceId = deviceName,
                status = "Acando IoT POC Hälsar Hej Hej!"
            };
            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
        }

        /*private static async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, DateTime.UtcNow);
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());

                //Console.WriteLine("Message received. Partition: {0} Data: '{1}'", partition, data);

            }
        }*/

    }
}
