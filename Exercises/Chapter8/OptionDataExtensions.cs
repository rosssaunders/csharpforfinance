using System.IO;

namespace CSharpForFinancialMarkets
{
    public static class OptionDataExtensions
    {
        public static void SaveData(this OptionData option, BinaryWriter bw)
        {
            bw.Write(option.r);
            bw.Write(option.sig);
            bw.Write(option.K);
            bw.Write(option.T);
            bw.Write(option.b);
            bw.Write(option.otyp);

            bw.Flush();     // clear BinaryWriter buffer
        }

        public static void ReadData(this OptionData option, BinaryReader br)
        {
            option.r = br.ReadDouble();
            option.sig = br.ReadDouble();
            option.K = br.ReadDouble();
            option.T = br.ReadDouble();
            option.b = br.ReadDouble();
            option.otyp = br.ReadString();
        }
    }
}