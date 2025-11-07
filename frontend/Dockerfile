# Stage 1: Build the Angular application
FROM node:20 AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build -- --configuration production

# Stage 2: Serve the application from a lightweight web server
FROM nginx

# Copy the built application from the build stage
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html

# Copy the nginx configuration file
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80