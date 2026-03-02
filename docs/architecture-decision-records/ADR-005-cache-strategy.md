# ADR-005: Estratégia de Cache com Redis

## Status
Aceito

## Contexto
O serviço de Consolidado recebe **50 requisições por segundo** com no máximo 5% de perda. Para atender esse throughput com latência p95 < 500ms, é necessária uma estratégia de cache eficiente.

## Decisão
Implementar **Cache-Aside (Lazy Loading)** com Redis para os endpoints do Consolidado.

## Estratégia

### Cache-Aside Pattern
```
1. Request chega → Verifica Redis
2. Cache HIT → Retorna dados do cache (< 10ms)
3. Cache MISS → Busca no PostgreSQL → Armazena no Redis → Retorna dados
```

### Chaves de Cache
- `consolidado:{yyyy-MM-dd}` — dados do consolidado por data (TTL: 5 minutos)
- Invalidação: quando um LancamentoCriadoEvent ou LancamentoCanceladoEvent é processado

### TTL (Time-to-Live)
- **5 minutos** para dados do consolidado: balanço entre consistência e performance
- Dados do dia atual são mais voláteis; considera-se aceitável consistência eventual de 5 min

### Por que Cache-Aside vs Write-Through?
- **Write-Through**: atualiza cache na escrita — mas a escrita é em outro serviço (Lançamentos)
- **Cache-Aside**: mais adequado quando leitura e escrita são em serviços distintos
- Consistência eventual é aceita (RNF07)

## Consequências
- Leituras repetidas do mesmo dia são atendidas em < 10ms pelo Redis
- Primeiro acesso após invalidação tem latência maior (miss no cache)
- Dados podem ter até 5 minutos de atraso (aceitável pela arquitetura)
- Resiliência: se Redis cair, sistema continua funcionando com PostgreSQL (degradação graciosa)
