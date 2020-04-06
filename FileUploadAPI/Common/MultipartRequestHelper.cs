using System.IO;
using Microsoft.Net.Http.Headers;

namespace FileUploadAPI
{
    /// <summary>
    /// Helper class for Multipart request handling.
    /// </summary>
    /// <remarks>
    /// Example:
    /// https://github.com/aspnet/Docs/blob/master/aspnetcore/mvc/models/file-uploads/sample/FileUploadSample/MultipartRequestHelper.cs
    /// </remarks>
    public static class MultipartRequestHelper
    {
        // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        // The spec says 70 characters is a reasonable limit.
        public static string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary);
            if (!boundary.HasValue)
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new InvalidDataException($"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary.Value;
        }
    }
}