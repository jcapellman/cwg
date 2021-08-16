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

        public ThreatLevels ThreatLevelEnum => (ThreatLevels)Enum.Parse(typeof(ThreatLevels), ThreatLevel);

        public string[] SelectableThreatLevels { get; private set; }

        public bool Repack { get; set; }

        public GenerationRequestModel()
        {
            NumberToGenerate = 1;
            Bosartige = true;
            Repack = false;
            ThreatLevel = ThreatLevels.MALICIOUS.ToString();

            SelectableThreatLevels = Enum.GetNames(typeof(ThreatLevels));
        }
    }
}