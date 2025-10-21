# Etapa de build
FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build

# Etapa de produção - Nginx
FROM nginx:stable-alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/dist .
EXPOSE 5173

# Redirecionar requisições de SPA para index.html
COPY <<EOF /etc/nginx/conf.d/default.conf
server {
    listen 5173;
    server_name localhost;

    location / {
        root /usr/share/nginx/html;
        index index.html;
        try_files \$uri /index.html;
    }
}
EOF
