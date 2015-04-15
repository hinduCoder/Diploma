using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DiplomaProject.ViewModel
{
    public class ImageFromPhoneReciever
    {
        public Task<BitmapSource> RecieveAsync()
        {
            return Task<BitmapSource>.Factory.StartNew(Recieve);
        }

        private static BitmapSource Recieve()
        {
            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 50001);
            socket.Bind(endPoint);
            var buffer = new byte[10000000];
            socket.ReceiveFrom(buffer, ref endPoint);
            var stream = new MemoryStream(buffer);
            var jpegBitmapDecoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.None);
            return jpegBitmapDecoder.Frames[0];
        }
    }
}