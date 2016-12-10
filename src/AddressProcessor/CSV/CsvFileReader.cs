using System;
using System.IO;
using CsvHelper;

namespace AddressProcessing.CSV
{
    public class CsvFileReader : ICsvFileReader, IDisposable
    {
        private StreamReader _readerStream;
        private ICsvReader _csvReader;

        public CsvFileReader() { }

        public CsvFileReader(ICsvReader csvReader)
        {
            _csvReader = csvReader;
        }

        public void Open(string fileName)
        {
            OpenFileReader(fileName);
        }

        private void OpenFileReader(string fileName)
        {
            if(_csvReader != null) return;

            _readerStream = File.OpenText(fileName);
            _csvReader = new CsvReader(_readerStream);
            _csvReader.Configuration.Delimiter = "\t";
            _csvReader.Configuration.HasHeaderRecord = false;
            _csvReader.Configuration.ThrowOnBadData = true;
        }

        public bool Read(out string column1, out string column2)
        {
            var isReadOk = _csvReader.Read();
            if (!isReadOk)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            column1 = _csvReader.GetField<string>(0);
            column2 = _csvReader.GetField<string>(1);
            return true;
        }

        public void Close()
        {
            _csvReader?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }
    }
}
