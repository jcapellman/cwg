namespace cwg.web.Generators
{
    public class PE32SignedGenerator : BaseGenerator
    {
        public override string Name => "PE32 SIGNED";

        protected override string SourceName => "sourcePESIGNED.exe";

        protected override string OutputExtension => "exe";
    }
}