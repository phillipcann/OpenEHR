namespace Shellscripts.OpenEHR.Serialisation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Shellscripts.OpenEHR.Attribution;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Models.Ehr;

    //public static class TypeMapLookup
    //{

    //    public static IDictionary<string, Type> Values => new Dictionary<string, Type>()
    //    {
    //        // DataValue
    //        //{ "DATA_VALUE", typeof(DataValue) },
    //        //{ "DV_BOOLEAN", typeof(DvBoolean) },
    //        //{ "DV_STATE", typeof(DvState) },
    //        //{ "DV_IDENTIFIER", typeof(DvIdentifier) },
    //        //{ "DV_TEXT", typeof(DvText) },
    //        //{ "DV_PARAGRAPH", typeof(DvParagraph) },
    //        //{ "DV_ORDERED", typeof(DvOrdered) },
    //        //{ "DV_INTERVAL", typeof(DvInterval) },
    //        //{ "DV_TIME_SPECIFICATION", typeof(DvTimeSpecification) },
    //        //{ "DV_ENCAPSULATED", typeof(DvEncapsulated) },
    //        //{ "DV_URI", typeof(DvUri) },

    //        // DvEncapsulated
    //        //{ "DV_MULTIMEDIA", typeof(DvMultiMedia) },
    //        //{ "DV_PARSABLE", typeof(DvParsable) },            

    //        // DvUri
    //        //{ "DV_EHR_URI", typeof(DvEhrUri) },

    //        // DvTimeSpecification
    //        //{ "DV_PERIODIC_TIME_SPECIFICATION", typeof(DvPeriodicTimeSpecification) },
    //        //{ "DV_GENERAL_TIME_SPECIFICATION", typeof(DvGeneralTimeSpecification) },

    //        // DvText
    //        //{ "DV_CODED_TEXT", typeof(DvCodedText) },

    //        // DvOrdered            
    //        //{ "DV_ORDINAL", typeof(DvOrdinal) },
    //        //{ "DV_SCALE", typeof(DvScale) },
    //        //{ "DV_QUANTIFIED", typeof(DvQuantified) },

    //        // DvQuantified
    //        //{ "DV_AMOUNT", typeof(DvAmount) },
    //        //{ "DV_ABSOLUTE_QUANTITY", typeof(DvAbsoluteQuantity) },

    //        // DvAmount
    //        //{ "DV_QUANTITY", typeof(DvQuantity) },
    //        //{ "DV_COUNT", typeof(DvCount) },
    //        //{ "DV_PROPORTION", typeof(DvProportion) },
    //        //{ "DV_DURATION", typeof(DvDuration) },

    //        // DvTemporal
    //        //{ "DV_TEMPORAL", typeof(DvTemporal) },
    //        //{ "DV_DATE", typeof(DvDate) },
    //        //{ "DV_TIME", typeof(DvTime) },
    //        //{ "DV_DATE_TIME", typeof(DvDateTime) },

    //        // Ehr
    //        //{ "EHR", typeof(Ehr) },

    //        // ObjectId
    //        //{ "OBJECT_ID", typeof(ObjectId) },
    //        //{ "UID_BASED_ID", typeof(UidBasedId)},
    //        //{ "ARCHETYPE_ID", typeof(ArchetypeId)},
    //        //{ "TEMPLATE_ID", typeof(TemplateId)},
    //        //{ "TERMINOLOGY_ID", typeof(TerminologyId)},
    //        //{ "GENERIC_ID", typeof(GenericId) },
    //        //{ "HIER_OBJECT_ID", typeof(HierObjectId) },
    //        //{ "OBJECT_VERSION_ID", typeof(ObjectVersionId)},

    //        // ObjectRef
    //        //{ "OBJECT_REF", typeof(ObjectRef) },
    //        //{ "PARTY_REF", typeof(PartyRef)},
    //        //{ "LOCATABLE_REF", typeof(LocatableRef) },

    //        // PartyProxy
    //        //{ "PARTY_PROXY", typeof(PartyProxy) },
    //        //{ "PARTY_SELF", typeof(PartySelf) },
    //        //{ "PARTY_IDENTIFIED", typeof(PartyIdentified) },
    //        //{ "PARTY_RELATED", typeof(PartyRelated) },

    //        // Pathable / Locatable
    //        //{ "ITEM_STRUCTURE", typeof(ItemStructure) },
    //        //{ "ITEM_SINGLE", typeof(ItemSingle) },
    //        //{ "ITEM_LIST", typeof(ItemList)},
    //        //{ "ITEM_TABLE", typeof(ItemTable)},
    //        //{ "ITEM_TREE", typeof(ItemTree)},

    //        //{ "DATA_STRUCTURE", typeof(DataStructure) },

    //        //{ "ITEM", typeof(Item) },
    //        //{ "ELEMENT", typeof(Element) },
    //        //{ "CLUSTER", typeof(Cluster) },

    //        // ContentItem
    //        //{ "CONTENT_ITEM", typeof(ContentItem) },
    //        //{ "SECTION", typeof(Section) },
    //        //{ "ENTRY", typeof(Entry) },
    //        //{ "ADMIN_ENTRY", typeof(AdminEntry) },
    //        //{ "CARE_ENTRY", typeof(CareEntry) },
            
    //        //{ "OBSERVATION", typeof(Observation) },
    //        //{ "EVALUATION", typeof(Evaluation) },
    //        //{ "INSTRUCTION", typeof(Instruction) },
    //        //{ "ACTIVITY", typeof(Activity) },
    //        //{ "ACTION", typeof(Models.Ehr.Action) },
    //        //{ "INSTRUCTION_DETAILS", typeof(InstructionDetails) },
    //        //{ "ISM_TRANSITION", typeof(IsmTransition) },

    //        //{ "COMPOSITION", typeof(Composition) },
    //        //{ "EHR_STATUS", typeof(EhrStatus) },
    //        //{ "EHR_ACCESS", typeof(EhrAccess) },


    //        // Lists / Generics ?
    //        //{ "EVENT", typeof(Event<>) },
    //        //{ "INTERVAL_EVENT", typeof(IntervalEvent<>) },
    //        //{ "POINT_EVENT", typeof(PointEvent<> ) },
    //        //{ "HISTORY", typeof(History<>) }

    //    };

    //    public static KeyValuePair<string, Type>? GetByType(Type type)
    //    {
    //        var found = Values.FirstOrDefault(kvp => kvp.Value.Equals(type));
    //        if (!string.IsNullOrWhiteSpace(found.Key))
    //            return found;

    //        return null;
    //    }

    //    public static KeyValuePair<string, Type>? GetByKey(string key)
    //    {
    //        if (Values.TryGetValue(key, out var type))
    //            return new KeyValuePair<string, Type>(key, type);

    //        return null;
    //    }
    //}


    public interface ITypeMapLookup
    {
        Type? GetTypeByName(string name);
    }

    public class TypeMapLookup : ITypeMapLookup
    {
        private readonly Dictionary<string, Type> _typeMap = new();

        public TypeMapLookup(Assembly assembly)
        {
            // Scan provided assembly for TypeMap attributes
            var typesWithAttribute = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<TypeMapAttribute>() != null);

            foreach (var type in typesWithAttribute)
            {
                var attribute = type.GetCustomAttribute<TypeMapAttribute>();
                if (attribute != null && !_typeMap.ContainsKey(attribute.Name))
                {
                    _typeMap[attribute.Name] = type;
                }
            }
        }

        public Type? GetTypeByName(string name)
        {
            return _typeMap.TryGetValue(name, out var type) ? type : null;
        }
    }
}
