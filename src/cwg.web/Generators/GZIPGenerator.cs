﻿using SharpCompress.Common;

namespace cwg.web.Generators
{
    public class GZIPGenerator : BaseArchiveGenerator
    {
        public override string Name => "GZIP";

        public override bool Active => true;

        protected override string OutputExtension => "gzip";

        protected override ArchiveType CurrentArchiveType => ArchiveType.GZip;

        public override bool Packable => false;
    }
}