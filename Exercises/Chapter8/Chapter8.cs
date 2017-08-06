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

namespace CSharpForFinancialMarkets
{
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

            var di = new DirectoryInfo(basePath);

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

            var xs2 = new XmlSerializer(typeof(BondModel),
                                new[]
                                {
                                    typeof(VasicekModel), typeof(CIRModel)
                                });

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
                                    typeof(WorstOfTwoStrategy),
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
                    new WorstOfTwoStrategy(),
                    new PortfolioStrategy()
                };
                
                xs2.Serialize(s, strategies);
            }

            Assert.IsTrue(File.Exists(persistedBondDefinition));
        }
    }
}
