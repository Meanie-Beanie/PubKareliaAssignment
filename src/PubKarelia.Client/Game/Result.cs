using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Game;

// Built upon the 'Result' -pattern
// https://codewithmukesh.com/blog/result-pattern-dotnet/
// https://www.linkedin.com/pulse/result-pattern-c-comprehensive-guide-andre-baltieri-wieuf

// Originally, it had a interface but I think it is unnecessary complication at this time.
// I will not place any constraints on the type, since the type can vary from simple data types into class structures.
public sealed class Result<TValue>
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess; // We'll always ensure it is the opposite of IsSuccess. Is Success -> true makes this false.

    public int StatusCode { get; init; }
    public string? ErrorMessage { get; init; } = null;


    private TValue? _data;

    // Data is the models that we deserialize from the responses, so we are required to keep this generic.
    public TValue? Data
    {
        get
            => IsSuccess ? _data : throw new InvalidOperationException("Trying to access data that does not exist.");

        init
            => _data = value;
    }

    private Result(bool isSuccess, int statusCode, TValue? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
    }   

    public static Result<TValue> Success(TValue data)
    => new(isSuccess: true, statusCode: 200, data: data, errorMessage: null);

    // We have to return default of the data type for now bu
    public static Result<TValue> Failure(TValue data, int statusCode, string errorMessage)
    => new(isSuccess: false, statusCode: statusCode, data: default, errorMessage: errorMessage);
}
