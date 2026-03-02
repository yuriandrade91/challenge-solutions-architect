# ADR-001: Adoção de Arquitetura de Microsserviços

## Status
Aceito

## Contexto
O sistema de controle de fluxo de caixa precisa atender a dois domínios independentes:
1. **Controle de Lançamentos**: CRUD de transações financeiras com alta disponibilidade
2. **Consolidado Diário**: Relatórios de saldo consolidado com suporte a 50 req/s

O principal requisito não-funcional é que **o serviço de lançamentos NÃO deve ficar indisponível se o sistema de consolidado cair**.

## Decisão
Adotar arquitetura de **Microsserviços** com dois serviços independentes:
- **FluxoCaixa.Lancamentos.Api** (porta 5001)
- **FluxoCaixa.Consolidado.Api** (porta 5002)

## Justificativa

### Benefícios
- **Independência de falhas**: se o Consolidado cair, o Lançamentos continua funcionando
- **Deploy independente**: cada serviço pode ser deployado separadamente
- **Escalabilidade independente**: o Consolidado pode escalar horizontalmente para atender 50 req/s
- **Times independentes**: equipes podem trabalhar em paralelo

### Alternativas Consideradas

| Abordagem | Vantagem | Desvantagem |
|---|---|---|
| Monolito | Simples, menos latência | Viola RNF02: acoplamento forte entre serviços |
| Monolito Modular | Simples com separação | Ainda compartilha processo, viola disponibilidade |
| **Microsserviços** | Isolamento total, independência | Complexidade operacional |

## Consequências
- Necessidade de gerenciar comunicação assíncrona (RabbitMQ)
- Consistência eventual entre serviços
- Infraestrutura mais complexa (Docker Compose, service discovery)
