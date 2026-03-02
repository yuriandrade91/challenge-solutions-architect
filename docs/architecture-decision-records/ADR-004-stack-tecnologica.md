# ADR-004: Justificativa da Stack Tecnológica

## Status
Aceito

## Decisão
Adotar a seguinte stack tecnológica:

| Componente | Tecnologia | Versão |
|---|---|---|
| Linguagem | C# / .NET 8 | 8.0 LTS |
| Banco de Dados | PostgreSQL | 16 |
| Cache | Redis | 7 |
| Mensageria | RabbitMQ | 3 |
| API Gateway | NGINX | Alpine |
| Containers | Docker + Docker Compose | Latest |
| Testes Unitários | xUnit + FluentAssertions + Moq | Latest |
| Testes Integração | Testcontainers | 4.x |
| Testes de Carga | k6 | Latest |
| Resiliência | Polly | 8.x |
| ORM | Entity Framework Core | 8.0 |
| Validação | FluentValidation | 11.x |
| Mediator | MediatR | 12.x |

## Justificativas

### C# / .NET 8
- LTS (Long Term Support) com suporte até novembro de 2026
- Performance: top 3 em benchmarks TechEmpower
- Tipagem forte: reduz bugs em runtime
- Ecossistema maduro: NuGet com milhares de pacotes

### PostgreSQL 16
- Open-source e gratuito
- ACID compliant: garantia de transações financeiras corretas
- Suporte a tipos complexos (UUID, JSONB, Arrays)
- Excelente performance com índices

### Redis 7
- Sub-milissegundo de latência para leituras
- Resolve RNF03: 50 req/s no consolidado com p95 < 500ms
- TTL configurável: invalida cache após mudanças
- Open-source, sem necessidade de cluster para este volume

### RabbitMQ 3
- Ver ADR-002

### NGINX
- Rate limiting nativo: protege contra sobrecarga
- Load balancing: distribui carga entre instâncias
- Configuração simples para proxy reverso
- Amplamente usado em produção

### Polly (Resiliência)
- Retry com exponential backoff e jitter
- Circuit Breaker: evita cascata de falhas
- Timeout: evita travamentos
- Bulkhead: isolamento de recursos

## Consequências
- Stack totalmente open-source (sem vendor lock-in)
- Curva de aprendizado para desenvolvedores C#/.NET
- Necessidade de gerenciar múltiplos containers
