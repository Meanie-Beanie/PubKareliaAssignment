using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Game;

// Built upon the 'Result' -pattern


// Originally, it had a interface but I think it is unnecessary complication at this time.
// I will not place any constraints on the type, since the type can vary from simple data types into class structures.
public sealed class Result<T>
{
    public bool IsSuccess { get; init; }
    public int StatusCode { get; init; }
    public string? ErrorMessage { get; init; }

    // Data is the models that we deserialize from the responses, so we are required to keep this generic.
    public T? Data { get; init; }

    public Result(bool isSuccess, int errorCode, T? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        StatusCode = errorCode;
        Data = data;
        ErrorMessage = errorMessage;
    }
}
