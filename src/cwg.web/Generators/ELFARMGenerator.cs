namespace cwg.web.Generators
{
    public class ELFARMGenerator : BaseGenerator
    {
        public override string Name => "ELF (ARM)";

        public override bool Active => true;

        protected override string SourceName => "sourceELFARM";

        protected override string CleanSourceName => "sourceCleanELFarm64";

        protected override string OutputExtension => "elf";

        public override bool Packable => true;
    }
}