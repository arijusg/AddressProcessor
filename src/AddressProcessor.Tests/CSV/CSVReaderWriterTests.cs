using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AddressProcessing.CSV;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private string _testDataFile;
        private string _testWriteDataFile;
        private List<MailAddressTest> _mailAddressTests;

        [OneTimeSetUp]
        public void Init()
        {
            _testDataFile = $"{AssemblyDirectory}{Path.DirectorySeparatorChar}test_data{Path.DirectorySeparatorChar}contactsSlimVersion.csv";
            _testWriteDataFile = $"{AssemblyDirectory}{Path.DirectorySeparatorChar}test_data{Path.DirectorySeparatorChar}contactsWriteTest.csv";
            SetTestData();
        }

        private void SetTestData()
        {
            _mailAddressTests = new List<MailAddressTest>
            {
                new MailAddressTest("Shelby Macias", @"3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England"),
                new MailAddressTest("Porter Coffey", @"Ap #827-9064 Sapien. Rd.|Palo Alto|Fl.|HM0G 0YR|Scotland"),
                new MailAddressTest("Noelani Ward", @"637-911 Mi Rd.|Monrovia|MB|M5M 6SC|Scotland")
            };
        }

        [SetUp]
        public void Setup()
        {
            DeleteTestWriteCsv();
        }

        private void DeleteTestWriteCsv()
        {
            if (File.Exists(_testWriteDataFile))
            {
                File.Delete(_testWriteDataFile);
            }
        }

        [Test]
        public void ReadCsvFile()
        {

            var reader = new CSVReaderWriter();
            reader.Open(_testDataFile, CSVReaderWriter.Mode.Read);

            var contacts = new List<MailAddressTest>();

            string column1, column2;

            while (reader.Read(out column1, out column2))
            {
                contacts.Add(new MailAddressTest(column1, column2));
            }

            reader.Close();

            CollectionAssert.AreEqual(_mailAddressTests, contacts);
        }

        [Test]
        public void WriteCsvFile()
        {

            var writer = new CSVReaderWriter();
            writer.Open(_testWriteDataFile, CSVReaderWriter.Mode.Write);

            //Write

            foreach (var testAddress in _mailAddressTests)
            {
                writer.Write(testAddress.Name, testAddress.Address);
            }
            writer.Close();

            //Read test File
            var contacts = new List<MailAddressTest>();
            string column1, column2;
            var reader = new CSVReaderWriter();
            reader.Open(_testWriteDataFile, CSVReaderWriter.Mode.Read);
            while (reader.Read(out column1, out column2))
            {
                contacts.Add(new MailAddressTest(column1, column2));
            }

            reader.Close();

            CollectionAssert.AreEqual(_mailAddressTests, contacts);
        }

        //Write mode -> reading file

        private class MailAddressTest
        {
            protected bool Equals(MailAddressTest other)
            {
                return string.Equals(Name, other.Name) && string.Equals(Address, other.Address);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MailAddressTest)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Address != null ? Address.GetHashCode() : 0);
                }
            }

            public MailAddressTest(string name, string address)
            {
                Name = name;
                Address = address;
            }

            public string Name { get; }
            public string Address { get; }
        }

        //This is used to make nCrunch and Resharper play nicely 
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
