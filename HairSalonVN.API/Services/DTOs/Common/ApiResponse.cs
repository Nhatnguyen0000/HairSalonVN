using System;

namespace HairSalonVN.API.Services.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null)
            => new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(string message)
            => new() { Success = false, Message = message, Errors = new() { message } };

        public static ApiResponse<T> Fail(List<string> errors)
            => new() { Success = false, Errors = errors, Message = errors.FirstOrDefault() };

        public static ApiResponse<T> Fail(string message, List<string> errors)
            => new() { Success = false, Message = message, Errors = errors };
    }
}
