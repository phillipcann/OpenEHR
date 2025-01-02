# Testing

## Description

All of the Unit Tests in the solution are configured with `[Trait(name: "TestCategory", value:"<test_type>")]` attribution markup. Some are Unit Tests and do not impact upon the Ehr Endpoint configured in the app settings file.

Some of the unit tests are marked up as "Integration" tests and could potentially have an impact on the end point. The tests could create / read / update or even delete data.


### Unit Tests

Tests marked up with the Unit category can safely be run without impacting an endpoint.



### Integration Tests

There are a number of Integration Tests in the solution that can be configured to be automatically skipped by the `xunit.runsettings` configuration file. This is an XML file and information on the configuration of this file can be found in the following locations

- [RunSettings](https://learn.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2022#example-runsettings-file)
- [XUnit](https://xunit.net/docs/runsettings#runsettings)

The solution has been checked in with <b>ALL</b> of the tests enabled.