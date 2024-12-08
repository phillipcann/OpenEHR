namespace Shellscripts.OpenEHR.Models.DataStructures
{
    // Data Structures Information Model (https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_data_structures_information_model)

    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataTypes;

    #region 3.1 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions

    public class DataStructure : Locatable { }

    #endregion


    #region 4.3 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions_2

    public class ItemStructure : DataStructure { }

    public class ItemSingle : ItemStructure
    {
        [JsonPropertyName("item")]
        public Element Item { get; set; }
    }

    public class ItemList : ItemStructure
    {
        [JsonPropertyName("items")]
        public Element[] Items { get; set; }
    }

    public class ItemTable : ItemStructure
    {
        [JsonPropertyName("rows")]
        public Cluster[] Rows { get; set; }
    }

    public class ItemTree : ItemStructure
    {
        [JsonPropertyName("items")]
        public Item[] Items { get; set; }
    }

    #endregion


    #region 5.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions_3

    public class Item : Locatable { }

    public class Cluster : Item
    {
        [JsonPropertyName("items")]
        public Item[] Items { get; set; }
    }

    public class Element : Item
    {
        [JsonPropertyName("null_flavour")]
        public DvCodedText NullFlavour { get; set; }

        [JsonPropertyName("value")]
        public DataValue Value { get; set; }

        [JsonPropertyName("null_reason")]
        public DvText NullReason { get; set; }

    }

    #endregion


    #region 6.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions_4

    public class History : DataStructure
    {
        [JsonPropertyName("origin")]
        public DvDateTime Origin { get; set; }

        [JsonPropertyName("period")]
        public DvDuration Period { get; set; }

        [JsonPropertyName("duration")]
        public DvDuration Duration { get; set; }

        [JsonPropertyName("summary")]
        public ItemStructure Summary { get; set; }

        [JsonPropertyName("events")]
        public Event[] Events { get; set; }

    }
    public class History<T> : History
        where T : class
    { }

    public class Event : Locatable
    {
        [JsonPropertyName("time")]
        public DvDateTime Time { get; set; }

        [JsonPropertyName("state")]
        public ItemStructure State { get; set; }

        [JsonPropertyName("data")]
        public object Data { get; set; }
    }
    public class Event<T> : Event
        where T : class
    {
        [JsonPropertyName("data")]
        public new T Data { get; set; }
    }

    public class PointEvent<T> : Event
        where T : class
    {

    }

    public class InternalEvent<T> : Event
        where T : class
    {
        [JsonPropertyName("width")]
        public DvDuration Width { get; set; }

        [JsonPropertyName("sample_count")]
        public int SampleCount { get; set; }

        [JsonPropertyName("math_function")]
        public DvCodedText MathFunction { get; set; }
    }




    #endregion

}
