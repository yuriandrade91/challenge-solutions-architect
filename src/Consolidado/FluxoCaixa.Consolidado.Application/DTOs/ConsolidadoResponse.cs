namespace FluxoCaixa.Consolidado.Application.DTOs;

public record ConsolidadoResponse(
    Guid Id,
    DateOnly Data,
    decimal TotalCreditos,
    decimal TotalDebitos,
    decimal Saldo,
    int QuantidadeLancamentos,
    DateTime UpdatedAt
);

public record ApiResponse<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }
    public MetadataInfo? Metadata { get; init; }

    public static ApiResponse<T> Ok(T data, MetadataInfo? metadata = null)
        => new() { Success = true, Data = data, Metadata = metadata };

    public static ApiResponse<T> Fail(string code, string message)
        => new() { Success = false, Error = new ApiError(code, message) };
}

public record ApiError(string Code, string Message);
public record MetadataInfo(string RequestId, DateTime Timestamp, bool? CacheHit = null);
