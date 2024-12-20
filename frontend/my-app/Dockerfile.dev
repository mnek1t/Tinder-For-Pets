# Fetching the latest node image on alpine linux
FROM node:alpine AS development

# Declaring environment variable
ENV NODE_ENV=development

# Setting up the work directory
WORKDIR /react-app

# Copy only package.json and package-lock.json first for efficient caching
COPY ./package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application files
COPY . .

# Start the application
CMD ["npm", "start"]
