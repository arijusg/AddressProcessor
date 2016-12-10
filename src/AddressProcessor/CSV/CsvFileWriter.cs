using System;
using System.IO;
using CsvHelper;

namespace AddressProcessing.CSV
{
    public class CsvFileWriter : ICsvFileWriter, IDisposable
    {
        private StreamWriter _writerStream;
        private ICsvWriter _csvWriter;

        public CsvFileWriter() { }

        public CsvFileWriter(ICsvWriter csvWriter)
        {
            _csvWriter = csvWriter;
        }

        public void Open(string fileName)
        {
            OpenFileWriter(fileName);
        }

        private void OpenFileWriter(string fileName)
        {
            if (_csvWriter != null) return;
             
            FileInfo fileInfo = new FileInfo(fileName);
            _writerStream = fileInfo.CreateText();
            _csvWriter = new CsvWriter(_writerStream);
            _csvWriter.Configuration.Delimiter = "\t";
            _csvWriter.Configuration.HasHeaderRecord = false;
            _csvWriter.Configuration.ThrowOnBadData = true;
        }

        public void Write(string[] columns)
        {
            foreach (string column in columns)
            {
                _csvWriter.WriteField(column);
            }
            _csvWriter.NextRecord();
        }

        public void Close()
        {
            _csvWriter?.Dispose();
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
