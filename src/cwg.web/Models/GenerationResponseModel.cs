using VirusTotalNET.Results;

namespace cwg.web.Models
{
    public class GenerationResponseModel
    {
        public string FileName { get; set; }

        public FileReport FileReportResult { get; set; }
    }
}