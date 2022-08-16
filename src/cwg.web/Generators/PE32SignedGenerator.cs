namespace cwg.web.Generators
{
    public class PE32SignedGenerator : BaseGenerator
    {
        public override string Name => "PE32 SIGNED";

        public override bool Active => true;

        protected override string SourceName => "sourcePESIGNED.exe";
        
        protected override string CleanSourceName => "sourcePESIGNED.exe";
        
        protected override string OutputExtension => "exe";

        public override bool Packable => true;
    }
}