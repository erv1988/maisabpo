using System.Diagnostics;
using static Kernel32;

namespace Codes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int bytesRW = 0;
            int N;
            byte[] buffer = new byte[64*1024*1024];
            Random random = new Random((int)DateTime.Now.Ticks);

            bool successfull = true;
            Console.Write("Ptr=");
            string ptr = Console.ReadLine();
            UInt64 lpAddress = UInt64.Parse(ptr, System.Globalization.NumberStyles.HexNumber);// 0x0000011513ED1048;

            Process process = Process.GetProcessesByName("loop")[0];
            IntPtr processHandle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);

            do
            {
                ReadProcessMemory((int)processHandle, lpAddress, buffer, buffer.Length, ref bytesRW);
                Console.WriteLine($"Read {bytesRW} from {lpAddress:X8}");
                for (int i = 0; i < 32; ++i)
                    Console.Write("{0:X2} ", buffer[i]);
                Console.WriteLine();
                
                N = random.Next(buffer.Length);
                buffer[0] = (byte)~buffer[N];
                Console.WriteLine($"Corrupt {N}th byte");
                for (int i = 0; i < 32; ++i)
                    Console.Write("{0:X2} ", buffer[i]);
                Console.WriteLine();

                WriteProcessMemory((int)processHandle, lpAddress, buffer, buffer.Length, ref bytesRW);
                Console.WriteLine($"Wrote {bytesRW} to {lpAddress:X8}");
                Console.WriteLine("Press key to continue");
                Console.ReadKey();
            } while (successfull);

        }
    }
}