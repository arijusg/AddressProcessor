using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AddressProcessing;
using AddressProcessing.CSV;
using AddressProcessing.Tests.CSV;
using Moq;
using NUnit.Framework;

namespace Csv.Tests
{
    [TestFixture]
    public class CSVReaderWriterTests
    {
        private string _testDataFile;
        private string _testWriteDataFile;
        private List<Contact> _mailAddressTests;

        [OneTimeSetUp]
        public void Init()
        {
            _testDataFile = $"{AssemblyDirectory}{Path.DirectorySeparatorChar}test_data{Path.DirectorySeparatorChar}contactsSlimVersion.csv";
            _testWriteDataFile = $"{AssemblyDirectory}{Path.DirectorySeparatorChar}test_data{Path.DirectorySeparatorChar}contactsWriteTest.csv";
            SetTestData();
        }

        private void SetTestData()
        {
            _mailAddressTests = new List<Contact>
            {
                new Contact("Shelby Macias", @"3027 Lorem St.|Kokomo|Hertfordshire|L9T 3D5|England"),
                new Contact("Porter Coffey", @"Ap #827-9064 Sapien. Rd.|Palo Alto|Fl.|HM0G 0YR|Scotland"),
                new Contact("Noelani Ward", @"637-911 Mi Rd.|Monrovia|MB|M5M 6SC|Scotland")
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
            var actualContacts = new List<Contact>();

            using (var reader = new CSVReaderWriter())
            {
                reader.Open(_testDataFile, CSVReaderWriter.Mode.Read);

                string column1, column2;

                while (reader.Read(out column1, out column2))
                {
                    actualContacts.Add(new Contact(column1, column2));
                }

                reader.Close();
            }

            CollectionAssert.AreEqual(_mailAddressTests, actualContacts);
        }

        [Test]
        public void ReadNotOpenedFile()
        {
            using (var reader = new CSVReaderWriter())
            {
                string column1, column2;
                Assert.Throws<CSVFileNotOpenException>(() => reader.Read(out column1, out column2));
            }
        }

        [Test]
        public void WriteCsvFile()
        {
            //Write test file
            using (var writer = new CSVReaderWriter())
            {
                writer.Open(_testWriteDataFile, CSVReaderWriter.Mode.Write);


                foreach (var testAddress in _mailAddressTests)
                {
                    writer.Write(testAddress.Name, testAddress.Address);
                }
                writer.Close();
            }

            //Read test file
            var contacts = new List<Contact>();

            using (var reader = new CSVReaderWriter())
            {
                reader.Open(_testWriteDataFile, CSVReaderWriter.Mode.Read);
                string column1, column2;
                while (reader.Read(out column1, out column2))
                {
                    contacts.Add(new Contact(column1, column2));
                }

                reader.Close();
            }

            CollectionAssert.AreEqual(_mailAddressTests, contacts);
        }

        [Test]
        public void WriteNotOpenedFile()
        {
            using (var writer = new CSVReaderWriter())
                Assert.Throws<CSVFileNotOpenException>(() => writer.Write("hello"));
        }

        [Test]
        public void CloseCleansReader()
        {
            var readerMock = new Mock<ICsvFileReader>();
            readerMock.Setup(x => x.Close());

            var sut = new CSVReaderWriter(readerMock.Object, null);
            sut.Close();
            readerMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void DisposeCleansReader()
        {
            var readerMock = new Mock<ICsvFileReader>();
            readerMock.Setup(x => x.Close());

            using (var sut = new CSVReaderWriter(readerMock.Object, null)) { }

            readerMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void CloseCleansWriter()
        {
            var writerMock = new Mock<ICsvFileWriter>();
            writerMock.Setup(x => x.Close());

            var sut = new CSVReaderWriter(null, writerMock.Object);
            sut.Close();
            writerMock.Verify(x => x.Close(), Times.Once);
        }

        [Test]
        public void DisposeCleansWriter()
        {
            var writerMock = new Mock<ICsvFileWriter>();
            writerMock.Setup(x => x.Close());

            using (var sut = new CSVReaderWriter(null, writerMock.Object)) { }

            writerMock.Verify(x => x.Close(), Times.Once);
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
