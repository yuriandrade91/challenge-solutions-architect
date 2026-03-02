# Requisitos Não Funcionais

| ID | Requisito | Meta | Implementação |
|---|---|---|---|
| RNF01 | Disponibilidade Lançamentos | 99.9% uptime | Docker health checks, independent service |
| RNF02 | Independência entre serviços | Lançamentos funciona se Consolidado cair | RabbitMQ async, sem chamadas síncronas |
| RNF03 | Throughput Consolidado | 50 req/s com ≤5% perda | Redis cache, NGINX rate limiting |
| RNF04 | Latência | p95 < 200ms lançamentos, p95 < 500ms consolidado | Cache, índices PostgreSQL |
| RNF05 | Segurança | TLS, JWT, criptografia | NGINX TLS termination |
| RNF06 | Observabilidade | Logs, métricas, tracing | OpenTelemetry, Prometheus, Grafana |
| RNF07 | Consistência eventual | entre Lançamentos e Consolidado | RabbitMQ events |
| RNF08 | Testabilidade | Cobertura mínima 80% | xUnit, Testcontainers, k6 |

## Resiliência (Polly)
- **Retry com Exponential Backoff + Jitter**: 3 tentativas, aguardando 1s, 2s, 4s
- **Circuit Breaker**: abre após 50% falhas em 30s, fecha após 30s
- **Timeout**: máximo 30 segundos por operação
- **Bulkhead**: isolamento de recursos críticos
