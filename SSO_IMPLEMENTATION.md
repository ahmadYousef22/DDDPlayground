# SSO Implementation Guide

This solution implements Single Sign-On (SSO) across multiple API projects using JWT tokens.

## Architecture Overview

```
┌─────────────────┐
│   SSO Service   │  (Identity Provider - IdP)
│  (Port 7001)    │
│                 │
│  Issues JWT     │
│  Tokens         │
└────────┬────────┘
         │
         │ JWT Tokens
         │
    ┌────┴────┐
    │         │
    ▼         ▼
┌─────────┐ ┌─────────┐
│ API 1   │ │ API 2   │  (Service Providers - SP)
│ (Host)  │ │ (Api2)  │
│         │ │         │
│ Validates│ │ Validates│
│ Tokens  │ │ Tokens  │
└─────────┘ └─────────┘
```

## Components

### 1. SSO Service (DDDPlayGround.SSO)
- **Role**: Identity Provider (IdP)
- **Port**: 7001 (default)
- **Purpose**: Issues JWT tokens that can be used across multiple APIs
- **Endpoints**:
  - `POST /api/SSO/jwt/login` - Login and get JWT token
  - `POST /api/SSO/jwt/validate` - Validate a JWT token
  - `GET /api/SSO/options` - Get available SSO options
  - `GET /api/SSO/oauth/authorize` - OAuth authorization URL
  - `GET /api/SSO/oauth/callback` - OAuth callback handler
  - `POST /api/SSO/saml/assertion` - Generate SAML assertion

### 2. API 1 (DDDPlayGround.Host)
- **Role**: Service Provider (SP)
- **Port**: 5001 (default)
- **Purpose**: Validates tokens from SSO service
- **Endpoints**:
  - `POST /api/SSO/validate` - Validate SSO token
  - `GET /api/SSO/me` - Get current user info (requires authentication)
  - `GET /api/Utility/*` - Protected endpoints (require SSO token)
  - `POST /api/Authentication/*` - Authentication endpoints

### 3. API 2 (DDDPlayGround.Api2)
- **Role**: Service Provider (SP)
- **Port**: 5002 (default)
- **Purpose**: Validates tokens from SSO service
- **Endpoints**:
  - `POST /api/SSO/validate` - Validate SSO token
  - `GET /api/SSO/me` - Get current user info (requires authentication)
  - `GET /api/Data` - Protected data endpoint (requires SSO token)
  - `GET /api/Data/user-specific` - User-specific data (requires SSO token)

## Configuration

All projects share the same JWT configuration in `appsettings.json`:

```json
{
  "JwtSettings": {
    "Secret": "DJMIKMK585855552GVBHBNJKM^HNJBJUHUHJN4",
    "Issuer": "DDDPlayGround",
    "Audience": "DDDPlayGroundUsers",
    "ExpiryMinutes": 60
  }
}
```

**Important**: All three projects must use the same JWT settings for SSO to work correctly.

## How SSO Works

### Step 1: User Authenticates with SSO Service
```http
POST https://localhost:7001/api/SSO/jwt/login
Content-Type: application/json

{
  "username": "john.doe",
  "email": "john.doe@example.com",
  "roles": ["User", "Admin"]
}
```

**Response**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenType": "Bearer",
  "expiresIn": 3600,
  "message": "JWT token generated successfully"
}
```

### Step 2: Use Token in API 1
```http
GET https://localhost:5001/api/Utility/GetAdvice
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Step 3: Use Same Token in API 2
```http
GET https://localhost:5002/api/Data
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Result**: User accesses both APIs with the same token - Single Sign-On!

## Testing SSO

### 1. Start All Services
```bash
# Terminal 1 - SSO Service
cd DDDPlayGround.SSO
dotnet run

# Terminal 2 - API 1
cd DDDPlayGround.Host
dotnet run

# Terminal 3 - API 2
cd DDDPlayGround.Api2
dotnet run
```

### 2. Get Token from SSO Service
```bash
curl -X POST https://localhost:7001/api/SSO/jwt/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "roles": ["User"]
  }'
```

### 3. Use Token in API 1
```bash
curl -X GET https://localhost:5001/api/SSO/me \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 4. Use Same Token in API 2
```bash
curl -X GET https://localhost:5002/api/Data \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Token Validation

Both APIs validate tokens using the same JWT settings:
- **Issuer**: Must be "DDDPlayGround"
- **Audience**: Must be "DDDPlayGroundUsers"
- **Secret**: Must match the secret used to sign the token
- **Expiry**: Tokens expire after 60 minutes (configurable)

## Security Features

1. **Shared Secret**: All services use the same secret key for token signing/validation
2. **Token Expiry**: Tokens expire after configured time (default: 60 minutes)
3. **HTTPS**: All services use HTTPS in production
4. **Claims Validation**: Tokens contain user claims (name, email, roles)

## Adding More APIs

To add a third API that uses SSO:

1. Create new API project
2. Add Infrastructure project reference
3. Configure JWT authentication:
   ```csharp
   builder.Services.AddJwtAuthentication(builder.Configuration);
   ```
4. Use the same `JwtSettings` in `appsettings.json`
5. Add `[Authorize]` attribute to protected endpoints

## Troubleshooting

### Token Validation Fails
- Check that all projects use the same `JwtSettings:Secret`
- Verify `Issuer` and `Audience` match across all projects
- Ensure token hasn't expired

### CORS Issues
- Configure CORS in each API project
- Allow the SSO service origin if needed

### Port Conflicts
- Update `launchSettings.json` in each project
- Ensure ports don't conflict

## Next Steps

- [ ] Add token refresh mechanism
- [ ] Implement token revocation
- [ ] Add OAuth 2.0 provider integration (Google, Microsoft)
- [ ] Implement SAML 2.0 support
- [ ] Add token caching
- [ ] Implement distributed token validation

