namespace cwg.web.Generators
{
    public class PE32PlusSignedGenerator : BaseGenerator
    {
        public override string Name => "PE32+ SIGNED";

        public override bool Active => true;

        protected override string SourceName => "sourcePE+Signed";
        
        protected override string CleanSourceName => "sourcePE+Signed";
        
        protected override string OutputExtension => "exe";

        public override bool Packable => true;
    }
}