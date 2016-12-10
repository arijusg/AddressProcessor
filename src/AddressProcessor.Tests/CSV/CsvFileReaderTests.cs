using AddressProcessing.CSV;
using CsvHelper;
using Moq;
using NUnit.Framework;

namespace AddressProcessing.Tests.CSV
{
    [TestFixture]
    public class CsvFileReaderTests
    {
        [Test]
        public void CloseCleansUp()
        {
            var readerMock = new Mock<ICsvReader>();
            readerMock.Setup(x => x.Dispose());

            var sut = new CsvFileReader(readerMock.Object);
            sut.Close();

            readerMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Test]
        public void DisposeCleansUp()
        {
            var readerMock = new Mock<ICsvReader>();
            readerMock.Setup(x => x.Dispose());

            using (var sut = new CsvFileReader(readerMock.Object)) { }

            readerMock.Verify(x => x.Dispose(), Times.Once);
        }
    }
}
