﻿/**
# Documentation

## Defining entities's @context and @frame

To correctly serialize and deserialize JSON-LD documents into given models it may be necessary to process them with a `@context` or `@frame`
so that it's structure conforms to a given schema. This page lists the ways in which it is possible to define `@context` and `@frame` for
the C# types.

First let's import the required namespaces.
 **/

using System;
using JsonLD.Entities;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

/**
### Define `@context` inline with class definition
**/

[TestFixture]
public class DefiningContextInline
{

private const string TestContext = "{ '@base': 'http://example.com/' }";

/**
The simples way is to define a `Context` property. It must be static and return a string or `JToken`. It can also be private. Another way
is to declare a `GetContext` method, which takes an object as parameter. This way a @context can be built dynamically.
    
Below are example entity types with various ways of defining the `@context` inline.
**/

public class ContextInlineProperty
{
    public static JObject Context
    {
        get { return JObject.Parse(TestContext); }
    }
}

public class ContextInlinePrivateProperty
{
    private static JObject Context
    {
        get { return JObject.Parse(TestContext); }
    }
}

public class ContextInlineStaticStringProperty
{
    public static string Context
    {
        get { return TestContext; }
    }
}

public class ContextInlineMethod
{
    private static JObject GetContext(ContextInlineMethod instance)
    {
        return JObject.Parse(TestContext);
    }
}

/**
When each of the above will be serailized, the inline context will be used.
**/

[TestCase(typeof(ContextInlineProperty))]
[TestCase(typeof(ContextInlineProperty))]
[TestCase(typeof(ContextInlinePrivateProperty))]
[TestCase(typeof(ContextInlineStaticStringProperty))]
[TestCase(typeof(ContextInlineMethod))]
public void UsesInlineContextWhenSerializing(Type entityType)
{
    // given
    const string expected = "{ '@context': { '@base': 'http://example.com/' } }";
    var entity = Activator.CreateInstance(entityType);
    var serializer = new EntitySerializer();

    // when
    var json = serializer.Serialize(entity);

    // then
    Assert.That(JToken.DeepEquals(json, JObject.Parse(expected)), "Actual object is {0}", json);
}
}