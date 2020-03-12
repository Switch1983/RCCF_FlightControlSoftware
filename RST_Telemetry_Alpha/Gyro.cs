using System;
using Microsoft.SPOT;
using RST_Telemetry_Alpha;
using System.Threading;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace Gyro
{
    class Gyro
    {
        public static string GyroGlobal;

        public static string Start()
        {
            //Wait for devices to stabilise
            Thread.Sleep(1500);

            #region variables

            const short sleep = 30;
            const float milliVoltPerDeg = 3.2f; //3.3mV/Deg/Sec (I needed to calibrate it to 3.2 for better readings)
            const short avg = 16;
            short gyroXZero = 0;
            short gyroYZero = 0;
            short gyroZZero = 0;
            short gyroXMin = 3300;
            short gyroYMin = 3300;
            short gyroZMin = 3300;
            short gyroXMax = 0;
            short gyroYMax = 0;
            short gyroZMax = 0;

            #endregion
            GyroGlobal = "";
            
            var gyroXReading = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An0);
            gyroXReading.SetLinearScale(gyroXMax, gyroXMin);
            var gyroYReading = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An1);
            gyroYReading.SetLinearScale(gyroYMax, gyroYMin);
            var gyroZReading = new AnalogIn((AnalogIn.Pin)FEZ_Pin.AnalogIn.An2);
            gyroZReading.SetLinearScale(gyroZMax, gyroZMin);

            //Calibrate the gyro
            gyroXZero = Calibrate(gyroXReading, ref gyroXMax, ref gyroXMin, sleep, avg, gyroXZero);
            gyroYZero = Calibrate(gyroYReading, ref gyroYMax, ref gyroYMin, sleep, avg, gyroYZero);
            gyroZZero = Calibrate(gyroZReading, ref gyroZMax, ref gyroZMin, sleep, avg, gyroZZero);

            //Read and print Gyro results
            Run(gyroXReading, gyroXMax, gyroXZero, gyroXMin, sleep, milliVoltPerDeg, "X");
            Run(gyroYReading, gyroYMax, gyroYZero, gyroYMin, sleep, milliVoltPerDeg, "Y");
            Run(gyroZReading, gyroZMax, gyroZZero, gyroZMin, sleep, milliVoltPerDeg, "Z");
            gyroXReading.Dispose();
            gyroYReading.Dispose();
            gyroZReading.Dispose();
            
           
            string ReturnValue = GyroGlobal;
            return ReturnValue;

        }
        private static short Calibrate(AnalogIn gyroReading, ref short gyroMax, ref short gyroMin, short sleep, short avg, short gyroZero)
        {
            var tempmV = (Int16)gyroReading.Read();
            Thread.Sleep(sleep);

            for (int i = 0; i < avg; i++)
            {
                Thread.Sleep(sleep / avg);
                tempmV = (Int16)gyroReading.Read();

                gyroZero += tempmV;

                if (tempmV > gyroMax)
                {
                    gyroMax = tempmV;
                }

                if (tempmV < gyroMin)
                {
                    gyroMin = tempmV;
                }
            }

            gyroMax += 3;
            gyroMin -= 3;
            gyroZero = (Int16)(gyroZero / avg);

            return gyroZero;
        }
        private static void Run(AnalogIn gyroReading, short gyroMax, short gyroZero, short gyroMin, short sleep,
                 float milliVoltPerDeg, string axis)
        {
            DateTime lastTime = DateTime.Now;
            float milliVolts = 0;
            float headingMilliVolts;
            DateTime time;
            float subheading;
            float heading = 0;

            //while (true)
            //{
                Thread.Sleep(sleep);

                milliVolts = gyroReading.Read();
                time = DateTime.Now;

                if (milliVolts >= gyroMin && milliVolts <= gyroMax)
                {
                    headingMilliVolts = 0f;
                }
                else
                {
                    headingMilliVolts = (gyroZero - milliVolts);
                }

                subheading = (headingMilliVolts / milliVoltPerDeg * time.Subtract(lastTime).Milliseconds / 1000);

                if (subheading > 0.066f || subheading < -0.066f)
                {
                    heading += subheading;
                }
                
                string test = "Axis (" + axis + ") " + heading.ToString("F") + "°";
                    
                   //"ms, MilliVolts: " + milliVolts + "mV (min:" + gyroMin + ";avg:" +
                   //         gyroZero + ";max:" + gyroMax + ")\nheading(mV): " +
                   //         headingMilliVolts.ToString("F") + "mV, Heading:" + 

                if (GyroGlobal.Length > 0)
                    GyroGlobal = GyroGlobal + "   " + test;
                else
                    GyroGlobal = "Gyro " + test;
                                    
                lastTime = time;
            //}
        }

    }
}
