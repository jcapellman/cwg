namespace cwg.web.Generators
{
    public class PowerShellController : BaseGenerator
    {
        public override string Name => "Powershell";

        protected override string SourceName => "sourcePS.ps1";

        protected override string OutputExtension => "ps1";
    }
}