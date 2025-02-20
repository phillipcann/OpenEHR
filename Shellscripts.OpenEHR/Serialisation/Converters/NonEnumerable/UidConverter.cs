namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class UidConverter : EhrItemJsonConverter<Uid>
    {
        public UidConverter(ILogger<UidConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }
}