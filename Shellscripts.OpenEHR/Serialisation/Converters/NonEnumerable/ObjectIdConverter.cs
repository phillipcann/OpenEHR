namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class ObjectIdConverter : EhrItemJsonConverter<ObjectId>        
    {
        public ObjectIdConverter(ILogger<ObjectIdConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }
}