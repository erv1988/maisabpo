using ReedSolomon.NET.Loops;

namespace RS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var codingLoop in CodingLoopHelpers.AllCodingLoops)
            {
                var codec = new ReedSolomon.NET.ReedSolomon(5, 5, codingLoop);
                var shards = new byte[10][];
                shards[0] = new byte[] { 0, 1 };
                shards[1] = new byte[] { 4, 5 };
                shards[2] = new byte[] { 2, 3 };
                shards[3] = new byte[] { 6, 7 };
                shards[4] = new byte[] { 8, 9 };
                shards[5] = new byte[2];
                shards[6] = new byte[2];
                shards[7] = new byte[2];
                shards[8] = new byte[2];
                shards[9] = new byte[2];

                codec.EncodeParity(shards, 0, 2);

                //shards[5].ShouldBe(new byte[] { 12, 13 });
                //shards[6].ShouldBe(new byte[] { 10, 11 });
                //shards[7].ShouldBe(new byte[] { 14, 15 });
                //shards[8].ShouldBe(new byte[] { 90, 91 });
                //shards[9].ShouldBe(new byte[] { 94, 95 });

                var a = codec.IsParityCorrect(shards, 0, 2);//.ShouldBeTrue();
                shards[8][0] += 1;
                var b = codec.IsParityCorrect(shards, 0, 2);//.ShouldBeFalse();
            }
        }
    }
}