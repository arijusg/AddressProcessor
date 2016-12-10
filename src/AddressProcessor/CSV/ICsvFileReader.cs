namespace AddressProcessing.CSV
{
    public interface ICsvFileReader
    {
        void Open(string fileName);
        bool Read(out string column1, out string column2);
        void Close();
    }
}
