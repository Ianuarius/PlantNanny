using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Threading.Tasks;

namespace PlantNanny
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        public void SetReminder_Click(object sender, RoutedEventArgs e)
        {
            string time = TimeInput.Text;
            SetReminder(time);
        }

        public DispatcherTimer timer;

        public void SetReminder(string time)
        {
            string[] timeParts = time.Split(':');
            int hour = int.Parse(timeParts[0]);
            int minute = int.Parse(timeParts[1]);
            
            DateTime now = DateTime.Now;
            DateTime nextReminder = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
            
            if (now > nextReminder)
            {
                nextReminder = nextReminder.AddDays(1);
            }

            TimeSpan timeUntilNextReminder = nextReminder - now;

            timer = new DispatcherTimer();
            timer.Interval = timeUntilNextReminder;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Timer_Tick(object sender, object e)
        {
            timer.Stop();
            ShowToastNotification("Plant care reminder", "It's time to take care of your plants.");
            timer.Interval = TimeSpan.FromDays(1);
            timer.Start();
        }

        public void ShowToastNotification(string title, string content)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode(title));
            stringElements[1].AppendChild(toastXml.CreateTextNode(content));

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
