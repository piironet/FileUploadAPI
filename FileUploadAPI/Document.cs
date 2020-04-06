using System.IO;

namespace FileUploadAPI
{
    public class Document
    {
        /// <summary>
        /// File as stream
        /// </summary>
        public Stream File { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File's content type
        /// </summary>
        public string ContentType { get; set; }
    }
}