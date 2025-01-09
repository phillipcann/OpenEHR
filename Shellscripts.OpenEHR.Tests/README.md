# Testing

<!--TOC-->
  - [Description](#description)
  - [Definition Tests](#definition-tests)
  - [Deserialisation Tests](#deserialisation-tests)
  - [Extension Method Tests](#extension-method-tests)
  - [Rest Client Tests](#rest-client-tests)
  - [Serialisation Tests](#serialisation-tests)
<!--/TOC-->


## Description

All of the Unit Tests in the solution are configured with `[Trait(name: "TestCategory", value:"<test_type>")]` attribution markup. Some are Unit Tests and do not impact upon the Ehr Endpoint configured in the app settings file.

Some of the unit tests are marked up as "Integration" tests and could potentially have an impact on the end point. The tests could create / read / update or even delete data.


## Definition Tests

These unit tests are testing that we can deserialise the json returned from the Definition Api [here](https://docs.ehrbase.org/api/hip-ehrbase/definition#tag/ADL1.4)

## Deserialisation Tests

The deserialisation tests, very similiar to the definition tests, are checking that we can deserialise a json response taken directly from the Ehrbase.org sandpit server and then access a specific value at a specific point within the structure of the deserialised object. This allows us to build up a comprehensive set of unit tests to ensure that we are deserialising the json correctly as there are many inherited types within the structure of the json.

## Extension Method Tests

There is functionality that "should" be available on most of the objects from the reference model which I decided to implement via extension methods. These tests just check that the implemented methods perform as expected. For example, look at the Composition class definition [here](https://specifications.openehr.org/releases/RM/Release-1.1.0/ehr.html#_composition_class) and scroll down to the "Functions" section. There should be a "is_persistent()" method implemented on the composition (which hasnt been at the time of publication of this document).

## Rest Client Tests

These tests are marked up as integration tests because the Client will actually call out to the test Open Ehr server found at "https://sandkiste.ehrbase.org/". It's suggested that you install a local instance of EhrBase via the Docker containers available to run any integration testing against.

The Integration Tests in the solution can be configured to be automatically skipped by configuration in the `xunit.runsettings` configuration file. This is an XML file and information on the configuration of this file can be found in the following locations

- [RunSettings](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022#example-runsettings-file)
- [XUnit](https://xunit.net/docs/runsettings#runsettings)

The solution has been checked in with <b>ONLY UNIT Tests</b> enabled.

This is the piece of configuration which enables / disables the Integration Tests. Comment it out to enable ALL of the tests.

```xml
<TestCaseFilter>(TestCategory != Integration)</TestCaseFilter>
```

## Serialisation Tests

The unit tests in this class test that we can turn the complex strongly typed models into appropriate format application json which can be posted through the Rest Client.

There are a series of .json files in the `./Shellscripts.OpenEHR.Tests/Assets/Serialisation` folder location which detail what our expected output should look like. The unit tests then 'new' up the objects and serialise them to compare against the file contents.