using System;
using System.Threading;
using System.Text;
using System.IO.Ports;
using Microsoft.SPOT;

using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;
using Microsoft.SPOT.Hardware;
using BMP085_Namepsace;
using Gyro_Accel;

namespace OneWireTemp
{
    public class OneWire_Temperature
    {
        public static string OneWire_Temperaturea()
        {
            // Change this your correct pin!
            Cpu.Pin myPin = (Cpu.Pin)FEZ_Pin.Digital.Di4;

            OneWire ow = new OneWire(myPin);
            ushort temperature;

            {

                BMP085 pressureSensor = new BMP085(0x77, BMP085.DeviceMode.UltraLowPower);

                string temperature_Pressure;
                if (ow.Reset())
                {
                    ow.WriteByte(0xCC);     // Skip ROM, we only have one device
                    ow.WriteByte(0x44);     // Start temperature conversion

                    while (ow.ReadByte() == 0) ;   // wait while busy

                    ow.Reset();
                    ow.WriteByte(0xCC);     // skip ROM
                    ow.WriteByte(0xBE);     // Read Scratchpad

                    temperature = ow.ReadByte();                 // LSB 
                    temperature |= (ushort)(ow.ReadByte() << 8); // MSB

                    temperature_Pressure = "Temperature: " + temperature / 16 + "\n" + "BMP085 Pascal: " + pressureSensor.Pascal +
                        "   BMP085 InchesMercury: " + pressureSensor.InchesMercury.ToString("F2") + "   BMP085 Temp*C: " + pressureSensor.Celsius.ToString("F2");
                }
                else
                {
                    temperature_Pressure = "Temperature Device is not Detected";
                }

                ow.Dispose();
                Thread.Sleep(1000);
                return temperature_Pressure;
            }
        }
    }
}

