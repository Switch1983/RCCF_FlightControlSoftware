using System;
using System.Threading;
using System.Text;
using System.IO.Ports;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;
using I2CBusClass;
using Gyro;
using Accel;
using OneWireTemp;
using GPS;
using Telemetry;
namespace RST_Telemetry_Alpha
{
    public class Program
    {
        public static void Main()
        {
            while(true)
            {            
                
                string temp_output = OneWire_Temperature.OneWire_Temperaturea();
                Debug.Print(temp_output);
                string Gyro_Output = Gyro.Gyro.Start();
                Debug.Print(Gyro_Output);
                string Acc_Output = Accelerometer.Start();
                Debug.Print(Acc_Output);
                GPS.GPS.Start();
                //Debug.Print(gps_output);
                string GlobalOutput = temp_output + "\n" + Gyro_Output + "\n" + Acc_Output + "\n";// + gps_output




                Telemetry_Transmit.Telemetry_Transmit_Out(GlobalOutput);
                
                //byte[] Gyro = Encoding.UTF8.GetBytes(GyroGlobal.ToString() + "/n");
            }
        }

    }
}
