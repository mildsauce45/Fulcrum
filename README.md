# Fulcrum

A library for turning your web apis into interfaces

## Description

A few years back I happened upon Paul Betts [Refit](https://github.com/reactiveui/refit) library and thought it was cool. I decided to see if I could implement something similar myself but using the [Emitter](https://github.com/mildsauce45/Emitter) project I myself wrote 'lo those many years ago. Hence Fulcrum was born

## Basic Usage

```C#
public interface IEchoApi
{
    [Get("public")]
    Task<Response> Get();
    
    [Get("public/{route}")]
    Task<Response> GetResource(string route, [QueryParams] IDictionary<string, string> queryMap);
    
    [Post("private")]
    Task<Response> Post([Body] Set set);
}

var api = Connect.To<IEchoApi>("https://localhost/echoservice", new SimpleOAuthProvider());

var response = await api.Get();
var response = await api.GetResource("foo", new Dictionary<string, string> { { "pretty", "true" } } );
var response = await api.Post(new Set { Code = "aer", Name = "Aether Revole" });
```
