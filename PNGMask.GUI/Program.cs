using PNGMask.Providers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PNGMask.GUI
{
    static class Program
    {
        public static Provider
        XOREOF = new Provider("XOR (EOF)", typeof(XOREOF)),
        XORTXT = new Provider("XOR (tEXt)", typeof(XORTEXT)),
        XORIDAT = new Provider("XOR (IDAT)", typeof(XORIDAT));
        public static Provider[] AllProviders;
        public static Provider[] Providers = 
        {
            new Provider("Colorcrash", typeof(ColorCrash))
        };

        [STAThread]
        static void Main()
        {
            List<Provider> prov = new List<Provider>();
            prov.Add(XOREOF);
            prov.Add(XORTXT);
            prov.Add(XORIDAT);
            prov.AddRange(Providers);
            AllProviders = prov.ToArray();
            prov = null;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }
    }
}
