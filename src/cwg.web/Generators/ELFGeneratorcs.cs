namespace cwg.web.Generators
{
    public class ELFGenerator : BaseGenerator
    {
        public override string Name => "ELF (x86)";

        public override bool Active => true;

        protected override string SourceName => "sourceELF";

        protected override string CleanSourceName => "sourceCleanELFx64";

        protected override string OutputExtension => "elf";

        public override bool Packable => true;
    }
}