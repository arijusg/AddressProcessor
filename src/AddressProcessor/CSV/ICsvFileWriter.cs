namespace AddressProcessing.CSV
{
    public interface ICsvFileWriter
    {
        void Open(string fileName);
        void Write(string[] columns);
        void Close();
    }
}
