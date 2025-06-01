# Options
# Table of Contents
- [Configuration Providers](#header_1)
- [Configurable Options](#header_2)
- [ConsoleLoggerOptions](#header_3)
  - [Properties](#header_4)
    - [LogToStandardErrorThreshold](#header_5)
    - [QueueFullMode](#header_6)
    - [TimestampFormat](#header_7)
    - [MaxQueueLength](#header_8)
    - [UseUtcTimestamp](#header_9)
    - [Format](#header_10)
    - [IncludeScopes](#header_11)
    - [FormatterName](#header_12)
    - [DisableColors](#header_13)
- [HostOptions](#header_14)
  - [Properties](#header_15)
    - [ServicesStartConcurrently](#header_16)
    - [ServicesStopConcurrently](#header_17)
    - [BackgroundServiceExceptionBehavior](#header_18)
- [LoggerFactoryOptions](#header_19)
  - [Properties](#header_20)
    - [ActivityTrackingOptions](#header_21)
- [LoggerFilterOptions](#header_22)
  - [Properties](#header_23)
    - [CaptureScopes](#header_24)
    - [MinLevel](#header_25)
- [JsonSchemaGeneratorOptions](#header_26)
  - [Properties](#header_27)
    - [TypeInfoResolverChain](#header_28)
    - [RespectRequiredConstructorParameters](#header_29)
    - [PreferredObjectCreationHandling](#header_30)
    - [IgnoreReadOnlyFields](#header_31)
    - [IndentSize](#header_32)
    - [AllowOutOfOrderMetadataProperties](#header_33)
    - [IncludeFields](#header_34)
    - [DefaultIgnoreCondition](#header_35)
    - [ReadCommentHandling](#header_36)
    - [WriteIndented](#header_37)
    - [PropertyNameCaseInsensitive](#header_38)
    - [TreatNullObliviousAsNonNullable](#header_39)
    - [DefaultBufferSize](#header_40)
    - [UnknownTypeHandling](#header_41)
    - [RespectNullableAnnotations](#header_42)
    - [MaxDepth](#header_43)
    - [JsonSchemaExporterOptions](#header_44)
    - [NumberHandling](#header_45)
    - [IndentCharacter](#header_46)
    - [NewLine](#header_47)
    - [UnmappedMemberHandling](#header_48)
    - [JsonSerializerOptions](#header_49)
    - [TypeInfoResolver](#header_50)
    - [IgnoreNullValues](#header_51)
    - [OutputDirectory](#header_52)
    - [AllowTrailingCommas](#header_53)
    - [IgnoreReadOnlyProperties](#header_54)
- [BindSightRunnerOptions](#header_55)
  - [Properties](#header_56)
    - [ExitMode](#header_57)
    - [ExitOnCancellation](#header_58)
    - [Run](#header_59)
- [ReadmeGeneratorOptions](#header_60)
  - [Properties](#header_61)
    - [OutputFile](#header_62)
    - [Title](#header_63)
- [Test](#header_64)
  - [Properties](#header_65)
    - [NestedSet](#header_66)
    - [Int32Property](#header_67)
    - [CircularReference](#header_68)
    - [NestedProperty](#header_69)
    - [StringList](#header_70)

## Configuration Providers <a id="header_1"></a>
| Type | Details |
| -- | -- |
| <details><summary>```MemoryConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Memory.MemoryConfigurationProvider```</details> |  |
| <details><summary>```MemoryConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Memory.MemoryConfigurationProvider```</details> |  |
| <details><summary>```EnvironmentVariablesConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider```</details> | Prefix: `DOTNET_` |
| <details><summary>```JsonConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider```</details> | Path: `appsettings.json`, Optional: True |
| <details><summary>```JsonConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider```</details> | Path: `appsettings.Development.json`, Optional: True |
| <details><summary>```JsonConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider```</details> | Path: `secrets.json`, Optional: True |
| <details><summary>```EnvironmentVariablesConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.EnvironmentVariables.EnvironmentVariablesConfigurationProvider```</details> |  |
| <details><summary>```CommandLineConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.CommandLine.CommandLineConfigurationProvider```</details> |  |
| <details><summary>```MemoryConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Memory.MemoryConfigurationProvider```</details> |  |
## Configurable Options <a id="header_2"></a>
### ConsoleLoggerOptions <a id="header_3"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions` |
| Type | <details><summary>```ConsoleLoggerOptions```</summary>```Microsoft.Extensions.Logging.Console.ConsoleLoggerOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_4"></a>
##### LogToStandardErrorThreshold <a id="header_5"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:LogToStandardErrorThreshold` |
| Type | <details><summary>```LogLevel```</summary>```Microsoft.Extensions.Logging.LogLevel```</details> |
| Allowed Values | `"Trace"`(`0`), `"Debug"`(`1`), `"Information"`(`2`), `"Warning"`(`3`), `"Error"`(`4`), `"Critical"`(`5`), `"None"`(`6`) |
| Disallowed Values | `null` |
##### QueueFullMode <a id="header_6"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:QueueFullMode` |
| Type | <details><summary>```ConsoleLoggerQueueFullMode```</summary>```Microsoft.Extensions.Logging.Console.ConsoleLoggerQueueFullMode```</details> |
| Allowed Values | `"Wait"`(`0`), `"DropWrite"`(`1`) |
| Disallowed Values | `null` |
##### TimestampFormat <a id="header_7"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:TimestampFormat` |
| Type | ``string`` |
| Allowed Values | `null` |
##### MaxQueueLength <a id="header_8"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:MaxQueueLength` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### UseUtcTimestamp <a id="header_9"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:UseUtcTimestamp` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### Format <a id="header_10"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:Format` |
| Type | <details><summary>```ConsoleLoggerFormat```</summary>```Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat```</details> |
| Allowed Values | `"Default"`(`0`), `"Systemd"`(`1`) |
| Disallowed Values | `null` |
##### IncludeScopes <a id="header_11"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:IncludeScopes` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### FormatterName <a id="header_12"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:FormatterName` |
| Type | ``string`` |
| Allowed Values | `null` |
##### DisableColors <a id="header_13"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:DisableColors` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
### HostOptions <a id="header_14"></a>
|  |  |
| -- | -- |
| Key | `HostOptions` |
| Type | <details><summary>```HostOptions```</summary>```Microsoft.Extensions.Hosting.HostOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_15"></a>
##### ServicesStartConcurrently <a id="header_16"></a>
|  |  |
| -- | -- |
| Key | `HostOptions:ServicesStartConcurrently` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### ServicesStopConcurrently <a id="header_17"></a>
|  |  |
| -- | -- |
| Key | `HostOptions:ServicesStopConcurrently` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### BackgroundServiceExceptionBehavior <a id="header_18"></a>
|  |  |
| -- | -- |
| Key | `HostOptions:BackgroundServiceExceptionBehavior` |
| Type | <details><summary>```BackgroundServiceExceptionBehavior```</summary>```Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior```</details> |
| Allowed Values | `"StopHost"`(`0`), `"Ignore"`(`1`) |
| Disallowed Values | `null` |
### LoggerFactoryOptions <a id="header_19"></a>
|  |  |
| -- | -- |
| Key | `LoggerFactoryOptions` |
| Type | <details><summary>```LoggerFactoryOptions```</summary>```Microsoft.Extensions.Logging.LoggerFactoryOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_20"></a>
##### ActivityTrackingOptions <a id="header_21"></a>
|  |  |
| -- | -- |
| Key | `LoggerFactoryOptions:ActivityTrackingOptions` |
| Type | <details><summary>```ActivityTrackingOptions``` (flags)</summary>```Microsoft.Extensions.Logging.ActivityTrackingOptions```</details> |
| Allowed Values | `"None"`(`0`), `"SpanId"`(`1`), `"TraceId"`(`2`), `"ParentId"`(`4`), `"TraceState"`(`8`), `"TraceFlags"`(`16`), `"Tags"`(`32`), `"Baggage"`(`64`) |
| Disallowed Values | `null` |
### LoggerFilterOptions <a id="header_22"></a>
|  |  |
| -- | -- |
| Key | `LoggerFilterOptions` |
| Type | <details><summary>```LoggerFilterOptions```</summary>```Microsoft.Extensions.Logging.LoggerFilterOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_23"></a>
##### CaptureScopes <a id="header_24"></a>
|  |  |
| -- | -- |
| Key | `LoggerFilterOptions:CaptureScopes` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### MinLevel <a id="header_25"></a>
|  |  |
| -- | -- |
| Key | `LoggerFilterOptions:MinLevel` |
| Type | <details><summary>```LogLevel```</summary>```Microsoft.Extensions.Logging.LogLevel```</details> |
| Allowed Values | `"Trace"`(`0`), `"Debug"`(`1`), `"Information"`(`2`), `"Warning"`(`3`), `"Error"`(`4`), `"Critical"`(`5`), `"None"`(`6`) |
| Disallowed Values | `null` |
### JsonSchemaGeneratorOptions <a id="header_26"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions` |
| Type | <details><summary>```MutableJsonSchemaGeneratorOptions```</summary>```RhoMicro.BindSight.Generators.MutableJsonSchemaGeneratorOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_27"></a>
##### TypeInfoResolverChain <a id="header_28"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:TypeInfoResolverChain` |
| Type | <details><summary>```IList<IJsonTypeInfoResolver>```</summary>```System.Collections.Generic.IList<System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver>```</details> |
| Disallowed Values | `null` |
##### RespectRequiredConstructorParameters <a id="header_29"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:RespectRequiredConstructorParameters` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### PreferredObjectCreationHandling <a id="header_30"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:PreferredObjectCreationHandling` |
| Type | <details><summary>```JsonObjectCreationHandling```</summary>```System.Text.Json.Serialization.JsonObjectCreationHandling```</details> |
| Allowed Values | `"Replace"`(`0`), `"Populate"`(`1`) |
| Disallowed Values | `null` |
##### IgnoreReadOnlyFields <a id="header_31"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IgnoreReadOnlyFields` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### IndentSize <a id="header_32"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IndentSize` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### AllowOutOfOrderMetadataProperties <a id="header_33"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:AllowOutOfOrderMetadataProperties` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### IncludeFields <a id="header_34"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IncludeFields` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### DefaultIgnoreCondition <a id="header_35"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:DefaultIgnoreCondition` |
| Type | <details><summary>```JsonIgnoreCondition```</summary>```System.Text.Json.Serialization.JsonIgnoreCondition```</details> |
| Allowed Values | `"Never"`(`0`), `"Always"`(`1`), `"WhenWritingDefault"`(`2`), `"WhenWritingNull"`(`3`), `"WhenWriting"`(`4`), `"WhenReading"`(`5`) |
| Disallowed Values | `null` |
##### ReadCommentHandling <a id="header_36"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:ReadCommentHandling` |
| Type | <details><summary>```JsonCommentHandling```</summary>```System.Text.Json.JsonCommentHandling```</details> |
| Allowed Values | `"Disallow"`(`0`), `"Skip"`(`1`), `"Allow"`(`2`) |
| Disallowed Values | `null` |
##### WriteIndented <a id="header_37"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:WriteIndented` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### PropertyNameCaseInsensitive <a id="header_38"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:PropertyNameCaseInsensitive` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### TreatNullObliviousAsNonNullable <a id="header_39"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSchemaExporterOptions:TreatNullObliviousAsNonNullable` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### DefaultBufferSize <a id="header_40"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:DefaultBufferSize` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### UnknownTypeHandling <a id="header_41"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:UnknownTypeHandling` |
| Type | <details><summary>```JsonUnknownTypeHandling```</summary>```System.Text.Json.Serialization.JsonUnknownTypeHandling```</details> |
| Allowed Values | `"JsonElement"`(`0`), `"JsonNode"`(`1`) |
| Disallowed Values | `null` |
##### RespectNullableAnnotations <a id="header_42"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:RespectNullableAnnotations` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### MaxDepth <a id="header_43"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:MaxDepth` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### JsonSchemaExporterOptions <a id="header_44"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSchemaExporterOptions` |
| Type | <details><summary>```JsonSchemaExporterOptions```</summary>```System.Text.Json.Schema.JsonSchemaExporterOptions```</details> |
| Disallowed Values | `null` |
##### NumberHandling <a id="header_45"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:NumberHandling` |
| Type | <details><summary>```JsonNumberHandling``` (flags)</summary>```System.Text.Json.Serialization.JsonNumberHandling```</details> |
| Allowed Values | `"Strict"`(`0`), `"AllowReadingFromString"`(`1`), `"WriteAsString"`(`2`), `"AllowNamedFloatingPointLiterals"`(`4`) |
| Disallowed Values | `null` |
##### IndentCharacter <a id="header_46"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IndentCharacter` |
| Type | ``char`` |
| Disallowed Values | `null` |
##### NewLine <a id="header_47"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:NewLine` |
| Type | ``string`` |
| Disallowed Values | `null` |
##### UnmappedMemberHandling <a id="header_48"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:UnmappedMemberHandling` |
| Type | <details><summary>```JsonUnmappedMemberHandling```</summary>```System.Text.Json.Serialization.JsonUnmappedMemberHandling```</details> |
| Allowed Values | `"Skip"`(`0`), `"Disallow"`(`1`) |
| Disallowed Values | `null` |
##### JsonSerializerOptions <a id="header_49"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions` |
| Type | <details><summary>```JsonSerializerOptions```</summary>```System.Text.Json.JsonSerializerOptions```</details> |
| Disallowed Values | `null` |
##### TypeInfoResolver <a id="header_50"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:TypeInfoResolver` |
| Type | <details><summary>```IJsonTypeInfoResolver```</summary>```System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver```</details> |
| Allowed Values | `null` |
##### IgnoreNullValues <a id="header_51"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IgnoreNullValues` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### OutputDirectory <a id="header_52"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:OutputDirectory` |
| Type | ``string`` |
| Description | Sets the directory to generate json schemata of options into. |
| Disallowed Values | `null` |
##### AllowTrailingCommas <a id="header_53"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:AllowTrailingCommas` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### IgnoreReadOnlyProperties <a id="header_54"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IgnoreReadOnlyProperties` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
### BindSightRunnerOptions <a id="header_55"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions` |
| Type | <details><summary>```MutableBindSightRunnerOptions```</summary>```RhoMicro.BindSight.Services.MutableBindSightRunnerOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_56"></a>
##### ExitMode <a id="header_57"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions:ExitMode` |
| Type | <details><summary>```ExitMode```</summary>```RhoMicro.BindSight.Services.ExitMode```</details> |
| Description | Sets the exit mode after running all options docs generators. The default is `ExitMode.Environment`. |
| Allowed Values | `"None"`(`0`), `"Host"`(`1`), `"Environment"`(`2`) |
| Disallowed Values | `null` |
##### ExitOnCancellation <a id="header_58"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions:ExitOnCancellation` |
| Type | ``bool`` |
| Description | If set to `true`, the options docs runner will exit after receiving cancellation. The default is `true`. |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### Run <a id="header_59"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions:Run` |
| Type | ``bool`` |
| Description | If set to `true`, the options docs runner will run. The default is `false`. |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
### ReadmeGeneratorOptions <a id="header_60"></a>
|  |  |
| -- | -- |
| Key | `ReadmeGeneratorOptions` |
| Type | <details><summary>```MutableReadmeGeneratorOptions```</summary>```RhoMicro.BindSight.Generators.MutableReadmeGeneratorOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_61"></a>
##### OutputFile <a id="header_62"></a>
|  |  |
| -- | -- |
| Key | `ReadmeGeneratorOptions:OutputFile` |
| Type | ``string`` |
| Description | Sets the path of the readme file to generate options docs into. The default is `Options/Readme.md`. |
| Disallowed Values | `null` |
##### Title <a id="header_63"></a>
|  |  |
| -- | -- |
| Key | `ReadmeGeneratorOptions:Title` |
| Type | ``string`` |
| Description | Sets the title of the readme file. The default is `Options`. |
| Disallowed Values | `null` |
### Test <a id="header_64"></a>
|  |  |
| -- | -- |
| Key | `Test` |
| Type | <details><summary>```TestOptions```</summary>```RhoMicro.BindSight.ExploratoryTests.TestOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_65"></a>
##### NestedSet <a id="header_66"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedSet` |
| Type | <details><summary>```HashSet<TestNestedOptions>```</summary>```System.Collections.Generic.HashSet<RhoMicro.BindSight.ExploratoryTests.TestNestedOptions>```</details> |
| Disallowed Values | `null` |
##### Int32Property <a id="header_67"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedSet:[n]:Int32Property` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### CircularReference <a id="header_68"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedSet:[n]:CircularReference` |
| Type | <details><summary>```TestNestedOptions```</summary>```RhoMicro.BindSight.ExploratoryTests.TestNestedOptions```</details> |
| Disallowed Values | `null` |
##### NestedProperty <a id="header_69"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedProperty` |
| Type | <details><summary>```TestNestedOptions```</summary>```RhoMicro.BindSight.ExploratoryTests.TestNestedOptions```</details> |
| Disallowed Values | `null` |
##### StringList <a id="header_70"></a>
|  |  |
| -- | -- |
| Key | `Test:StringList` |
| Type | <details><summary>```List<string>```</summary>```System.Collections.Generic.List<string>```</details> |
| Disallowed Values | `null` |
