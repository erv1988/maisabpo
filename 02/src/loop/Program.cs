namespace loop
{
    internal class Program
    {
        public static byte[] memory = new byte[64*1024*1024];
        static unsafe void Main(string[] args)
        {
            // Инициализация
            Random random = new Random((int)DateTime.Now.Ticks);

            fixed (byte* ptr = memory)
            {
                UIntPtr uIntPtr = new UIntPtr(ptr);
                Console.WriteLine($"Address = {uIntPtr:X16}");
            }
            Console.WriteLine("press key for continue");
            Console.ReadKey();

            // цикл
            while (true)
            {
                for (int i = 0; i < 16; i++)
                    memory[i] = 0xab;
                for (int i = 16; i < memory.Length-16; ++i)
                    memory[i] = (byte)random.Next();

                uint chksum_0 = chksum_xor(memory);
                Console.WriteLine($"Chksum before= {chksum_0:X8}");

                for (int i = 0; i < 32; ++i)
                    Console.Write("{0:X2} ", memory[i]);
                Console.WriteLine();

                // задержка на 1 сек
                System.Threading.Thread.Sleep(1000);
                uint chksum_1 = chksum_xor(memory);
                Console.WriteLine($"Chksum after=  {chksum_1:X8}");

                for (int i = 0; i < 32; ++i)
                    Console.Write("{0:X2} ", memory[i]);
                Console.WriteLine();

                if (chksum_0 != chksum_1)
                    Console.WriteLine("Corrupt!!!");
                // задержка на 1 сек
                System.Threading.Thread.Sleep(1000);
            }
        }


        public static uint chksum_xor(byte[] data)
        {
            uint chk = 0;
            for (int n = 0; n < data.Length; ++n)
            {
                chk ^= data[n];
            }

            return chk;
        }
    }
}