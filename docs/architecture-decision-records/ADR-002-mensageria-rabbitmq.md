# ADR-002: Uso de RabbitMQ para Comunicação Assíncrona

## Status
Aceito

## Contexto
Os dois microsserviços precisam se comunicar: quando um lançamento é criado no serviço de Lançamentos, o serviço de Consolidado precisa ser notificado para atualizar o saldo diário.

## Decisão
Usar **RabbitMQ** como message broker para comunicação assíncrona entre os serviços.

## Justificativa

### Por que Comunicação Assíncrona?
- Garante que o Lançamentos continue operando mesmo se o Consolidado estiver indisponível
- Mensagens são enfileiradas e processadas quando o Consolidado voltar
- Filas duráveis garantem que nenhuma mensagem seja perdida

### Por que RabbitMQ?
- **Filas duráveis**: mensagens persistidas em disco
- **Dead Letter Queue (DLQ)**: mensagens com falha são redirecionadas para análise
- **AMQP protocol**: protocolo robusto e amplamente testado
- **Retry automático**: suporte nativo a reprocessamento
- **Management UI**: interface web para monitoramento (porta 15672)
- **Open-source**: sem custos de licença

### Padrão Utilizado: Topic Exchange
- Exchange: `fluxocaixa.lancamentos`
- Routing keys: `lancamentocriadoevent`, `lancamentocanceladoevent`, `lancamentoatualizadoevent`
- Queue: `consolidado.lancamentos`

### Alternativas Consideradas
| Broker | Vantagem | Desvantagem |
|---|---|---|
| **RabbitMQ** | Filas duráveis, DLQ, AMQP | Requer gerenciamento |
| Apache Kafka | Alto throughput, replay | Complexidade, overhead para este volume |
| Azure Service Bus | Managed service | Vendor lock-in, custo |

## Consequências
- Consistência eventual entre Lançamentos e Consolidado
- Necessidade de implementar idempotência no consumer
- Monitoramento adicional do broker
