using AddressProcessing.CSV;
using CsvHelper;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvFileWriterrTests
    {
        [Test]
        public void CloseCleansUp()
        {
            var csvWriterMock = new Mock<ICsvWriter>();
            csvWriterMock.Setup(x => x.Dispose());

            var sut = new CsvFileWriter(csvWriterMock.Object);
            sut.Close();

            csvWriterMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Test]
        public void DisposeCleansUp()
        {
            var writerMock = new Mock<ICsvWriter>();
            writerMock.Setup(x => x.Dispose());

            using (var sut = new CsvFileWriter(writerMock.Object)) { }

            writerMock.Verify(x => x.Dispose(), Times.Once);
        }
    }
}
