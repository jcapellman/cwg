namespace cwg.web.Generators
{
    public class DotnetLinuxArm64 : BaseGenerator
    {
        public override string Name => "DotnetLinuxArm64";

        public override bool Active => true;

        protected override string SourceName => "sourceDotnetLinuxArm64";

        protected override string CleanSourceName => "sourceDotnetLinuxArm64";

        protected override string OutputExtension => "bin";

        public override bool Packable => true;
    }
}