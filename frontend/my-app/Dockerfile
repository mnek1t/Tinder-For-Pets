# Fetching the latest node image on alpine linux
FROM node:alpine AS development

# Declaring environment variable
ENV NODE_ENV=development

# Setting up the work directory
WORKDIR /react-app

# Copy only package.json and package-lock.json first for efficient caching
COPY package.json package.json
COPY package-lock.json package-lock.json

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . .

# Set environment variable to development
ENV NODE_ENV=development

# Expose the development server port
EXPOSE 3000

# Start the application
CMD ["npm", "start"]
