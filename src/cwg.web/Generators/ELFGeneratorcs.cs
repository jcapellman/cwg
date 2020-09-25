namespace cwg.web.Generators
{
    public class ELFGenerator : BaseGenerator
    {
        public override string Name => "ELF";

        protected override string SourceName => "sourceELF";

        protected override string OutputExtension => "elf";

        public override bool Packable => true;
    }
}