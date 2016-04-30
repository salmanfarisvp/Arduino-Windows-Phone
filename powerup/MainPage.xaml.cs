
//Developed by Avirup171
//Thanks to Marcos Pereira
//Date of last edit: 28-06-2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using powerup.Resources;
using BluetoothConnectionManager;
using Windows.Networking.Proximity;


namespace powerup
{
    public partial class MainPage : PhoneApplicationPage
    {
        String s;
        // Constructor
        private ConnectionManager connectionManager;
        public MainPage()
        {
            connectionManager = new ConnectionManager();
            InitializeComponent();
            
            
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            on.IsEnabled = false;
            off.IsEnabled = false;
            //Initialize the connection manager
            connectionManager.Initialize();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            connectionManager.Terminate();
        }
        private async void on_Click(object sender, RoutedEventArgs e)
        {
            string command = "1";
            await connectionManager.SendCommand(command);
        }

        private async void off_Click(object sender, RoutedEventArgs e)
        {
            string command = "0";
            await connectionManager.SendCommand(command);
        }
        private void connect_Click(object sender, RoutedEventArgs e)
        {
            
            //get the string
            s = DeviceName.Text;
            if (DeviceName.Text == "")
            {
                MessageBox.Show("Please enter the device name");
                connect.Content = "Connect";
            }
            else
            {
                try
                {
                  
                    AppToDevice();
                }
                catch (Exception)
                {

                    MessageBox.Show("Some error occured! Please try again :(  ");
                }
            }
        }
        private async void AppToDevice()
        {
            connect.Content = "Connecting...";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            var pairedDevices = await PeerFinder.FindAllPeersAsync();

            if (pairedDevices.Count == 0)
            {
                MessageBox.Show("No paired devices were found.");
            }
            else
            {
                foreach (var pairedDevice in pairedDevices)
                {
                    if (pairedDevice.DisplayName == DeviceName.Text)
                    {
                        connectionManager.Connect(pairedDevice.HostName);
                        connect.Content = "Connected";
                        on.IsEnabled = true;
                        off.IsEnabled = false;
                        continue;
                    }
                }
            }
        }

     }
}