CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE consolidado_diario (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    data DATE NOT NULL UNIQUE,
    total_creditos DECIMAL(15,2) NOT NULL DEFAULT 0,
    total_debitos DECIMAL(15,2) NOT NULL DEFAULT 0,
    quantidade_lancamentos INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

CREATE INDEX idx_consolidado_data ON consolidado_diario(data);
CREATE INDEX idx_consolidado_data_range ON consolidado_diario(data DESC);
