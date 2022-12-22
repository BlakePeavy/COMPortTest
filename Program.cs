using System;
using System.IO.Ports;

// https://www.nuget.org/packages/System.IO.Ports/7.0.0/
namespace CANConsole
{
    class Program
    {
        static SerialPort _serialPort;
        static void Main(string[] args)
        {
            // Display ports
            Console.WriteLine("Available  Ports:");
            foreach (var p in SerialPort.GetPortNames())
            {
                Console.WriteLine($"{p}");
            }

            // Enter selected port from above.
            Console.WriteLine("Enter COM port:");
            string portName = Console.ReadLine();
            if(!SerialPort.GetPortNames().Contains(portName))
            {
                Console.WriteLine("Cannot find that port");
                return;
                // Exit if not found.
            }
            Console.WriteLine("Buad rate:");
            int rate = int.Parse(Console.ReadLine());
            // Set up the port based on entered information.
            _serialPort = new SerialPort(portName, rate, Parity.None, 8, StopBits.One);
            // Open the port
            _serialPort.Open();

            Console.WriteLine("Enter message bytes seperated by space. Ex. 0x01 0x02");
            // Read the entered data and seperate it based on spaces.
            string[] data = Console.ReadLine().Split(" ");
            // Now loop through the string and populate data. 
            // This could be done in one line, but to show how it's done I've choosen this method.
            byte[] COMMessage = new byte[data.Length];
            for (int i = 0; i < COMMessage.Length; i++)
            {
                COMMessage[i] = Convert.ToByte(data[i]);
            }
            // Write the message to the port
            _serialPort.Write(COMMessage, 0, COMMessage.Length);

            // Read the response message from the port.
            byte[] response = new byte[8];
            _serialPort.Read(response, 0, response.Length);

            // Read out the response
            Console.WriteLine($"Response:{Environment.NewLine}");
            foreach (var b in response)
                Console.Write($"{b} ");
            Console.WriteLine("");

            // All done, close, or keep going..
            _serialPort.Close();
        }
    }
}