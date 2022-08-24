namespace cwg.web.Generators
{
    public class PE32SignedGenerator : BaseGenerator
    {
        public override string Name => "PE32 SIGNED";

        public override bool Active => true;

        protected override string SourceName => "sourcePESigned";
        
        protected override string CleanSourceName => "sourcePESigned";
        
        protected override string OutputExtension => "exe";

        public override bool Packable => false;
    }
}