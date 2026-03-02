# Requisitos Funcionais

## RF01 — Registrar Lançamento de Débito
O sistema deve permitir registrar lançamentos do tipo **DEBIT** com valor, descrição, categoria e data.

## RF02 — Registrar Lançamento de Crédito
O sistema deve permitir registrar lançamentos do tipo **CREDIT** com valor, descrição, categoria e data.

## RF03 — Atributos de Lançamento
Cada lançamento deve conter:
- `valor`: valor monetário (> 0)
- `tipo`: CREDIT ou DEBIT
- `descricao`: descrição textual (obrigatória, max 500 chars)
- `data_lancamento`: data do lançamento (default: data atual)
- `categoria`: categoria opcional (max 100 chars)
- `idempotency_key`: UUID único para evitar duplicações

## RF04 — Consultar Lançamentos por Período
O sistema deve permitir consultar lançamentos filtrando por `dataInicio` e `dataFim`, com paginação (`page`, `pageSize`).

## RF05 — Editar Lançamento
O sistema deve permitir editar valor, descrição e categoria de um lançamento **ACTIVE**.

## RF06 — Cancelar Lançamento (Soft Delete)
O sistema deve permitir cancelar um lançamento (marcar como CANCELLED) sem removê-lo do banco.

## RF07 — Consolidar Saldo Diário Automaticamente
O serviço de Consolidado deve atualizar automaticamente o saldo diário ao receber eventos de novos lançamentos via RabbitMQ.

## RF08 — Relatório de Saldo Consolidado por Dia
O sistema deve retornar o saldo consolidado de uma data específica.

## RF09 — Campos do Relatório Consolidado
O relatório deve incluir: `data`, `total_creditos`, `total_debitos`, `saldo`, `quantidade_lancamentos`.

## RF10 — Consultar Consolidado por Período
O sistema deve permitir consultar o consolidado para um range de datas.

## RF11 — Recalcular Consolidado
O sistema deve recalcular o consolidado quando um lançamento é cancelado (via evento LancamentoCanceladoEvent).
