namespace cwg.web.Generators
{
    public class PE32EDRGenerator : BaseGenerator
    {
        public override string Name => "PE32 (EDR)";

        public override bool Active => true;

        protected override string SourceName => "sourcePE32EDR.exe";

        protected override string CleanSourceName => "sourceCleanPE32.exe";

        protected override string OutputExtension => "exe";

        public override bool Packable => true;
    }
}
