FROM node:20-slim AS build
    WORKDIR /app
    COPY package.json pnpm-lock.yaml ./
    RUN npm install -g pnpm && pnpm install

    COPY . .
    RUN pnpm run build

FROM nginx:latest
    COPY --from=build /app/build /usr/share/nginx/html
    COPY nginx.conf /etc/nginx/conf.d/default.conf

    EXPOSE 80
    CMD sh -c "envsubst '\$CONFIG_PIZZA_API_URL' < /usr/share/nginx/html/config.example.js > /usr/share/nginx/html/config.js && cat /usr/share/nginx/html/config.js && nginx -g 'daemon off;'"
