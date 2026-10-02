using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PubKarelia.Client.Game;

// Built upon the 'Result' -pattern
// https://codewithmukesh.com/blog/result-pattern-dotnet/
// https://www.linkedin.com/pulse/result-pattern-c-comprehensive-guide-andre-baltieri-wieuf

// Originally, it had a interface but I think it is unnecessary complication at this time.
// I will not place any constraints on the type, since the type can vary from simple data types into class structures.
public sealed class ApiResult<TValue>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess; // We'll always ensure it is the opposite of IsSuccess. Is Success -> true makes this false.

    // We'll use HttpStatusCode enum instead so we have better typing and still access to int values.
    public HttpStatusCode StatusCode { get; init; }
    public string? ErrorMessage { get; init; } = null;


    private TValue? _data;

    // Data is the models that we deserialize from the responses, so we are required to keep this generic.
    // How do we deal with a situation when the data they access is actually null or something? should they always just check?
    public TValue? Data
    {
        get
            => IsSuccess ? _data : throw new InvalidOperationException("Trying to access data that does not exist.");

        init
            => _data = value;
    }

    private ApiResult(bool isSuccess, HttpStatusCode statusCode, TValue? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
    }   

    // 200 is the most common, but there is also 201 and I feel like I am forgetting something else as well, but let's make that default.
    public static ApiResult<TValue> Success(TValue? data, HttpStatusCode statusCode = HttpStatusCode.OK)
    => new(isSuccess: true, statusCode: statusCode, data: data, errorMessage: null);

    // We have to return default of the data type for now.
    public static ApiResult<TValue> Failure(HttpStatusCode statusCode, string errorMessage)
    => new(isSuccess: false, statusCode: statusCode, data: default, errorMessage: errorMessage);
}
