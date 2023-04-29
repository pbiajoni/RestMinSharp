## RestMinSharp

RestMinSharp is a .NET library that provides an easy-to-use interface for making HTTP requests to RESTful APIs. It is built on top of the RestSharp library and aims to simplify the usage of RestSharp, especially when dealing with common use cases.

### Classes

#### `RequestResult<T>`

The `RequestResult<T>` class is the base class for all result types returned by RestMinSharp. It provides the following properties:

- `RawData`: the raw data returned by the API (can be of any type)
- `IsAuthorized`: a boolean indicating whether the request was authorized or not
- `Notifications`: a list of notifications, which can be used to provide additional information about the request result
- `HasNotifications`: a boolean indicating whether the request result has any notifications
- `Data`: the deserialized response data (of type `T`)

#### `ERequestResult<T, E>`

The `ERequestResult<T, E>` class extends the `RequestResult<T>` class and adds an `Error` property of type `E`. It can be used when the API returns both response data and error information.

#### `IRestMinSharpContext`

The `IRestMinSharpContext` interface defines the main interface of RestMinSharp. It provides the following methods:

- `AddBearerToken`: adds a bearer token to all requests made through the context
- `DeleteAsync<T>(string url)`: sends a DELETE request to the specified URL and returns a `RequestResult<T>` object
- `GetAsync<T>(string url)`: sends a GET request to the specified URL and returns a `RequestResult<T>` object
- `GetAsync<T, E>(string url)`: sends a GET request to the specified URL and returns an `ERequestResult<T, E>` object
- `GetStreamAsync(string url)`: sends a GET request to the specified URL and returns a `MemoryStreamRequestResult` object, which contains the response as a `MemoryStream`
- `PatchAsync<T>(string url, List<PatchOperation> operations)`: sends a PATCH request with a list of patch operations to the specified URL and returns a `RequestResult<T>` object
- `PatchAsync<T>(string url, PatchOperation operations)`: sends a PATCH request with a single patch operation to the specified URL and returns a `RequestResult<T>` object
- `PostAsync<T>(string url, object payload)`: sends a POST request with a payload object to the specified URL and returns a `RequestResult<T>` object
- `PostAsync<T>(string url, string json)`: sends a POST request with a JSON payload to the specified URL and returns a `RequestResult<T>` object
- `PostAsync<T, E>(string url, object payload)`: sends a POST request with a payload object to the specified URL and returns an `ERequestResult<T, E>` object
- `PostAsync<T>(string url)`: sends a POST request without a payload to the specified URL and returns a `RequestResult<T>` object
- `PostAsync<T, E>(string url)`: sends a POST request without a payload to the specified URL and returns an `ERequestResult<T, E>` object
- `PutAsync<T>(string url)`: sends a PUT request without a payload to the specified URL and returns a `RequestResult<T>` object
- `PutAsync<T>(string url, object payload)`: sends a PUT request with a payload object to the specified URL and returns a `RequestResult<T>` object
- `PutAsync<T, E>(string url, object payload)`: sends a PUT request with a payload object to the specified

## Requirements

- .NET Standard 2.0
- RestSharp 106.11.7 or higher

## Installation

RestMinSharp is available as a NuGet package. You can install it using the NuGet Package Manager in Visual Studio or via the command line:

```powershell
Install-Package RestMinSharp
```

## Usage

RestMinSharp provides an interface called `IRestMinSharpContext` that defines a set of methods to interact with RESTful APIs. You can create an instance of this interface using the `RestMinSharpContext` class:

```csharp
IRestMinSharpContext context = new RestMinSharpContext("https://api.example.com/");
```

Once you have an instance of `IRestMinSharpContext`, you can use its methods to send HTTP requests:

```csharp
var result = await context.GetAsync<MyModel>("my-endpoint");
```

The library provides support for various HTTP methods and supports deserialization of JSON responses to your models. RestMinSharp also includes an implementation of the `IRequestResult<T>` interface, which is a wrapper around the RestSharp `IRestResponse` class that provides additional properties such as `IsAuthorized`, `Notifications`, and `Data`.
