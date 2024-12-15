namespace WebAppCore.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; } // Holds the binary content
        public DateTime UploadedAt { get; set; }

        //public string FilePath { get; set; }

    }
}
