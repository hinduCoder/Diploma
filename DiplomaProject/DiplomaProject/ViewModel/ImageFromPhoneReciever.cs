using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiplomaProject.ViewModel
{
    public class ImageFromPhoneReciever
    {
        public async Task<BitmapSource> RecieveAsync()
        {
            if (!Directory.Exists("temp"))
            {
                var tempDirectory = Directory.CreateDirectory("temp");
                tempDirectory.Attributes = FileAttributes.Hidden;
            }
            var fileName = Path.Combine("temp", String.Format("Image_{0}.jpg", DateTime.Now.Millisecond));
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var socket = new Socket(SocketType.Stream, ProtocolType.Tcp))
                {
                    EndPoint endPoint = new IPEndPoint(IPAddress.Any, 50001);
                    socket.Bind(endPoint);
                    socket.Listen(1);
                    var acceptSocket = await Task.Factory.StartNew<Socket>(socket.Accept);
                    var bufferSize = 10000;
                    var buffer = new byte[bufferSize];
                    int received;
                    do
                    {
                        received = acceptSocket.Receive(buffer, bufferSize, SocketFlags.None);
                        stream.Write(buffer, 0, received);
                    } while (received > 0);
                }
            }
            return new BitmapImage(new Uri(Path.GetFullPath(fileName)));
        }

    }
}