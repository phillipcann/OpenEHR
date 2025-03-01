namespace Shellscripts.OpenEHR.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Text.Json;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class SerialisationBaseTypeTests : BaseTest
    {

        public SerialisationBaseTypeTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }


        #region UID Tests

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseIsoOid_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new IsoOid() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "ISO_OID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseUuid_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new Uuid() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "UUID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseInternetId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new InternetId() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "INTERNET_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        #endregion

        #region ObjectId Tests

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseUidBasedId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new UidBasedId() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "UID_BASED_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseHierObjectId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new HierObjectId() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "HIER_OBJECT_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseObjectVersionId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new ObjectVersionId() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "OBJECT_VERSION_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseArchetypeId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new ArchetypeId() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "ARCHETYPE_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseTemplateId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new TemplateId() { Value = "12345" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "TEMPLATE_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseTerminologyId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new TerminologyId() { 
                Value = "12345",
                Name = "SomeTerminology",
                VersionId = "SomeVersion"
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
            AssertJsonValueEquality(actual_json, "$.name", "SomeTerminology");
            AssertJsonValueEquality(actual_json, "$.version_id", "SomeVersion");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseGenericId_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new GenericId() { Value = "12345", Scheme = "Scheme" };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "GENERIC_ID");
            AssertJsonValueEquality(actual_json, "$.value", "12345");
            AssertJsonValueEquality(actual_json, "$.scheme", "Scheme");
        }

        #endregion

        #region ObjectRef Tests

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseObjectRef_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new ObjectRef() {
                Namespace = "SomeNamespace",
                Type = "SomeType",
                Id = new UidBasedId() { Value = "12345"}
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "OBJECT_REF");
            AssertJsonValueEquality(actual_json, "$.namespace", "SomeNamespace");
            AssertJsonValueEquality(actual_json, "$.type", "SomeType");
            AssertJsonValueEquality(actual_json, "$.id._type", "UID_BASED_ID");
            AssertJsonValueEquality(actual_json, "$.id.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialisePartyRef_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new PartyRef()
            {
                Namespace = "SomeNamespace",
                Type = "SomeType",
                Id = new HierObjectId() { Value = "12345" }
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "PARTY_REF");
            AssertJsonValueEquality(actual_json, "$.namespace", "SomeNamespace");
            AssertJsonValueEquality(actual_json, "$.type", "SomeType");
            AssertJsonValueEquality(actual_json, "$.id._type", "HIER_OBJECT_ID");
            AssertJsonValueEquality(actual_json, "$.id.value", "12345");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseLocatableRef_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new LocatableRef()
            {
                Namespace = "SomeNamespace",
                Type = "SomeType",
                Id = new UidBasedId() { Value = "12345" },
                Path = "SomePath",
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "LOCATABLE_REF");
            AssertJsonValueEquality(actual_json, "$.namespace", "SomeNamespace");
            AssertJsonValueEquality(actual_json, "$.path", "SomePath");
            AssertJsonValueEquality(actual_json, "$.type", "SomeType");
            AssertJsonValueEquality(actual_json, "$.id._type", "UID_BASED_ID");
            AssertJsonValueEquality(actual_json, "$.id.value", "12345");
        }

        #endregion

    }
}
