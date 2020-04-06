using System;
using System.Threading.Tasks;
using FileUploadAPI.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace FileUploadAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class DocumentsController : ControllerBase
    {
        private const int FileMaxCount = 1;
        private readonly ILogger<DocumentsController> _logger;
        //private readonly IDocumentService _documentService;

        public DocumentsController(ILogger<DocumentsController> logger /*, IDocumentService documentService*/)
        {
            _logger = logger;
            //_documentService = _documentService;
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post()
        {
            _logger.LogInformation("Starting to store file and metadata...");

            if (!Request.HasFormContentType)
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}.");
            }

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), new FormOptions().MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var document = new Document();
       
            for (var i = 0; i < FileMaxCount; i++)
            {
                var section = await reader.ReadNextSectionAsync();

                if (section == null)
                {
                    continue;
                }

                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (!hasContentDispositionHeader || !contentDisposition.IsFileDisposition())
                {
                    return BadRequest("Content-Disposition header is missing or is not valid (e.g. form-data and/or filename is missing).");
                }

                if (string.IsNullOrWhiteSpace(section.ContentType))
                {
                    return BadRequest($"Content-Type header is missing. File: {contentDisposition.FileName.Value}.");
                }
                
                document.File = section.Body;
                document.ContentType = section.ContentType;
                document.Name = contentDisposition.FileName.Value;
            }


            if (document.File == null)
            {
                return BadRequest("File is missing.");
            }
            
            try
            {
                // Do your stuff here

                //await _documentService.UpsertAsync(document);

                _logger.LogInformation($"File stored. File name: {document.Name}");
                return Ok();
            }
            catch (Exception ex)
            {
                // Do you error handling here
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}