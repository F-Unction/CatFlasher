using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFlasher.Utils
{
    public class FlashArgs : EventArgs
    {
        public UsbController.Device.DMode TargetMode { get; set; }
        public string Target { get; set; }
        public string UnlockCode { get; set; }
        public bool DisableFBLOCK { get; set; }
        public bool Reboot { get; set; }
        public Bootloader Bootloader { get; set; } = null;
    }
}
