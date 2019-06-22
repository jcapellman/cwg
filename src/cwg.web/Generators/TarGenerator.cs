using SharpCompress.Common;

namespace cwg.web.Generators
{
    public class TarGenerator : BaseArchiveGenerator
    {
        public override string Name => "TAR";

        protected override string OutputExtension => "tar";

        protected override ArchiveType CurrentArchiveType => ArchiveType.Tar;
    }
}