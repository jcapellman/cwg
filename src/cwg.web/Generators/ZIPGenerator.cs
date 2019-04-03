using SharpCompress.Common;

namespace cwg.web.Generators
{
    public class ZIPGenerator : BaseArchiveGenerator
    {
        public override string Name => "ZIP";

        protected override string OutputExtension => "zip";

        protected override ArchiveType CurrentArchiveType => ArchiveType.Zip;
    }
}