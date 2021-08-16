namespace cwg.web.Generators
{
    public class PE32withPSGenerator : BaseGenerator
    {
        public override string Name => "PE32 with PS Embedded";

        public override bool Active => false;

        protected override string SourceName => "sourcePEwithPS.exe";
        
        protected override string CleanSourceName => "sourcePEwithPS.exe";

        protected override string OutputExtension => "exe";

        public override bool Packable => false;
    }
}