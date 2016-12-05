using System;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter
    {
        private CsvFileReader _csvFileReader;
        private CsvFileWriter _csvFileWriter;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
        {
            if (mode == Mode.Read)
            {
                _csvFileReader = new CsvFileReader();
                _csvFileReader.Open(fileName);
            }
            if (mode == Mode.Write)
            {
                _csvFileWriter = new CsvFileWriter();
                _csvFileWriter.Open(fileName);
            }
        }

        public bool Read(out string column1, out string column2)
        {
            if(_csvFileReader == null)
                throw new CSVFileNotOpenException();

            return _csvFileReader.Read(out column1, out column2);
        }

        public void Write(params string[] columns)
        {
            if (_csvFileWriter == null)
                throw new CSVFileNotOpenException();

            _csvFileWriter.Write(columns);
        }

        public void Close()
        {
            _csvFileReader?.Close();
            _csvFileWriter?.Close();
        }
    }
}
