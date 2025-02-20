namespace Shellscripts.OpenEHR.Models.DataStructures
{
    // Data Structures Information Model (https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_data_structures_information_model)

    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Attribution;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataTypes;

    #region 3.1 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions

    [TypeMap("DATA_STRUCTURE")]
    public abstract class DataStructure : Locatable { }

    #endregion


    #region 4.3 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions_2

    // TODO : This "should" be an abstract class. It causes problems with Deserialisation though
    [TypeMap("ITEM_STRUCTURE")]
    public abstract class ItemStructure : DataStructure { }

    [TypeMap("ITEM_SINGLE")]
    public class ItemSingle : ItemStructure
    {
        [JsonPropertyName("item")]
        public Element Item { get; set; }
    }

    [TypeMap("ITEM_LIST")]
    public class ItemList : ItemStructure
    {
        [JsonPropertyName("items")]
        public Element[] Items { get; set; }
    }

    [TypeMap("ITEM_TABLE")]
    public class ItemTable : ItemStructure
    {
        [JsonPropertyName("rows")]
        public Cluster[] Rows { get; set; }
    }

    [TypeMap("ITEM_TREE")]
    public class ItemTree : ItemStructure
    {
        [JsonPropertyName("items")]
        public Item[] Items { get; set; }
    }

    #endregion


    #region 5.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_structures.html#_class_descriptions_3

    // TODO : This class should be abstract. Causes problems with Deserialisation though.
    [TypeMap("ITEM")]
    public abstract class Item : Locatable { }

    [TypeMap("CLUSTER")]
    public class Cluster : Item
    {
        [JsonPropertyName("items")]
        public Item[] Items { get; set; }
    }

    [TypeMap("ELEMENT")]
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

    [TypeMap("HISTORY")]
    public class History<T> : DataStructure
        where T : ItemStructure
    {
        [JsonPropertyName("origin")]
        public DvDateTime Origin { get; set; }

        [JsonPropertyName("period")]
        public DvDuration? Period { get; set; }

        [JsonPropertyName("duration")]
        public DvDuration? Duration { get; set; }

        [JsonPropertyName("summary")]
        public ItemStructure? Summary { get; set; }

        [JsonPropertyName("events")]
        public Event<T>[]? Events { get; set; }

    }

    // TODO : This class "should" be abstract but it causes problems with the deserialisation
    [TypeMap("EVENT")]
    public abstract class Event<T> : Locatable
        where T : ItemStructure
    {
        [JsonPropertyName("time")]
        public DvDateTime Time { get; set; }

        [JsonPropertyName("state")]
        public ItemStructure? State { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("offset")]
        public DvDuration? Offset { get; set; }
    }

    [TypeMap("POINT_EVENT")]
    public class PointEvent<T> : Event<T>
        where T : ItemStructure
    {

    }

    [TypeMap("INTERVAL_EVENT")]
    public class IntervalEvent<T> : Event<T>
        where T : ItemStructure
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
