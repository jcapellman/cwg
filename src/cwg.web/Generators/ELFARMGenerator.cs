namespace cwg.web.Generators
{
    public class ELFARMGenerator : BaseGenerator
    {
        public override string Name => "ELF (ARM)";

        protected override string SourceName => "sourceELFARM";

        protected override string OutputExtension => "elf";

        public override bool Packable => true;
    }
}