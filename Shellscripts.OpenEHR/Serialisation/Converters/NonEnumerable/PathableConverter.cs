namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class PathableConverter : EhrItemJsonConverter<Pathable>        
    {
        public PathableConverter(ILogger<PathableConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }

    }
}