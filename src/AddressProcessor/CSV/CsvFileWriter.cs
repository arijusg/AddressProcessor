using System.IO;
using CsvHelper;

namespace AddressProcessing.CSV
{
    public class CsvFileWriter
    {
        private StreamWriter _writerStream;
        private CsvWriter _csvWriter;

        public void Open(string fileName)
        {
            OpenFileWriter(fileName);
        }

        private void OpenFileWriter(string fileName)
        {
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
    }
}
