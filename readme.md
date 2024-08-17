# Dockerized CRUD-Based .NET Web API
This is a sample implementation of a Dockerized CRUD-based .NET Web API, meticulously designed following best practices including:

- **Onion Architecture**
- **Domain-Driven Design (DDD)**
- **MediatR**
- **FluentValidation**
<br><br>
## Key Features
- **Dockerized**: The application runs in Docker containers, making it easy to set up and deploy.
- **Onion Architecture**: The project is structured to separate concerns and allow for high scalability and maintainability.
- **Domain-Driven Design (DDD)**: Business logic and domain rules are at the center of the implementation.
- **MediatR**: For clean separation of commands and queries in the application.
- **FluentValidation**: Ensures that validation logic is clean and reusable across the application.
<br><br>
## Testing and Quality Assurance
- **Behavior-Driven Development (BDD)**: The robustness of this API is ensured through testing conducted in accordance with Behavior-Driven Development (BDD) methodologies. The tests focus on ensuring that the application behaves as expected under various scenarios.
<br><br>
## Continuous Integration and Deployment
The project includes a fully configured GitHub Actions CI/CD pipeline. This pipeline performs the following tasks automatically on every push to the master and develop branches:

1. **Build**: The project is built using the latest version of .NET.
2. **Test**: All tests are executed to ensure that no changes break the application.
3. **Containerization**: The application is built and packaged as a Docker container.
4. **Deployment**: The Docker image is tagged and pushed to a Docker registry.
<br><br>
## Environment Configuration
The application uses a .env file for environment-specific configurations, such as MongoDB connection strings, database names, and other sensitive settings.
<br><br>
#### Sample .env File
```bash
MONGODB_CONNECTION_STRING=mongodb://localhost:27018
MONGODB_DATABASE_NAME=SampleTestDb
MONGODB_COLLECTION_NAME=CustomerEntity
API_BASE_ADDRESS=http://localhost:5111
```
<br><br>
#### Configuring GitHub Actions with Secrets
In the GitHub Actions workflow, sensitive environment variables such as connection strings are stored in GitHub Secrets. The workflow automatically reads these secrets and injects them into the application environment during the CI/CD process.

```bash
 env:
      MONGODB_CONNECTION_STRING: ${{ secrets.MONGODB_CONNECTION_STRING }}
      MONGODB_DATABASE_NAME: ${{ secrets.MONGODB_DATABASE_NAME }}
      MONGODB_COLLECTION_NAME: ${{ secrets.MONGODB_COLLECTION_NAME }}
      API_BASE_ADDRESS: ${{ secrets.API_BASE_ADDRESS }}
```
<br><br>
#### MongoDB Service Configuration in CI/CD
During the CI/CD process, a MongoDB service is started in a Docker container. The workflow ensures that MongoDB is fully up and running before the tests and other dependent tasks are executed:

```bash
- name: Wait for MongoDB to become available
  run: |
    for i in {1..10}; do
      if mongo --eval "db.runCommand({ connectionStatus: 1 })" > /dev/null 2>&1; then
        echo "MongoDB is up and running"
        break
      fi
      echo "Waiting for MongoDB to be ready..."
      sleep 5
    done
```
<br><br>
## Running the Application Locally
#### Prerequisites
- **Docker**: Ensure that Docker is installed on your system.
- **.NET SDK**: Install the latest .NET SDK to build and run the application.
#### Steps
1. Clone the repository.

2. Create a .env file in the root of the project (CSharpSampleCRUDTest.API) and configure your environment variables.

3. Build and run the application using Docker:

```bash
docker-compose up --build
```

4. Access the API via **'http://localhost:5003'**.

![](https://vistr.dev/badge?repo=mehrighafar.CSharpSampleCRUDTest) 