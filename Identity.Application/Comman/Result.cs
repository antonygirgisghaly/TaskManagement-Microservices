using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Comman
{
    public class Result<T> 
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; } 
        public string? Error { get; set; }

        public Result(bool isSuccess, T? data = default!, string? error = default!)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static Result<T> Success(T data) => new Result<T>(true, data);
        public static Result<T> Failure(string error) => new Result<T>(false,default!, error);
    }
}
