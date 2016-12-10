using System;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.

        Trade offs/comments

      - I have not implemented my own csv reader, it would have been wasteful. CsvHelper is high quality open source library.
      - I tested CSVReaderWriter functionality, not implementation.
      - "Unknown file mode for check and throws exception" - not relevant as we passing in a enum. If enum gets more values, tests should be written around it. We do not code defensively..
      - Method Read(string column, string column2) is redundant, string is immutable type
      - Reading returns two columns only - not flexible implementation. Should return column array or be mapped to objects
      - Exception handling is left for AddressFileProcessor.

    */

    public class CSVReaderWriter : IDisposable
    {
        private ICsvFileReader _csvFileReader;
        private ICsvFileWriter _csvFileWriter;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public CSVReaderWriter() { }

        public CSVReaderWriter(ICsvFileReader reader, ICsvFileWriter writer)
        {
            _csvFileReader = reader;
            _csvFileWriter = writer;
        }

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
            if (_csvFileReader == null)
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
