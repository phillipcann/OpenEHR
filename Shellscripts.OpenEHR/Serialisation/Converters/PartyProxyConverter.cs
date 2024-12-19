namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.CommonInformation;


    public class PartyProxyConverter : EhrItemJsonConverter<PartyProxy>
    {
        public PartyProxyConverter(ILogger<PartyProxyConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "PARTY_PROXY", typeof(PartyProxy) },
            { "PARTY_SELF", typeof(PartySelf) },
            { "PARTY_IDENTIFIED", typeof(PartyIdentified) },
            { "PARTY_RELATED", typeof(PartyRelated) }
        };
    }


    //public class PartyProxyConverter : JsonConverter<PartyProxy>
    //{

    //    private readonly ILogger<PartyProxyConverter> _logger;

    //    public PartyProxyConverter(ILogger<PartyProxyConverter> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public override PartyProxy? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        PartyProxy partyProxy;

    //        using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
    //        {
    //            JsonElement root = jsonDocument.RootElement;

    //            if (root.TryGetProperty("_type", out JsonElement typeElement))
    //            {
    //                IDictionary<string, Type> typeMap = new Dictionary<string, Type>
    //                {
    //                    { "PARTY_PROXY", typeof(PartyProxy) },
    //                    { "PARTY_SELF", typeof(PartySelf) },
    //                    { "PARTY_IDENTIFIED", typeof(PartyIdentified) },
    //                    { "PARTY_RELATED", typeof(PartyRelated) }
    //                };

    //                string idType = typeElement.GetString() ?? string.Empty;

    //                if (!typeMap.TryGetValue(idType, out Type? targetType))
    //                {
    //                    var unknownTypeMessage = $"Unknown _type: '{idType}'";
    //                    _logger.LogWarning(unknownTypeMessage);
    //                    throw new JsonException(unknownTypeMessage);
    //                }

    //                partyProxy = (PartyProxy)JsonSerializer.Deserialize(root.GetRawText() ?? string.Empty, targetType, options);
    //            }
    //            else
    //            {
    //                // unable to ascertain the type....
    //                throw new JsonException("Unknown _type");
    //            }
    //        }

    //        return partyProxy;
    //    }

    //    public override void Write(Utf8JsonWriter writer, PartyProxy value, JsonSerializerOptions options)
    //    {
    //        // TODO : Create Implementation
    //        throw new NotImplementedException();
    //    }
    //}
}
