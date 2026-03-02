# Regras de Negócio

| ID | Regra | Implementação |
|---|---|---|
| RN01 | Todo lançamento deve ter valor > 0 | `Lancamento.Criar()` valida `valor > 0` → `DomainException` |
| RN02 | Lançamento deve ter data; default = data atual | `DataLancamento = dataLancamento ?? DateOnly.FromDateTime(DateTime.UtcNow)` |
| RN03 | Lançamento deve ser DEBIT ou CREDIT | Enum `TipoLancamento` com `FluentValidation` |
| RN04 | Saldo consolidado = Σ Créditos - Σ Débitos do dia | `Saldo => TotalCreditos - TotalDebitos` (computed) |
| RN05 | Lançamentos cancelados não podem ser editados | `Lancamento.Atualizar()` verifica `Status == CANCELLED` |
| RN06 | Estorno gera novo lançamento inverso com rastreabilidade | Cancelar gera `LancamentoCanceladoEvent` → Consolidado reverter |
| RN07 | Idempotência — requisições duplicadas não geram lançamentos duplicados | Verificação por `IdempotencyKey` antes de criar |

## Detalhamento

### RN01 - Validação de Valor
```csharp
if (valor <= 0)
    throw new DomainException("RN01", "O valor do lançamento deve ser maior que zero.");
```

### RN04 - Cálculo de Saldo
```csharp
public decimal Saldo => TotalCreditos - TotalDebitos;
```

### RN07 - Idempotência
O header `Idempotency-Key` (UUID) é verificado antes de criar o lançamento.
Se um lançamento com a mesma chave já existe, retorna o existente sem criar duplicata.
```csharp
var existing = await _repository.GetByIdempotencyKeyAsync(request.IdempotencyKey);
if (existing != null) return MapToResponse(existing);
```
