using SharpCompress.Common;

namespace cwg.web.Generators
{
    public class SevenZipGenerator : BaseArchiveGenerator
    {
        public override string Name => "7Z";

        protected override string OutputExtension => "7z";

        protected override ArchiveType CurrentArchiveType => ArchiveType.SevenZip;
    }
}