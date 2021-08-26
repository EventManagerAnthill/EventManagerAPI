using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QRCoder;
using Study.EventManager.Services.Wrappers.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Study.EventManager.Services.Wrappers
{
    internal class GenerateQRCodeWrapper : IGenerateQRCode
    {
        public string QRCode(string qrText)
        {            
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            string base64ImgString = Convert.ToBase64String(BitmapToBytes(qrCodeImage));

            return base64ImgString;
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
