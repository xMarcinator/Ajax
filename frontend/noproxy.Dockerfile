FROM node:21.7.1-alpine3.19 AS build

RUN corepack enable
WORKDIR /app
COPY frontend/package.json frontend/pnpm-lock.yaml ./
RUN --mount=type=cache,id=pnpm,target=/root/.local/share/pnpm/store pnpm fetch --frozen-lockfile
RUN --mount=type=cache,id=pnpm,target=/root/.local/share/pnpm/store pnpm install --frozen-lockfile --prod
COPY ./frontend .
RUN pnpm run build


FROM nginx
# Copy the default nginx.conf provided by tiangolo/node-frontend
COPY frontend/noproxy.nginx.conf /etc/nginx/conf.d/default.conf

#Copy the static file from the build stage to the nginx server
COPY --from=build /app/dist   /var/www/html
 