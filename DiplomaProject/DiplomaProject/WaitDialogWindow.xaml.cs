using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace DiplomaProject {
    /// <summary>
    /// Interaction logic for WaitDialogWindow.xaml
    /// </summary>
    public partial class WaitDialogWindow : Window
    {
        private IEnumerable<Host> _availibleInterfaces; 
        public WaitDialogWindow() {
            InitializeComponent();
            _availibleInterfaces = GetAvailibaleInterfaces();
            foreach (var @interface in _availibleInterfaces)
            {
                ipInterfacesTextBlock.Text += String.Format("{0}: {1}\n", @interface.Name, @interface.Address);
            }
            SendBroadcast();
        }

        private void SendBroadcast()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            new Timer(TimerCallback, socket, 0, 2000);
        }

        private void TimerCallback(object state)
        {
            foreach (var address in _availibleInterfaces.Select(h => h.Address.ToString()))
            {
                var socket = state as Socket;
                socket.BeginSendTo(new byte[] {127}, 0, 1, SocketFlags.None,
                    new IPEndPoint(IPAddress.Parse(address.Remove(address.LastIndexOf(".")+1) + "255") , 50001), null, null);
            }
        }

        private IEnumerable<Host> GetAvailibaleInterfaces()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            var availibleInterfaces = new List<Host>();
            foreach(var networkInterface in networkInterfaces) {
                if(networkInterface.OperationalStatus == OperationalStatus.Up)
                    foreach(var unicastAddress in networkInterface.GetIPProperties().UnicastAddresses) {
                        var ipAddress = unicastAddress.Address;
                        if (ipAddress.AddressFamily == AddressFamily.InterNetwork &&
                            !ipAddress.Equals(IPAddress.Loopback))
                            availibleInterfaces.Add(new Host(networkInterface.Name, ipAddress));
                    }
            }
            return availibleInterfaces;
        }
    }

    struct Host
    {
        public string Name { get; set; }
        public IPAddress Address { get; set; }

        public Host(string name, IPAddress address) : this()
        {
            Address = address;
            Name = name;
        }
    }
}
