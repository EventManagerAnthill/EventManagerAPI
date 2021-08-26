using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Study.EventManager.Services.Wrappers.Contracts
{
    public interface IGenerateQRCode
    {
        public string QRCode(string qrText);
    }
}
