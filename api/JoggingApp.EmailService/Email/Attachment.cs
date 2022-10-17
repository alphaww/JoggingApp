namespace JoggingApp.EemailService
{
    public class Attachment
    {
        public Attachment(string fileName, byte[] data)
        {
            FileName = fileName;
            Data = data;
        }
        public string FileName { get; private set; }
        public byte[] Data { get; private set; }
    }
}
