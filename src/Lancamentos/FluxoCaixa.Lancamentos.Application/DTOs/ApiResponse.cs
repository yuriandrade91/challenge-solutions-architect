namespace FluxoCaixa.Lancamentos.Application.DTOs;

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }
    public PaginationInfo? Pagination { get; init; }
    public MetadataInfo? Metadata { get; init; }

    public static ApiResponse<T> Ok(T data, PaginationInfo? pagination = null, MetadataInfo? metadata = null)
        => new() { Success = true, Data = data, Pagination = pagination, Metadata = metadata };

    public static ApiResponse<T> Fail(string code, string message, IEnumerable<string>? details = null)
        => new() { Success = false, Error = new ApiError(code, message, details?.ToList() ?? []) };
}

public record ApiError(string Code, string Message, List<string> Details);

public record PaginationInfo(int Page, int PageSize, int TotalItems, int TotalPages);

public record MetadataInfo(string RequestId, DateTime Timestamp, bool? CacheHit = null);
