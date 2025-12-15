# Authentication Flows - Detailed Explanations

This document explains the step-by-step flows for different authentication methods.

## Table of Contents

1. [JWT Token Flow](#jwt-token-flow)
2. [OAuth 2.0 Authorization Code Flow](#oauth-20-authorization-code-flow)
3. [SAML 2.0 SSO Flow](#saml-20-sso-flow)
4. [Comparison of Flows](#comparison-of-flows)

---

## JWT Token Flow

### What is JWT?

**JWT (JSON Web Token)** is a compact, URL-safe token format used for authentication and authorization.

### JWT Structure

A JWT has three parts separated by dots (`.`):

```
Header.Payload.Signature
```

**Example:**
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

1. **Header**: Algorithm and token type
2. **Payload**: Claims (user info, roles, expiration)
3. **Signature**: Ensures token hasn't been tampered with

### JWT Authentication Flow

```mermaid
sequenceDiagram
    participant User
    participant Client as Client App
    participant Server as Auth Server
    participant API as Protected API

    User->>Client: Enters credentials
    Client->>Server: POST /login (username, password)
    Server->>Server: Validate credentials
    Server->>Server: Generate JWT token
    Server->>Client: Return JWT token
    Client->>Client: Store token (localStorage/cookie)
    
    Note over Client,API: User makes API request
    Client->>API: GET /api/data<br/>Header: Authorization: Bearer {JWT}
    API->>API: Validate JWT signature
    API->>API: Check expiration
    API->>API: Extract user claims
    API->>Client: Return data
```

### Step-by-Step Explanation

1. **User Login**
   - User provides username and password
   - Client sends credentials to authentication server

2. **Server Validation**
   - Server validates credentials against database
   - If valid, creates JWT token with user information

3. **Token Generation**
   - Server creates JWT with:
     - User ID
     - Roles/Permissions
     - Expiration time
     - Other claims
   - Signs token with secret key

4. **Token Return**
   - Server returns JWT to client
   - Client stores token (usually in localStorage or cookie)

5. **Using Token**
   - Client includes token in `Authorization` header:
     ```
     Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
     ```

6. **Token Validation**
   - API server validates:
     - Signature (not tampered)
     - Expiration (not expired)
     - Issuer (from trusted source)
   - Extracts user information from token

### JWT Advantages

- ✅ **Stateless**: Server doesn't need to store session
- ✅ **Scalable**: Works across multiple servers
- ✅ **Self-contained**: Token has all user info
- ✅ **Mobile-friendly**: Works well with mobile apps

### JWT Disadvantages

- ❌ **Token Revocation**: Hard to revoke before expiration
- ❌ **Token Size**: Can be large if many claims
- ❌ **Security**: If token is stolen, attacker has access until expiration

---

## OAuth 2.0 Authorization Code Flow

### What is OAuth 2.0?

**OAuth 2.0** is an authorization framework that allows applications to obtain limited access to user accounts.

**Note**: OAuth 2.0 is for **authorization** (what can you do), but when combined with **OpenID Connect (OIDC)**, it provides **authentication** (who are you) - which is what we need for SSO.

### OAuth 2.0 Flow Diagram

```mermaid
sequenceDiagram
    participant User
    participant App as Your App (SP)
    participant Browser
    participant Provider as OAuth Provider<br/>(Google/Microsoft)
    participant API as Provider API

    User->>App: Clicks "Login with Google"
    App->>Browser: Redirect to Provider<br/>GET /oauth/authorize?client_id=...&redirect_uri=...
    Browser->>Provider: User sees login page
    User->>Provider: Enters credentials
    Provider->>Provider: Validates credentials
    Provider->>Browser: Redirect back to App<br/>?code=AUTHORIZATION_CODE
    Browser->>App: Callback with code
    App->>Provider: Exchange code for tokens<br/>POST /oauth/token<br/>(code + client_secret)
    Provider->>App: Return tokens<br/>{access_token, id_token, refresh_token}
    App->>API: Use access_token<br/>GET /userinfo
    API->>App: Return user info
    App->>User: User logged in
```

### Step-by-Step Explanation

#### Step 1: User Initiates Login
```
User clicks "Login with Google" button in your app
```

#### Step 2: Redirect to Provider
```
Your app redirects user to:
https://accounts.google.com/oauth/authorize?
  client_id=YOUR_CLIENT_ID
  &redirect_uri=https://yourapp.com/callback
  &response_type=code
  &scope=openid email profile
```

**Parameters:**
- `client_id`: Your app's ID (registered with provider)
- `redirect_uri`: Where to send user after authentication
- `response_type=code`: Request authorization code
- `scope`: What information you need (openid = authentication)

#### Step 3: User Authenticates
```
User sees Google login page
User enters Google credentials
Google validates credentials
```

#### Step 4: Authorization Code Returned
```
Google redirects back to your app:
https://yourapp.com/callback?code=AUTHORIZATION_CODE&state=...
```

**Important**: This code is temporary (expires in minutes) and can only be used once.

#### Step 5: Exchange Code for Tokens
```
Your app (server-side) exchanges code for tokens:
POST https://accounts.google.com/oauth/token
  client_id=YOUR_CLIENT_ID
  client_secret=YOUR_CLIENT_SECRET
  code=AUTHORIZATION_CODE
  grant_type=authorization_code
  redirect_uri=https://yourapp.com/callback
```

**Why server-side?** Client secret must be kept secret - never expose in browser.

#### Step 6: Receive Tokens
```
Google returns:
{
  "access_token": "ya29.a0AfH6...",
  "id_token": "eyJhbGciOiJSUzI1NiIs...",
  "refresh_token": "1//04...",
  "expires_in": 3600
}
```

**Token Types:**
- **Access Token**: Used to access user's resources (e.g., get email)
- **ID Token**: Contains user identity (JWT format)
- **Refresh Token**: Used to get new access tokens when expired

#### Step 7: Get User Information
```
Your app uses access_token to get user info:
GET https://www.googleapis.com/oauth2/v2/userinfo
  Authorization: Bearer {access_token}

Response:
{
  "id": "123456789",
  "email": "user@gmail.com",
  "name": "John Doe",
  "picture": "https://..."
}
```

#### Step 8: Create Session
```
Your app creates session for user
User is now logged in
```

### OAuth 2.0 Key Concepts

**Client ID**: Public identifier for your app (safe to expose)
**Client Secret**: Secret key for your app (must be kept secret)
**Authorization Code**: Temporary code exchanged for tokens
**Access Token**: Token to access user resources
**ID Token**: Token containing user identity (OIDC)
**Redirect URI**: Where provider sends user after authentication

### OAuth 2.0 Advantages

- ✅ **No Password Storage**: Provider handles passwords
- ✅ **User Trust**: Users trust Google/Microsoft more
- ✅ **Standard Protocol**: Works with many providers
- ✅ **Granular Permissions**: Request only what you need

### OAuth 2.0 Disadvantages

- ❌ **Complex Flow**: More steps than simple login
- ❌ **Provider Dependency**: Depends on third-party
- ❌ **Configuration**: Requires setup with each provider

---

## SAML 2.0 SSO Flow

### What is SAML 2.0?

**SAML 2.0 (Security Assertion Markup Language)** is an XML-based standard for exchanging authentication and authorization data between parties.

### SAML 2.0 Flow Diagram

```mermaid
sequenceDiagram
    participant User
    participant Browser
    participant SP as Service Provider<br/>(Your App)
    participant IdP as Identity Provider<br/>(Company IdP)

    User->>Browser: Accesses https://yourapp.com
    Browser->>SP: GET /protected-resource
    SP->>SP: User not authenticated
    SP->>Browser: Redirect to IdP<br/>SAML AuthnRequest
    Browser->>IdP: User sees login page
    User->>IdP: Enters credentials
    IdP->>IdP: Validates credentials
    IdP->>IdP: Creates SAML Assertion
    IdP->>Browser: POST SAML Response<br/>(signed XML)
    Browser->>SP: POST /saml/acs<br/>(SAML Response)
    SP->>SP: Validate SAML signature
    SP->>SP: Extract user attributes
    SP->>Browser: Create session
    Browser->>User: User logged in
```

### Step-by-Step Explanation

#### Step 1: User Accesses Application
```
User navigates to: https://yourapp.com/dashboard
```

#### Step 2: Application Detects No Authentication
```
Your app (Service Provider) detects user is not logged in
```

#### Step 3: Create SAML AuthnRequest
```
Your app creates SAML AuthnRequest (XML):
<samlp:AuthnRequest
  ID="abc123"
  IssueInstant="2024-01-01T10:00:00Z"
  Destination="https://idp.company.com/sso"
  AssertionConsumerServiceURL="https://yourapp.com/saml/acs">
  <saml:Issuer>https://yourapp.com</saml:Issuer>
</samlp:AuthnRequest>
```

#### Step 4: Redirect to Identity Provider
```
Your app redirects user to IdP with SAML request:
https://idp.company.com/sso?
  SAMLRequest={base64_encoded_xml}
  RelayState={optional_state}
```

#### Step 5: User Authenticates at IdP
```
User sees company login page
User enters company credentials
IdP validates credentials
```

#### Step 6: IdP Creates SAML Assertion
```
IdP creates SAML Assertion (XML) containing:
- User identity (email, name)
- Authentication method
- Timestamp
- Digital signature
```

**SAML Assertion Example:**
```xml
<saml:Assertion>
  <saml:Subject>
    <saml:NameID>user@company.com</saml:NameID>
  </saml:Subject>
  <saml:AttributeStatement>
    <saml:Attribute Name="email">
      <saml:AttributeValue>user@company.com</saml:AttributeValue>
    </saml:Attribute>
    <saml:Attribute Name="name">
      <saml:AttributeValue>John Doe</saml:AttributeValue>
    </saml:Attribute>
  </saml:AttributeStatement>
</saml:Assertion>
```

#### Step 7: IdP Sends SAML Response
```
IdP creates SAML Response (wraps assertion):
<samlp:Response>
  <saml:Assertion>...</saml:Assertion>
  <ds:Signature>...</ds:Signature>
</samlp:Response>
```

IdP **signs** the response with its certificate (proves it's from IdP).

#### Step 8: POST Back to Application
```
IdP POSTs SAML Response to your app:
POST https://yourapp.com/saml/acs
  SAMLResponse={base64_encoded_signed_xml}
  RelayState={optional_state}
```

**Why POST?** SAML responses can be large (XML), POST handles large data better.

#### Step 9: Application Validates SAML Response
```
Your app validates:
1. XML signature (using IdP's public certificate)
2. Assertion hasn't expired
3. Assertion is for your app (Audience)
4. Response is from trusted IdP
```

#### Step 10: Extract User Information
```
Your app extracts from SAML Assertion:
- User email
- User name
- Roles/Groups
- Other attributes
```

#### Step 11: Create Session
```
Your app creates session for user
User is now logged in
```

### SAML 2.0 Key Concepts

**IdP (Identity Provider)**: System that authenticates users (e.g., Azure AD, Okta)
**SP (Service Provider)**: Your application
**SAML Assertion**: XML document proving user authentication
**AuthnRequest**: Request from SP to IdP to authenticate user
**SAML Response**: Response from IdP containing assertion
**ACS (Assertion Consumer Service)**: Endpoint in SP that receives SAML response
**Digital Signature**: Cryptographic proof that assertion is from IdP

### SAML 2.0 Advantages

- ✅ **Enterprise Standard**: Widely used in enterprises
- ✅ **Very Secure**: Signed XML assertions
- ✅ **Attribute Exchange**: Can pass user attributes
- ✅ **Established**: Mature, well-tested protocol

### SAML 2.0 Disadvantages

- ❌ **XML Complexity**: More complex than JSON
- ❌ **Limited Mobile**: Not ideal for mobile apps
- ❌ **Certificate Management**: Requires certificate setup
- ❌ **Verbose**: XML is larger than JSON

---

## Comparison of Flows

| Aspect | JWT | OAuth 2.0 / OIDC | SAML 2.0 |
|--------|-----|------------------|----------|
| **Format** | JSON | JSON | XML |
| **Flow Type** | Direct | Redirect | Redirect |
| **Token Type** | JWT | Access Token + ID Token | SAML Assertion |
| **Complexity** | Simple | Medium | Complex |
| **Mobile Support** | Excellent | Excellent | Limited |
| **API Support** | Excellent | Excellent | Limited |
| **Best For** | APIs, SPAs | Modern web/mobile | Enterprise |
| **User Experience** | Fast | Good | Good |
| **Setup Complexity** | Low | Medium | High |

## Key Takeaways

1. **JWT**: Simple, stateless tokens for APIs and modern apps
2. **OAuth 2.0/OIDC**: Modern SSO for web and mobile, uses redirect flow
3. **SAML 2.0**: Enterprise SSO, XML-based, very secure
4. **All provide SSO**: User authenticates once, accesses multiple apps
5. **Choose based on**: Your use case, infrastructure, and requirements

## Next Steps

1. Understand which flow fits your scenario
2. See implementation examples in the code
3. Test each flow to understand the experience
