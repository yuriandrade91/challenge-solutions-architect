import http from 'k6/http';
import { check, sleep } from 'k6';
import { Rate, Counter } from 'k6/metrics';
import { uuidv4 } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';

const errorRate = new Rate('errors');
const createdLancamentos = new Counter('created_lancamentos');

export const options = {
  stages: [
    { duration: '30s', target: 20 },
    { duration: '2m', target: 20 },
    { duration: '30s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(95)<200'],
    errors: ['rate<0.05'],
  },
};

const BASE_URL = __ENV.BASE_URL || 'http://localhost:8080';

export default function () {
  const payload = JSON.stringify({
    idempotencyKey: uuidv4(),
    tipo: Math.random() > 0.5 ? 'CREDIT' : 'DEBIT',
    valor: Math.round(Math.random() * 1000 * 100) / 100 + 1,
    descricao: `Load test lancamento ${Date.now()}`,
    categoria: 'LOAD_TEST',
  });

  const headers = { 'Content-Type': 'application/json' };

  const createRes = http.post(`${BASE_URL}/lancamentos/api/v1/lancamentos`, payload, { headers });
  const createSuccess = check(createRes, {
    'create status is 201': (r) => r.status === 201,
    'create response time < 200ms': (r) => r.timings.duration < 200,
  });
  errorRate.add(!createSuccess);
  if (createSuccess) createdLancamentos.add(1);

  const listRes = http.get(`${BASE_URL}/lancamentos/api/v1/lancamentos?page=1&pageSize=10`);
  const listSuccess = check(listRes, {
    'list status is 200': (r) => r.status === 200,
    'list response time < 200ms': (r) => r.timings.duration < 200,
  });
  errorRate.add(!listSuccess);

  sleep(1);
}
