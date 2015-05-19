using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;

namespace DiplomaProject {
    /// <summary>
    /// Interaction logic for WaitDialogWindow.xaml
    /// </summary>
    public partial class WaitDialogWindow : Window {
        public WaitDialogWindow() {
            InitializeComponent();
            ShowAvailibaleInterfaces();
        }

        private void ShowAvailibaleInterfaces()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var availibleInterfaces = String.Empty;
            foreach(var networkInterface in networkInterfaces) {
                if(networkInterface.OperationalStatus == OperationalStatus.Up)
                    foreach(var unicastAddress in networkInterface.GetIPProperties().UnicastAddresses) {
                        var ipAddress = unicastAddress.Address;
                        if (ipAddress.AddressFamily == AddressFamily.InterNetwork &&
                            !ipAddress.Equals(IPAddress.Loopback))
                            availibleInterfaces += String.Format("{0}: {1}\n", networkInterface.Name, ipAddress);
                    }
            }
            ipInterfacesTextBlock.Text = availibleInterfaces;
        }
    }
}
