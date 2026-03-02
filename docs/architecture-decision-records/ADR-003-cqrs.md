# ADR-003: Adoção do Padrão CQRS

## Status
Aceito

## Contexto
O sistema tem características distintas de leitura e escrita:
- **Escritas**: criar, atualizar, cancelar lançamentos (baixa frequência, requer validação)
- **Leituras**: consultar lançamentos, consolidados (alta frequência, 50 req/s no consolidado)

## Decisão
Implementar **CQRS (Command Query Responsibility Segregation)** usando MediatR como mediador.

## Justificativa

### Benefícios
- **Separação clara de responsabilidades**: Commands (escrita) vs Queries (leitura)
- **Otimização independente**: queries podem usar cache (Redis), commands garantem consistência
- **Testabilidade**: handlers são testados independentemente
- **Extensibilidade**: novos comportamentos via pipeline behaviors (logging, validation)

### Implementação com MediatR
```
Commands (escrita):
  CriarLancamentoCommand → CriarLancamentoCommandHandler
  AtualizarLancamentoCommand → AtualizarLancamentoCommandHandler
  CancelarLancamentoCommand → CancelarLancamentoCommandHandler

Queries (leitura):
  GetLancamentosQuery → GetLancamentosQueryHandler
  GetLancamentoByIdQuery → GetLancamentoByIdQueryHandler
  GetConsolidadoByDataQuery → GetConsolidadoByDataQueryHandler (com cache Redis)
  GetConsolidadoPorPeriodoQuery → GetConsolidadoPorPeriodoQueryHandler
```

### Alternativas Consideradas
- **Repository Pattern simples**: sem separação de leitura/escrita — não aproveita cache
- **CQRS com banco separado**: separação física de bancos — overhead desnecessário para este volume

## Consequências
- Mais classes (Commands, Handlers, Queries) — código mais estruturado
- Necessidade de biblioteca MediatR
- Facilidade para adicionar cross-cutting concerns (logging, timing, tracing)
