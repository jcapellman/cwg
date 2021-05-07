namespace cwg.web.Generators
{
    public class PE32Generator : BaseGenerator
    {
        public override string Name => "PE32";

        protected override string SourceName => "sourcePE.exe";

        protected override string CleanSourceName => "sourceCleanPE32.exe";

        protected override string OutputExtension => "exe";

        public override bool Packable => true;
    }
}