# Learning POC - SSO & RabbitMQ

Simple POC for learning and practicing:
1. **SSO (Single Sign-On)** - Authentication Options
2. **RabbitMQ** - Message Queue patterns

## Features

### SSO (Authentication Options)

The POC demonstrates different authentication methods:

1. **JWT (JSON Web Token)**
   - Generate JWT tokens
   - Validate JWT tokens
   - Token-based authentication

2. **OAuth 2.0**
   - Google OAuth
   - Microsoft OAuth
   - GitHub OAuth
   - Authorization URL generation

3. **SAML 2.0**
   - SAML assertion generation (simulated)

### RabbitMQ (Message Queue)

The POC demonstrates message queue operations:

1. **Queue Management**
   - Create queues
   - Delete queues
   - Get queue statistics
   - List all queues

2. **Message Operations**
   - Send messages (Producer)
   - Receive messages (Consumer)
   - Message routing

3. **Exchange Patterns**
   - Direct exchange
   - Topic exchange
   - Fanout exchange

## API Endpoints

### SSO Endpoints

- `GET /api/sso/options` - Get available SSO options
- `POST /api/sso/jwt/login` - Generate JWT token
- `POST /api/sso/jwt/validate` - Validate JWT token
- `GET /api/sso/oauth/authorize` - Get OAuth authorization URL
- `POST /api/sso/saml/assertion` - Generate SAML assertion

### RabbitMQ Endpoints

- `POST /api/rabbitmq/queue/create` - Create a queue
- `POST /api/rabbitmq/send` - Send message to queue
- `GET /api/rabbitmq/receive` - Receive message from queue
- `GET /api/rabbitmq/queue/stats` - Get queue statistics
- `GET /api/rabbitmq/queues` - Get all queues
- `DELETE /api/rabbitmq/queue` - Delete queue
- `POST /api/rabbitmq/exchange/send` - Send message to exchange

## Usage Examples

### Generate JWT Token

```bash
POST /api/sso/jwt/login
Content-Type: application/json

{
  "username": "john.doe",
  "email": "john.doe@example.com",
  "roles": ["User", "Admin"]
}
```

### Send Message to Queue

```bash
POST /api/rabbitmq/send
Content-Type: application/json

{
  "queueName": "email-queue",
  "message": {
    "to": "user@example.com",
    "subject": "Welcome",
    "body": "Welcome to our service!"
  },
  "routingKey": "email.notification"
}
```

### Receive Message from Queue

```bash
GET /api/rabbitmq/receive?queueName=email-queue&autoAck=true
```

## Configuration

Update `appsettings.json` with your OAuth credentials:

```json
{
  "Jwt": {
    "Key": "YourSecretKeyForJWTTokenGeneration123456789",
    "Issuer": "LearningPOC",
    "Audience": "LearningPOC"
  },
  "OAuth": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    }
  }
}
```

## Running the Application

1. Restore packages:
   ```bash
   dotnet restore
   ```

2. Run the application:
   ```bash
   dotnet run
   ```

3. Access Swagger UI:
   - Navigate to `https://localhost:5001/swagger` (or the port shown in console)

## Notes

- This is a **POC for learning purposes only**
- No database persistence - all data is in-memory
- RabbitMQ operations are simulated (not using actual RabbitMQ server)
- SSO operations demonstrate concepts but may need actual OAuth providers for full functionality

## Learning Resources

See the `Documentation` folder for detailed concepts:
- `SSO_CONCEPTS.md` - SSO fundamentals
- `AUTHENTICATION_FLOWS.md` - Authentication flow diagrams
- `RABBITMQ_CONCEPTS.md` - RabbitMQ fundamentals
