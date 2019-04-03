using SharpCompress.Common;

namespace cwg.web.Generators
{
    public class RARGenerator : BaseArchiveGenerator
    {
        public override string Name => "RAR";

        protected override string OutputExtension => "rar";

        protected override ArchiveType CurrentArchiveType => ArchiveType.Rar;
    }
}