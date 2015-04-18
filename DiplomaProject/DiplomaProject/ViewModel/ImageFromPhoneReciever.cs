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
            JpegBitmapDecoder jpegBitmapDecoder;
            using (var stream = new MemoryStream())
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
                stream.Seek(0, SeekOrigin.Begin);
                jpegBitmapDecoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            }
            return jpegBitmapDecoder.Frames[0];
        }

    }
}