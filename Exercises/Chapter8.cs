using Library;
using Library.Bonds;
using Library.Dates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using static System.Math;

namespace CSharpForFinancialMarkets
{
    public class OptionData
    {
        public string ID;

        // Member data public for convenience
        public double r;        // Interest rate
        public double sig;      // Volatility
        public double K;        // Strike price
        public double T;        // Expiry date
        public double b;        // Cost of carry

        public string otyp;     // Option name (call, put)
    }

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

    public interface ITwoFactorPayoff
    { 
        // The unambiguous specification of two-factor payoff contracts
        // This interfaces can be specialised to various underlyings
        double Payoff(double factorI1, double factorII);
    }

    public abstract class MultiAssetPayoffStrategy : ITwoFactorPayoff
    {
        public abstract double Payoff(double S1, double S2);
    }

    public class ExchangeStrategy : MultiAssetPayoffStrategy
    {
        // No member data
        public ExchangeStrategy() { }

        public override double Payoff(double S1, double S2)
        {
            return Max(S1 - S2, 0.0); // a1S1 - a2S2 in general
        }
    }

    public class RainbowStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// Comparisons.Max (1) or Comparisons.Min (!1) of 2 assets
        /// </summary>
        public int type { get; set; }

        public RainbowStrategy(int cp, double strike, int DMinDMax)
        {
            K = strike;
            w = cp;
            type = DMinDMax;
        }

        public override double Payoff(double S1, double S2)
        {
            if (type == 1)  // Comparisons.Max
                return Max(w * Max(S1, S2) - w * K, 0.0);

            return Max(w * Min(S1, S2) - w * K, 0.0);
        }
    }

    /// <summary>
    /// 2-asset basket option payoff
    /// </summary>
    public class BasketStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        ///  +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// w1 + w2 = 1
        /// </summary>
        public double w1 { get; set; }

        /// <summary>
        /// w1 + w2 = 1
        /// </summary>
        public double w2 { get; set; } 

        // All public classes need default ructor
        public BasketStrategy()
        {
            K = 95.0; w = +1; w1 = 0.5; w2 = 0.5;
        }

        public BasketStrategy(double strike, int cp, double weight1, double weight2)
        {
            K = strike;
            w = cp;
            w1 = weight1;
            w2 = weight2;
        }

        public override double Payoff(double S1, double S2)
        {
            double sum = w1 * S1 + w2 * S2;
            return Max(w * (sum - K), 0.0);
        }
    }

    public class BestWorstStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; } 

        /// <summary>
        /// +1 Best, -1 Worst
        /// </summary>
        public double w { get; set; }      

        public BestWorstStrategy()
        {

        }

        public BestWorstStrategy(double cash, double BestWorst)
        {
            K = cash;
            w = BestWorst;
        }

        public override double Payoff(double S1, double S2)
        {
            if (w == 1) // Best
                return Max(Max(S1, S2), K);

            return Min(Min(S1, S2), K);
        }
    }

    public class QuotientStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        public QuotientStrategy()
        {

        }

        public QuotientStrategy(double strike, int cp)
        {
            K = strike;
            w = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            return Max(w * (S1 / S2) - w * K, 0.0);
        }
    }

    public class QuantoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike in foreign currency
        /// </summary>
        public double Kf { get; set; }

        /// <summary>
        /// Fixed exchange rate
        /// </summary>
        public double fer { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        public QuantoStrategy()
        {

        }

        public QuantoStrategy(double foreignStrike, int cp, double forExchangeRate)
        {
            Kf = foreignStrike;
            w = cp;
            fer = forExchangeRate;
        }

        public override double Payoff(double S1, double S2)
        {
            return fer * Max(w * S1 - w * Kf, 0.0);
        }
    }

    public class SpreadStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// a > 0, b < 0
        /// </summary>
        public double a { get; set; }

        /// <summary>
        /// a > 0, b < 0
        /// </summary>
        public double b { get; set; }

        public SpreadStrategy()
        {

        }

        public SpreadStrategy(int cp)
        {
            K = 0.0;
            w = cp;
            a = 1.0;
            b = -1.0;
        }

        public SpreadStrategy(int cp, double strike, double A, double B)
        {
            K = strike;
            w = cp;
            a = A;
            b = B;
        }

        public override double Payoff(double S1, double S2)
        {
            double sum = a * S1 + b * S2;
            return Max(w * (sum - K), 0.0);
        }
    }

    public class DualStrikeStrategy : MultiAssetPayoffStrategy
    {
        public double K1 { get; set; }

        public double K2 { get; set; }

        public int w1 { get; set; }

        /// <summary>
        /// calls or puts
        /// </summary>
        public int w2 { get; set; } 

        public DualStrikeStrategy()
        {

        }

        public DualStrikeStrategy(double strike1, double strike2, int cp1, int cp2)
        {
            K1 = strike1;
            K2 = strike2;
            w1 = cp1;
            w2 = cp2;
        }

        public override double Payoff(double S1, double S2)
        {
            return Max(Max(w1 * (S1 - K1), w2 * (S2 - K2)), 0.0);
        }
    }

    public class OutPerformanceStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Values of underlyings at maturity
        /// </summary>
        public double I1 { get; set; } 

        /// <summary>
        /// Values of underlyings at maturity
        /// </summary>
        public double I2 { get; set; }

        /// <summary>
        /// Call +1 or put -1
        /// </summary>
        public int w { get; set; }  

        /// <summary>
        /// Strike rate of option
        /// </summary>
        public double K { get; set; }  

        public OutPerformanceStrategy()
        {

        }

        public OutPerformanceStrategy(double currentRate1, double currentRate2, int cp, double strikeRate)
        {
            I1 = currentRate1;
            I2 = currentRate2;
            w = cp;
            K = strikeRate;
        }

        public override double Payoff(double S1, double S2)
        {
            return Max(w * ((I1 / S1) - (I2 / S2)) - w * K, 0.0);
        }
    }

    /// <summary>
    /// Best of 2 options
    /// </summary>
    public class BestofTwoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }      

        public BestofTwoStrategy()
        {
            K = 95.0;
            w = +1;
        }

        public BestofTwoStrategy(double strike, int cp)
        {
            K = strike; w = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            double max = Max(S1, S2);
            return Max(w * (max - K), 0.0);
        }
    }

    /// <summary>
    /// Best of 2 options
    /// </summary>
    public class WorstofTwoStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }       

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }         

        // All public classes need default ructor
        public WorstofTwoStrategy()
        {
            K = 95.0;
            w = +1;
        }

        public WorstofTwoStrategy(double strike, int cp)
        {
            K = strike;
            w = cp;
        }

        public override double Payoff(double S1, double S2)
        {
            double min = Min(S1, S2);
            return Max(w * (min - K), 0.0);
        }
    }

    /// <summary>
    /// Portfolio option
    /// </summary>
    public class PortfolioStrategy : MultiAssetPayoffStrategy
    {
        /// <summary>
        /// Strike
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// +1 call, -1 put
        /// </summary>
        public int w { get; set; }

        /// <summary>
        /// quantities of each underlying
        /// </summary>
        public double n1 { get; set; }

        /// <summary>
        /// quantities of each underlying
        /// </summary>
        public double n2 { get; set; }              

        // All public classes need default ructor
        public PortfolioStrategy()
        {
            K = 95.0;
            w = +1;
            n1 = n2 = 1;
        }

        public PortfolioStrategy(int N1, int N2, double strike, int cp)
        {
            K = strike; w = cp;
            n1 = N1; n2 = N2;
        }

        public override double Payoff(double S1, double S2)
        {
            double min = Min(S1, S2);
            return Max(w * (n1 * S1 + n2 * S2 - K), 0.0);
        }
    }

    [TestClass]
    public class Chapter8
    {
        [TestMethod]
        public void C8_10_1_MemoryStreams()
        {
            // Create FileStream on new file with read/write access.
            // Note that after create we use a Stream reference to access 
            // the FileStream thus we can easily replace it with another kind of stream.
            //using (Stream s = new FileStream("data.tmp", FileMode.Create, FileAccess.ReadWrite))
            using (Stream s = new MemoryStream(10)) // We can use a MemoryStream instead
            {
                // If we can write, write something
                if (s.CanWrite)
                {
                    byte[] buffer = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };   // Data to write
                    s.Write(buffer, 0, buffer.Length);              // Write it
                    s.Flush();                                      // Flush the buffer
                }

                // Set the stream position to the beginning
                if (s.CanSeek) s.Seek(0, SeekOrigin.Begin);

                // If we can read, read it
                if (s.CanRead)
                {
                    byte[] buffer = new byte[s.Length];             // Create buffer with file length
                    int count = s.Read(buffer, 0, buffer.Length);       // Read the whole file

                    // Print byte count
                    Console.WriteLine("{0} bytes read.", count);

                    // Print every byte in the buffer
                    foreach (byte b in buffer)
                        Console.WriteLine(b.ToString() + ", ");
                }
            }
        }

        [TestMethod]
        public void C8_10_2_AppendingDataToAFile()
        {
            // Create FileStream on new file with read/write access.
            // Note that after create we use a Stream reference to access 
            // the FileStream thus we can easily replace it with another kind of stream.
            using (Stream s = new FileStream("data.tmp", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (s.CanSeek)
                    s.Seek(0, SeekOrigin.End);

                // If we can write, write something
                if (s.CanWrite)
                {
                    byte[] buffer = { 99 };   // Data to write
                    s.Write(buffer, 0, buffer.Length);              // Write it
                    s.Flush();                                      // Flush the buffer
                }

                // Set the stream position to the beginning
                if (s.CanSeek)
                    s.Seek(0, SeekOrigin.Begin);

                // If we can read, read it
                if (s.CanRead)
                {
                    byte[] buffer = new byte[s.Length];             // Create buffer with file length
                    int count = s.Read(buffer, 0, buffer.Length);       // Read the whole file

                    // Print byte count
                    Console.WriteLine("{0} bytes read.", count);

                    // Print every byte in the buffer
                    foreach (byte b in buffer)
                        Console.WriteLine(b.ToString() + ", ");
                }
            }
        }

        [TestMethod]
        public void C8_10_3_A_CreatingPersistentData()
        {
            OptionData od = new OptionData()
            {
                ID = "Test1",
                r = 0.05,
                sig = 0.20,
                K = 100,
                T = 365,
                b = 0.01,
                otyp = "Call"
            };

            using (var backingStore = new MemoryStream())
            {
                var bw = new BinaryWriter(backingStore);
                od.SaveData(bw);
                    
                backingStore.Position = 0;

                //Change some variables
                od.r = 0.1;

                var br = new BinaryReader(backingStore);
                od.ReadData(br);
            }
            
            Assert.AreEqual(od.r, 0.05);
        }

        [TestMethod]
        public void C8_10_3_B_CreatingPersistentData()
        {
            /*
            The extension methods is optimal as the Option is now 'Persistance Ignorant'. You can change the way the Option is stored without touching the option class.

            See more here: http://deviq.com/persistence-ignorance/
            */
        }

        [TestMethod]
        public void C8_10_4_A_ApplicationConfiguration()
        {
            //Check a drive exists and that it is ready
            var drives = DriveInfo.GetDrives();

            foreach(var drive in drives)
            {
                if(drive.Name.Equals(@"C:\", StringComparison.CurrentCultureIgnoreCase))
                {
                    Assert.IsTrue(drive.IsReady);
                }
            }
        }

        [TestMethod]
        public void C8_10_4_B_ApplicationConfiguration()
        {
            //Create a directory
            var testDirectory = "Test";

            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), testDirectory));

            Assert.IsTrue(Directory.Exists(Path.Combine(Path.GetTempPath(), testDirectory)));
        }

        [TestMethod]
        public void C8_10_4_C_ApplicationConfiguration()
        {
            var testDirectory = @"Test\SubTest";

            Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), testDirectory));

            Assert.IsTrue(Directory.Exists(Path.Combine(Path.GetTempPath(), testDirectory)));
        }

        [TestMethod]
        public void C8_10_4_D_ApplicationConfiguration()
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(Path.GetTempPath()));

            void SearchDirectory(DirectoryInfo directory)
            {
                foreach(var dir in directory.GetDirectories())
                {
                    Console.WriteLine(dir.FullName);
                    Console.WriteLine("   Creation Time = {0}", dir.CreationTime);
                    Console.WriteLine("   Last Access Time = {0}", dir.LastAccessTime);
                    Console.WriteLine("   Last Write Time = {0}", dir.LastWriteTime);

                    SearchDirectory(dir);
                }
            }

            SearchDirectory(di);
        }

        [TestMethod]
        public void C8_10_4_E_ApplicationConfiguration()
        {
            var testDirectory = "Test";
            var basePath = Path.Combine(Path.GetTempPath(), testDirectory);

            Directory.CreateDirectory(basePath);
            for(int i = 0; i < 100; i++)
            {
                var x = Path.Combine(basePath, i.ToString());
                if (!Directory.Exists(x))
                    Directory.CreateDirectory(x);
            }

            DirectoryInfo di = new DirectoryInfo(basePath);

            void DeleteSubDirectories(DirectoryInfo directory)
            {
                foreach (var dir in directory.GetDirectories().ToList())
                {
                    DeleteSubDirectories(dir);

                    dir.Delete();
                }
            }

            DeleteSubDirectories(di);
        }

        [TestMethod]
        public void C8_10_5_E_ReportingOnADirectoriesContents()
        {
            var basePath = Path.Combine(Path.GetTempPath());

            DirectoryInfo di = new DirectoryInfo(basePath);

            long size = 0;
            long count = 0;
            void Sum(DirectoryInfo directory)
            {
                foreach (var dir in directory.GetDirectories().ToList())
                {
                    dir.GetFiles().ToList().ForEach(c => {
                        size += c.Length;
                        count++;
                        });

                    Sum(dir);
                }
            }

            Sum(di);

            Console.WriteLine("Size = {0}", size);
            Console.WriteLine("Count = {0}", count);
        }

        [TestMethod]
        public void C8_10_6_SerialisationAndCIRModel()
        {
            double r = 0.05;
            double kappa = 0.15;
            double vol = 0.01;
            double theta = r;

            BondModel vasicek = new CIRModel(kappa, theta, vol, r);

            XmlSerializer xs2 = new XmlSerializer(typeof(BondModel),
                                new Type[] { typeof(VasicekModel), typeof(CIRModel) });

            var persistedBondDefinition = Path.Combine(Path.GetTempPath(), "Bond.xml");
            using (Stream s = File.Create(persistedBondDefinition))
            {
                xs2.Serialize(s, vasicek);
            }

            Assert.IsTrue(File.Exists(persistedBondDefinition));
        }

        [TestMethod]
        public void C8_10_7_SerialisationAndPayoffHierarchies()
        {
            //Note: Binary Serialization is NOT available in .net Core 1.1 (it's coming back in .net Standard 2.0 later in 2017 apparently - https://github.com/dotnet/corefx/pull/10144)
            //as such, I'm just using the XmlSerializer again for this exercise.

            //Since the XmlSerializer only serializes public data, the Strategy classes have been modified to expose all their data as properties.

            XmlSerializer xs2 = new XmlSerializer(typeof(MultiAssetPayoffStrategy[]),
                                new Type[] 
                                {
                                    typeof(BasketStrategy),
                                    typeof(BestWorstStrategy),
                                    typeof(QuotientStrategy),
                                    typeof(QuantoStrategy),
                                    typeof(SpreadStrategy),
                                    typeof(DualStrikeStrategy),
                                    typeof(OutPerformanceStrategy),
                                    typeof(BestofTwoStrategy),
                                    typeof(WorstofTwoStrategy),
                                    typeof(PortfolioStrategy)
                                });

            var persistedBondDefinition = Path.Combine(Path.GetTempPath(), "Strategies.xml");
            using (Stream s = File.Create(persistedBondDefinition))
            {
                var strategies = new MultiAssetPayoffStrategy[] {

                    new BasketStrategy(),
                    new BestWorstStrategy(10, 10),
                    new QuotientStrategy(100, 10),
                    new QuantoStrategy(1.45, 10, 1),
                    new SpreadStrategy(10, 50, 1,2),
                    new DualStrikeStrategy(50, 60, 10, 02),
                    new OutPerformanceStrategy(10, 20, 30, 50),
                    new BestofTwoStrategy(),
                    new WorstofTwoStrategy(),
                    new PortfolioStrategy()
                };
                
                xs2.Serialize(s, strategies);
            }

            Assert.IsTrue(File.Exists(persistedBondDefinition));
        }
    }
}
