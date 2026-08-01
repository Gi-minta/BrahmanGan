# syntax=docker/dockerfile:1
#
# Sirve el frontend como sitio estático detrás de nginx, igual que el Static Site de
# Render. Existe para que el entorno local reproduzca el despliegue: dos servicios
# separados, y el proxy de /api resuelto del lado servidor.
#
# Nota: en Render este Dockerfile NO se usa. Allí el sitio estático se construye con
# `npm ci && npm run build` y se publica `dist/`, y el equivalente de nginx.conf son
# las reglas de Redirects/Rewrites (ver render.yaml).

# ── Build ──────────────────────────────────────────────────────────────
FROM node:22-alpine AS build
WORKDIR /src

# Copia primero los manifiestos para aprovechar la caché de npm ci.
COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build

# ── Runtime ────────────────────────────────────────────────────────────
FROM nginx:alpine AS final
COPY --from=build /src/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
