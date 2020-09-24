using System;

using cwg.web.Enums;

namespace cwg.web.Data
{
    public class GenerationRequestModel
    {
        public string FileType { get; set; }

        public int NumberToGenerate { get; set; }

        public bool Bosartige { get; set; }

        public string Injection { get; set; }

        public string ThreatLevel { get; set; }

        public string[] SelectableThreatLevels { get; private set; }

        public GenerationRequestModel()
        {
            NumberToGenerate = 1;
            Bosartige = true;
            ThreatLevel = ThreatLevels.ABNORMAL.ToString();

            SelectableThreatLevels = Enum.GetNames(typeof(ThreatLevels));
        }
    }
}