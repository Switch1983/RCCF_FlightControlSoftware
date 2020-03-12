using System;
using System.Threading;
using System.Text;
using System.IO.Ports;

using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;
using GHIElectronics.NETMF.Hardware;


namespace Telemetry
{
    class Telemetry_Transmit
    {
        public static byte[] byteConcat; 
        public static void Telemetry_Transmit_Out(string Output)
        {
            byte[] byteConcatonate = Encoding.UTF8.GetBytes(Output);
            byteConcat = byteConcatonate;
            Transmit_Now();
        }
        private static void Transmit_Now()
        {
            //SerialPort Test = new SerialPort("COM1", , Parity.None, 8, StopBits.One);
            //if (Test.IsOpen == true)
            //{
            //    Test.Flush();
            //    Test.Write(byteConcat, 0, byteConcat.Length);
            //    Test.Close();
            //    Test.Dispose();
            //}
            //else
            //{
            //    Test.Flush();
            //    Test.Open();
            //    Test.Flush();
            //    Test.Write(byteConcat, 0, byteConcat.Length);
            //    Test.Close();
            //    Test.Dispose();
            //}
            SerialPort UART = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            UART.Open();
            UART.Flush();
            UART.Write(byteConcat, 0, byteConcat.Length);
            UART.Flush();
            UART.Close();
            UART.Dispose();
        }
    }
}
    