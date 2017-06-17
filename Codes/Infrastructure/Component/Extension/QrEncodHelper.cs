using System.Drawing.Imaging;
using System.IO;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Component.Extension
{
    public static class QrEncodHelper
    {
        public static byte[] Create(string content)
        {
            var encoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qr;
            if (encoder.TryEncode(content, out qr))
            {
                var ms=new MemoryStream();
                var render = new GraphicsRenderer(new FixedModuleSize(12, QuietZoneModules.Two));
                render.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                return ms.GetBuffer();
            }
            return null;
        }
    }
}
