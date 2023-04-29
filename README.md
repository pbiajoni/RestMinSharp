# RestMinSharp

`RestMinSharp` is a .NET library that provides a simple and intuitive interface to work with RESTful APIs. It uses the popular RestSharp library under the hood to perform HTTP requests.

## RequestResult<T>

`RequestResult<T>` is a generic class that represents the result of an HTTP request. It contains the following properties:

- `RawData` (object): the raw data returned by the server.
- `IsAuthorized` (bool): a flag that indicates if the user is authorized to access the requested resource.
- `Notifications` (List<Notification>): a list of notifications that may have been generated during the request.
- `HasNotifications` (bool): a flag that indicates if there are any notifications.
- `Data` (T): the deserialized data returned by the server.

## ERequestResult<T, E>

`ERequestResult<T, E>` is a generic class that extends `RequestResult<T>` and adds an `Error` property of type `E`. This class is useful when you want to include additional error information in the response.

## IRestMinSharpContext

`IRestMinSharpContext` is an interface that defines the methods for performing HTTP requests. The methods include:

- `AddBearerToken(string token)`: adds a bearer token to the request headers.
- `DeleteAsync<T>(string url)`: sends a DELETE request and returns a `RequestResult<T>`.
- `GetAsync<T>(string url)`: sends a GET request and returns a `RequestResult<T>`.
- `GetAsync<T, E>(string url)`: sends a GET request and returns an `ERequestResult<T, E>`.
- `GetStreamAsync(string url)`: sends a GET request and returns a `MemoryStreamRequestResult`.
- `PatchAsync<T>(string url, List<PatchOperation> operations)`: sends a PATCH request with a list of `PatchOperation` objects and returns a `RequestResult<T>`.
- `PatchAsync<T>(string url, PatchOperation operations)`: sends a PATCH request with a single `PatchOperation` object and returns a `RequestResult<T>`.
- `PostAsync<T>(string url, object payload)`: sends a POST request with an object payload and returns a `RequestResult<T>`.
- `PostAsync<T>(string url, string json)`: sends a POST request with a JSON payload and returns a `RequestResult<T>`.
- `PostAsync<T, E>(string url, object payload)`: sends a POST request with an object payload and returns an `ERequestResult<T, E>`.
- `PostAsync<T>(string url)`: sends a POST request without a payload and returns a `RequestResult<T>`.
- `PostAsync<T, E>(string url)`: sends a POST request without a payload and returns an `ERequestResult<T, E>`.
- `PutAsync<T>(string url)`: sends a PUT request without a payload and returns a `RequestResult<T>`.
- `PutAsync<T>(string url, object payload)`: sends a PUT request with an object payload and returns a `RequestResult<T>`.
- `PutAsync<T, E>(string url, object payload)`: sends a PUT request with an object payload and returns an `ERequestResult<T, E>`.
- `SerializeObject(object obj)`: serializes an object to JSON.
- `SetBearerToken(string token)`: sets a bearer token for all future requests.
- `Upload<T>(string url, string name, byte[] bytes, Method method)`: uploads a file and returns a `RequestResult<T>`.
