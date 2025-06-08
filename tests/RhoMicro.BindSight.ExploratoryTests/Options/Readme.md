# Options
# Table of Contents
- [Configuration Providers](#header_1)
- [Configurable Options](#header_2)
- [ConsoleLoggerOptions](#header_3)
  - [Properties](#header_4)
    - [FormatterName](#header_5)
    - [IncludeScopes](#header_6)
    - [MaxQueueLength](#header_7)
    - [QueueFullMode](#header_8)
    - [TimestampFormat](#header_9)
    - [DisableColors](#header_10)
    - [Format](#header_11)
    - [LogToStandardErrorThreshold](#header_12)
    - [UseUtcTimestamp](#header_13)
- [HostOptions](#header_14)
  - [Properties](#header_15)
    - [ServicesStopConcurrently](#header_16)
    - [BackgroundServiceExceptionBehavior](#header_17)
    - [ServicesStartConcurrently](#header_18)
- [LoggerFactoryOptions](#header_19)
  - [Properties](#header_20)
    - [ActivityTrackingOptions](#header_21)
- [LoggerFilterOptions](#header_22)
  - [Properties](#header_23)
    - [MinLevel](#header_24)
    - [CaptureScopes](#header_25)
- [BindSightRunnerOptions](#header_26)
  - [Properties](#header_27)
    - [Run](#header_28)
    - [ExitMode](#header_29)
    - [ExitOnCancellation](#header_30)
- [JsonSchemaGeneratorOptions](#header_31)
  - [Properties](#header_32)
    - [UnmappedMemberHandling](#header_33)
    - [IndentCharacter](#header_34)
    - [IndentSize](#header_35)
    - [TreatNullObliviousAsNonNullable](#header_36)
    - [ReadCommentHandling](#header_37)
    - [IncludeFields](#header_38)
    - [DefaultIgnoreCondition](#header_39)
    - [JsonSchemaExporterOptions](#header_40)
    - [AllowTrailingCommas](#header_41)
    - [AllowOutOfOrderMetadataProperties](#header_42)
    - [MaxDepth](#header_43)
    - [NumberHandling](#header_44)
    - [IgnoreReadOnlyFields](#header_45)
    - [NewLine](#header_46)
    - [TypeInfoResolver](#header_47)
    - [IgnoreReadOnlyProperties](#header_48)
    - [OutputDirectory](#header_49)
    - [PreferredObjectCreationHandling](#header_50)
    - [IgnoreNullValues](#header_51)
    - [JsonSerializerOptions](#header_52)
    - [TypeInfoResolverChain](#header_53)
    - [PropertyNameCaseInsensitive](#header_54)
    - [DefaultBufferSize](#header_55)
    - [UnknownTypeHandling](#header_56)
    - [RespectNullableAnnotations](#header_57)
    - [RespectRequiredConstructorParameters](#header_58)
    - [WriteIndented](#header_59)
- [ReadmeGeneratorOptions](#header_60)
  - [Properties](#header_61)
    - [Title](#header_62)
    - [OutputFile](#header_63)
- [Test](#header_64)
  - [Properties](#header_65)
    - [StringList](#header_66)
    - [Int32Property](#header_67)
    - [NestedSet](#header_68)
    - [NestedProperty](#header_69)
    - [CircularReference](#header_70)

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
| <details><summary>```JsonConfigurationProvider```</summary>```Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider```</details> | Path: `myconfig.json`, Optional: True |
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
##### FormatterName <a id="header_5"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:FormatterName` |
| Type | ``string`` |
| Allowed Values | `null` |
##### IncludeScopes <a id="header_6"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:IncludeScopes` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### MaxQueueLength <a id="header_7"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:MaxQueueLength` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### QueueFullMode <a id="header_8"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:QueueFullMode` |
| Type | <details><summary>```ConsoleLoggerQueueFullMode```</summary>```Microsoft.Extensions.Logging.Console.ConsoleLoggerQueueFullMode```</details> |
| Allowed Values | `"Wait"`(`0`), `"DropWrite"`(`1`) |
| Disallowed Values | `null` |
##### TimestampFormat <a id="header_9"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:TimestampFormat` |
| Type | ``string`` |
| Allowed Values | `null` |
##### DisableColors <a id="header_10"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:DisableColors` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### Format <a id="header_11"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:Format` |
| Type | <details><summary>```ConsoleLoggerFormat```</summary>```Microsoft.Extensions.Logging.Console.ConsoleLoggerFormat```</details> |
| Allowed Values | `"Default"`(`0`), `"Systemd"`(`1`) |
| Disallowed Values | `null` |
##### LogToStandardErrorThreshold <a id="header_12"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:LogToStandardErrorThreshold` |
| Type | <details><summary>```LogLevel```</summary>```Microsoft.Extensions.Logging.LogLevel```</details> |
| Allowed Values | `"Trace"`(`0`), `"Debug"`(`1`), `"Information"`(`2`), `"Warning"`(`3`), `"Error"`(`4`), `"Critical"`(`5`), `"None"`(`6`) |
| Disallowed Values | `null` |
##### UseUtcTimestamp <a id="header_13"></a>
|  |  |
| -- | -- |
| Key | `ConsoleLoggerOptions:UseUtcTimestamp` |
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
##### ServicesStopConcurrently <a id="header_16"></a>
|  |  |
| -- | -- |
| Key | `HostOptions:ServicesStopConcurrently` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### BackgroundServiceExceptionBehavior <a id="header_17"></a>
|  |  |
| -- | -- |
| Key | `HostOptions:BackgroundServiceExceptionBehavior` |
| Type | <details><summary>```BackgroundServiceExceptionBehavior```</summary>```Microsoft.Extensions.Hosting.BackgroundServiceExceptionBehavior```</details> |
| Allowed Values | `"StopHost"`(`0`), `"Ignore"`(`1`) |
| Disallowed Values | `null` |
##### ServicesStartConcurrently <a id="header_18"></a>
|  |  |
| -- | -- |
| Key | `HostOptions:ServicesStartConcurrently` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
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
##### MinLevel <a id="header_24"></a>
|  |  |
| -- | -- |
| Key | `LoggerFilterOptions:MinLevel` |
| Type | <details><summary>```LogLevel```</summary>```Microsoft.Extensions.Logging.LogLevel```</details> |
| Allowed Values | `"Trace"`(`0`), `"Debug"`(`1`), `"Information"`(`2`), `"Warning"`(`3`), `"Error"`(`4`), `"Critical"`(`5`), `"None"`(`6`) |
| Disallowed Values | `null` |
##### CaptureScopes <a id="header_25"></a>
|  |  |
| -- | -- |
| Key | `LoggerFilterOptions:CaptureScopes` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
### BindSightRunnerOptions <a id="header_26"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions` |
| Type | <details><summary>```MutableBindSightRunnerOptions```</summary>```RhoMicro.BindSight.Services.MutableBindSightRunnerOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_27"></a>
##### Run <a id="header_28"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions:Run` |
| Type | ``bool`` |
| Description | If set to `true`, the options docs runner will run. The default is `false`. |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### ExitMode <a id="header_29"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions:ExitMode` |
| Type | <details><summary>```ExitMode```</summary>```RhoMicro.BindSight.Services.ExitMode```</details> |
| Description | Sets the exit mode after running all options docs generators. The default is `ExitMode.Environment`. |
| Allowed Values | `"None"`(`0`), `"Host"`(`1`), `"Environment"`(`2`) |
| Disallowed Values | `null` |
##### ExitOnCancellation <a id="header_30"></a>
|  |  |
| -- | -- |
| Key | `BindSightRunnerOptions:ExitOnCancellation` |
| Type | ``bool`` |
| Description | If set to `true`, the options docs runner will exit after receiving cancellation. The default is `true`. |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
### JsonSchemaGeneratorOptions <a id="header_31"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions` |
| Type | <details><summary>```MutableJsonSchemaGeneratorOptions```</summary>```RhoMicro.BindSight.Generators.MutableJsonSchemaGeneratorOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_32"></a>
##### UnmappedMemberHandling <a id="header_33"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:UnmappedMemberHandling` |
| Type | <details><summary>```JsonUnmappedMemberHandling```</summary>```System.Text.Json.Serialization.JsonUnmappedMemberHandling```</details> |
| Allowed Values | `"Skip"`(`0`), `"Disallow"`(`1`) |
| Disallowed Values | `null` |
##### IndentCharacter <a id="header_34"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IndentCharacter` |
| Type | ``char`` |
| Disallowed Values | `null` |
##### IndentSize <a id="header_35"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IndentSize` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### TreatNullObliviousAsNonNullable <a id="header_36"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSchemaExporterOptions:TreatNullObliviousAsNonNullable` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### ReadCommentHandling <a id="header_37"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:ReadCommentHandling` |
| Type | <details><summary>```JsonCommentHandling```</summary>```System.Text.Json.JsonCommentHandling```</details> |
| Allowed Values | `"Disallow"`(`0`), `"Skip"`(`1`), `"Allow"`(`2`) |
| Disallowed Values | `null` |
##### IncludeFields <a id="header_38"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IncludeFields` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### DefaultIgnoreCondition <a id="header_39"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:DefaultIgnoreCondition` |
| Type | <details><summary>```JsonIgnoreCondition```</summary>```System.Text.Json.Serialization.JsonIgnoreCondition```</details> |
| Allowed Values | `"Never"`(`0`), `"Always"`(`1`), `"WhenWritingDefault"`(`2`), `"WhenWritingNull"`(`3`), `"WhenWriting"`(`4`), `"WhenReading"`(`5`) |
| Disallowed Values | `null` |
##### JsonSchemaExporterOptions <a id="header_40"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSchemaExporterOptions` |
| Type | <details><summary>```JsonSchemaExporterOptions```</summary>```System.Text.Json.Schema.JsonSchemaExporterOptions```</details> |
| Disallowed Values | `null` |
##### AllowTrailingCommas <a id="header_41"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:AllowTrailingCommas` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### AllowOutOfOrderMetadataProperties <a id="header_42"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:AllowOutOfOrderMetadataProperties` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### MaxDepth <a id="header_43"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:MaxDepth` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### NumberHandling <a id="header_44"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:NumberHandling` |
| Type | <details><summary>```JsonNumberHandling``` (flags)</summary>```System.Text.Json.Serialization.JsonNumberHandling```</details> |
| Allowed Values | `"Strict"`(`0`), `"AllowReadingFromString"`(`1`), `"WriteAsString"`(`2`), `"AllowNamedFloatingPointLiterals"`(`4`) |
| Disallowed Values | `null` |
##### IgnoreReadOnlyFields <a id="header_45"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IgnoreReadOnlyFields` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### NewLine <a id="header_46"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:NewLine` |
| Type | ``string`` |
| Disallowed Values | `null` |
##### TypeInfoResolver <a id="header_47"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:TypeInfoResolver` |
| Type | <details><summary>```IJsonTypeInfoResolver```</summary>```System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver```</details> |
| Allowed Values | `null` |
##### IgnoreReadOnlyProperties <a id="header_48"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IgnoreReadOnlyProperties` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### OutputDirectory <a id="header_49"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:OutputDirectory` |
| Type | ``string`` |
| Description | Sets the directory to generate json schemata of options into. |
| Disallowed Values | `null` |
##### PreferredObjectCreationHandling <a id="header_50"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:PreferredObjectCreationHandling` |
| Type | <details><summary>```JsonObjectCreationHandling```</summary>```System.Text.Json.Serialization.JsonObjectCreationHandling```</details> |
| Allowed Values | `"Replace"`(`0`), `"Populate"`(`1`) |
| Disallowed Values | `null` |
##### IgnoreNullValues <a id="header_51"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:IgnoreNullValues` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### JsonSerializerOptions <a id="header_52"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions` |
| Type | <details><summary>```JsonSerializerOptions```</summary>```System.Text.Json.JsonSerializerOptions```</details> |
| Disallowed Values | `null` |
##### TypeInfoResolverChain <a id="header_53"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:TypeInfoResolverChain` |
| Type | <details><summary>```IList<IJsonTypeInfoResolver>```</summary>```System.Collections.Generic.IList<System.Text.Json.Serialization.Metadata.IJsonTypeInfoResolver>```</details> |
| Disallowed Values | `null` |
##### PropertyNameCaseInsensitive <a id="header_54"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:PropertyNameCaseInsensitive` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### DefaultBufferSize <a id="header_55"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:DefaultBufferSize` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### UnknownTypeHandling <a id="header_56"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:UnknownTypeHandling` |
| Type | <details><summary>```JsonUnknownTypeHandling```</summary>```System.Text.Json.Serialization.JsonUnknownTypeHandling```</details> |
| Allowed Values | `"JsonElement"`(`0`), `"JsonNode"`(`1`) |
| Disallowed Values | `null` |
##### RespectNullableAnnotations <a id="header_57"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:RespectNullableAnnotations` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### RespectRequiredConstructorParameters <a id="header_58"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:RespectRequiredConstructorParameters` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
##### WriteIndented <a id="header_59"></a>
|  |  |
| -- | -- |
| Key | `JsonSchemaGeneratorOptions:JsonSerializerOptions:WriteIndented` |
| Type | ``bool`` |
| Allowed Values | `true`, `false` |
| Disallowed Values | `null` |
### ReadmeGeneratorOptions <a id="header_60"></a>
|  |  |
| -- | -- |
| Key | `ReadmeGeneratorOptions` |
| Type | <details><summary>```MutableReadmeGeneratorOptions```</summary>```RhoMicro.BindSight.Generators.MutableReadmeGeneratorOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_61"></a>
##### Title <a id="header_62"></a>
|  |  |
| -- | -- |
| Key | `ReadmeGeneratorOptions:Title` |
| Type | ``string`` |
| Description | Sets the title of the readme file. The default is `Options`. |
| Disallowed Values | `null` |
##### OutputFile <a id="header_63"></a>
|  |  |
| -- | -- |
| Key | `ReadmeGeneratorOptions:OutputFile` |
| Type | ``string`` |
| Description | Sets the path of the readme file to generate options docs into. The default is `Options/Readme.md`. |
| Disallowed Values | `null` |
### Test <a id="header_64"></a>
|  |  |
| -- | -- |
| Key | `Test` |
| Type | <details><summary>```TestOptions```</summary>```RhoMicro.BindSight.ExploratoryTests.TestOptions```</details> |
| Disallowed Values | `null` |
#### Properties <a id="header_65"></a>
##### StringList <a id="header_66"></a>
|  |  |
| -- | -- |
| Key | `Test:StringList` |
| Type | <details><summary>```List<string>```</summary>```System.Collections.Generic.List<string>```</details> |
| Disallowed Values | `null` |
##### Int32Property <a id="header_67"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedSet:[n]:Int32Property` |
| Type | ``int`` |
| Disallowed Values | `null` |
##### NestedSet <a id="header_68"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedSet` |
| Type | <details><summary>```HashSet<TestNestedOptions>```</summary>```System.Collections.Generic.HashSet<RhoMicro.BindSight.ExploratoryTests.TestNestedOptions>```</details> |
| Disallowed Values | `null` |
##### NestedProperty <a id="header_69"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedProperty` |
| Type | <details><summary>```TestNestedOptions```</summary>```RhoMicro.BindSight.ExploratoryTests.TestNestedOptions```</details> |
| Disallowed Values | `null` |
##### CircularReference <a id="header_70"></a>
|  |  |
| -- | -- |
| Key | `Test:NestedSet:[n]:CircularReference` |
| Type | <details><summary>```TestNestedOptions```</summary>```RhoMicro.BindSight.ExploratoryTests.TestNestedOptions```</details> |
| Disallowed Values | `null` |
