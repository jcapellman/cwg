using SharpCompress.Common;

namespace cwg.web.Generators
{
    public class TarGenerator : BaseArchiveGenerator
    {
        public override string Name => "TAR";

        protected override string OutputExtension => "tar";

        public override bool Packable => false;

        protected override ArchiveType CurrentArchiveType => ArchiveType.Tar;
    }
}