using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRCcs
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b, c, d;
            int N = 8;
            byte[] xs = new byte[N];
            for (a = 0; a < N; ++a)
                xs[a] = (byte)a;
            uint crc = CRC.crc32a(xs);
            for (a = 1; a < 256; ++a)
            {
                xs[0] = (byte)a;
                uint crc2 = CRC.crc32a(xs);
                if (crc != crc2)
                    Console.WriteLine($"  {crc2:X08} /= {crc:X08}");
                else
                    Console.WriteLine($"* {crc2:X08} == {crc:X08}");
            }

            uint n = 0xffffffff;
            int t0, t1;
            t0 = Environment.TickCount;
            for (a = 255; a >= 0; --a)
                for (b = 255; b >= 0; --b)
                    for (c = 255; c >= 0; --c)
                        for (d = 255; d >= 0; --d)
                        {
                            n--;
                            xs[0] = (byte)a;
                            xs[1] = (byte)b;
                            xs[2] = (byte)c;
                            xs[3] = (byte)d;
                            uint crc2 = CRC.crc32b(xs);
                            if (crc == crc2)
                            {
                                Console.WriteLine($"* {crc2:X08} == {crc:X08} : a={a:X02} b={b:X02} c={c:X02} d={d:X02}");
                            }
                            t1 = Environment.TickCount;
                            if (t1 - t0 > 1000)
                            {
                                Console.Write($"\r{n:X08}\r");
                                t0 = t1;
                            }
                        }

        }
    }
}
