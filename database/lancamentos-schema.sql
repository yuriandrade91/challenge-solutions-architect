CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE TYPE lancamento_tipo AS ENUM ('CREDIT', 'DEBIT');
CREATE TYPE lancamento_status AS ENUM ('ACTIVE', 'CANCELLED');

CREATE TABLE lancamentos (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    idempotency_key UUID NOT NULL UNIQUE,
    tipo lancamento_tipo NOT NULL,
    valor DECIMAL(15,2) NOT NULL CHECK (valor > 0),
    descricao VARCHAR(500) NOT NULL,
    categoria VARCHAR(100),
    data_lancamento DATE NOT NULL DEFAULT CURRENT_DATE,
    status lancamento_status NOT NULL DEFAULT 'ACTIVE',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    deleted_at TIMESTAMP WITH TIME ZONE NULL
);

CREATE INDEX idx_lancamentos_data ON lancamentos(data_lancamento);
CREATE INDEX idx_lancamentos_tipo ON lancamentos(tipo);
CREATE INDEX idx_lancamentos_status ON lancamentos(status) WHERE status = 'ACTIVE';
CREATE INDEX idx_lancamentos_data_status ON lancamentos(data_lancamento, status);
