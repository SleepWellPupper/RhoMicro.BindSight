---
title: Class ServiceCollectionExtensions
description: The reference documentation for the RhoMicro.BindSight.ServiceCollectionExtensions type.
---

Provides extension methods for [IServiceCollection](https://duckduckgo.com/?q=Microsoft.Extensions.DependencyInjection.IServiceCollection) .

```cs
public class ServiceCollectionExtensions
```

Namespace: RhoMicro.BindSight


## Members

### Methods

#### AddBindSight <a id="AddBindSight-0-0p0-Microsoft-Extensions-DependencyInjection-IServiceCollection-0-0p1"></a>

Adds options docs services to the service collection.

```cs
public static BindSightBuilder AddBindSight(this IServiceCollection services)
```
#### AddDocumentedOptions <a id="AddDocumentedOptions-1-0p0-Microsoft-Extensions-DependencyInjection-IServiceCollection-0_System-String-0_System-Action-1-0g0-Microsoft-Extensions-Configuration-BinderOptions-0-0g1-0p1"></a>

Gets an options builder that forwards Configure calls for the    same [TOptions]() to the underlying service    collection. Documentation services for the options are    registered as well.

```cs
public static OptionsBuilder<TOptions> AddDocumentedOptions<TOptions>(this IServiceCollection services, string configSectionPath, [Action<BinderOptions>? configureBinder = null]) where TOptions : class
```
#### AddDocumentedOptions <a id="AddDocumentedOptions-1-0p0-Microsoft-Extensions-DependencyInjection-IServiceCollection-0_System-String-0_System-String-0_System-Action-1-0g0-Microsoft-Extensions-Configuration-BinderOptions-0-0g1-0p1"></a>

Gets an options builder that forwards Configure calls for the    same [TOptions]() to the underlying service    collection. Documentation services for the options are    registered as well.

```cs
public static OptionsBuilder<TOptions> AddDocumentedOptions<TOptions>(this IServiceCollection services, string name, string configSectionPath, [Action<BinderOptions>? configureBinder = null]) where TOptions : class
```
