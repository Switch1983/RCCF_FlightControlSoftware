using System;
using Microsoft.SPOT;
using RST_Telemetry_Alpha;
using System.Threading;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace Accel
{
    class Accelerometer
    {
        public static string AccGlobal;

        public static string Start()
        {
            AccGlobal = "";
            var accX = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An3);
            var accY = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An4);
            var accZ = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An5);
            Accelerometer_(accX, "X");
            Accelerometer_(accY, "Y");
            Accelerometer_(accZ, "Z");
            accX.Dispose();
            accY.Dispose();
            accZ.Dispose();
            string ReturnValue = AccGlobal;
            return ReturnValue;
        }
        private static void Accelerometer_(AnalogIn accXYZ, string axis)
        {
            int AccOutput = accXYZ.Read();
            if (AccGlobal.Length > 0)
                AccGlobal = AccGlobal + "   Axis (" + axis + ") " + AccOutput.ToString();
            else
                AccGlobal = "Accelerometer Axis (" + axis + ") " + AccOutput.ToString();
        }
    }
}
