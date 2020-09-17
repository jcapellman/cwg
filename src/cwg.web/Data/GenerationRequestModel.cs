namespace cwg.web.Data
{
    public class GenerationRequestModel
    {
        public string FileType { get; set; }

        public int NumberToGenerate { get; set; }

        public bool Bosartige { get; set; }

        public GenerationRequestModel()
        {
            NumberToGenerate = 1;
            Bosartige = true;
        }
    }
}