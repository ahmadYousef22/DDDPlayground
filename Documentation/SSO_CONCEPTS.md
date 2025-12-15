# SSO (Single Sign-On) Concepts

## What is SSO?

**Single Sign-On (SSO)** is an authentication method that allows users to log in once and gain access to multiple applications or services without needing to authenticate separately for each one.

### Simple Analogy
Think of SSO like a **master key** for a building:
- You authenticate once at the front desk (Identity Provider)
- You get access to all rooms (applications) in the building
- You don't need separate keys for each room

## Why Use SSO?

### Benefits

1. **Better User Experience**
   - Users log in once, access everything
   - No need to remember multiple passwords
   - Faster access to applications

2. **Improved Security**
   - Centralized password management
   - Easier to enforce security policies
   - Single point for password resets
   - Reduced password fatigue (users create stronger passwords)

3. **Reduced IT Costs**
   - Less password reset requests
   - Centralized user management
   - Easier to add/remove users

4. **Compliance**
   - Easier audit trails
   - Centralized access control
   - Better for regulatory requirements

### Real-World Examples

- **Google**: Log in once, access Gmail, YouTube, Drive, etc.
- **Microsoft 365**: Log in once, access Outlook, Teams, SharePoint, etc.
- **Enterprise**: Employees log in once, access all company applications

## How SSO Works

### Basic Flow

```
User → Application → Identity Provider (IdP) → User Authenticates → 
IdP Confirms → Application Grants Access
```

### Key Components

1. **Identity Provider (IdP)**
   - The system that authenticates users
   - Examples: Azure AD, Okta, Google, Active Directory

2. **Service Provider (SP) / Application**
   - The application the user wants to access
   - Examples: Your web app, SaaS application

3. **User**
   - The person trying to access the application

## SSO Protocols

There are two main protocols for SSO:

### 1. SAML 2.0 (Security Assertion Markup Language)

**What it is:**
- XML-based standard for enterprise SSO
- Older, well-established protocol
- Used primarily in enterprise/B2B scenarios

**Characteristics:**
- ✅ Very secure (signed XML assertions)
- ✅ Enterprise standard
- ✅ Works with existing enterprise systems
- ❌ XML-based (more complex)
- ❌ Limited mobile support
- ❌ Requires certificate management

**Best for:**
- Enterprise applications
- B2B integrations
- Organizations with existing SAML IdP

### 2. OAuth 2.0 / OpenID Connect (OIDC)

**What it is:**
- Modern, JSON-based protocol
- OAuth 2.0 = Authorization protocol
- OpenID Connect = Authentication layer on top of OAuth 2.0
- Used for modern web and mobile applications

**Characteristics:**
- ✅ Modern, widely adopted
- ✅ JSON-based (simpler than XML)
- ✅ Great mobile support
- ✅ Works well with APIs
- ✅ Multiple providers (Google, Microsoft, GitHub, etc.)
- ❌ More complex flow
- ❌ Requires HTTPS

**Best for:**
- Modern web applications
- Mobile applications
- Consumer applications
- APIs and microservices

## Comparison: SAML vs OAuth/OIDC

| Feature | SAML 2.0 | OAuth 2.0 / OIDC |
|---------|----------|-------------------|
| **Format** | XML | JSON |
| **Primary Use** | Enterprise SSO | Modern web/mobile |
| **Mobile Support** | Limited | Excellent |
| **API Support** | Limited | Excellent |
| **Complexity** | High | Medium |
| **Provider Support** | Enterprise IdPs | Google, Microsoft, GitHub, etc. |
| **Best For** | Enterprise/B2B | Consumer/Modern Apps |

## Authentication vs Authorization

### Authentication (AuthN)
**"Who are you?"**
- Verifie
- Example: Los user identitygin with username/password

### Authorization (AuthZ)
**"What can you do?"**
- Determines what user can access
- Example: User has "Admin" role, can access admin panel

**SSO handles Authentication** - it proves who you are. Authorization is usually handled separately by the application.

## Common SSO Scenarios

### Scenario 1: Employee Accessing Company Apps
```
Employee → Company Portal → Azure AD → Authenticates → 
Access to: Email, HR System, Payroll, etc.
```

### Scenario 2: Customer Using Multiple Services
```
Customer → Service A → Google OAuth → Authenticates → 
Access to: Service A, Service B, Service C (all using Google)
```

### Scenario 3: Partner Company Access
```
Partner Employee → Your App → Partner's SAML IdP → 
Authenticates → Access to Your App
```

## Security Considerations

### SSO Security Benefits
1. **Centralized Security**
   - One place to enforce password policies
   - Easier to detect suspicious activity
   - Single point for security updates

2. **Reduced Attack Surface**
   - Fewer passwords = fewer attack vectors
   - Centralized monitoring

### SSO Security Risks
1. **Single Point of Failure**
   - If IdP is compromised, all apps are at risk
   - Mitigation: Strong security at IdP, MFA

2. **Token Security**
   - Tokens must be protected
   - Proper expiration and validation needed

## Key Terms

- **IdP (Identity Provider)**: System that authenticates users
- **SP (Service Provider)**: Application that trusts the IdP
- **Assertion**: Proof of authentication (SAML)
- **Token**: Proof of authentication (OAuth/OIDC)
- **Federation**: Trust relationship between IdP and SP
- **SAML Assertion**: XML document proving authentication
- **JWT (JSON Web Token)**: JSON token used in OAuth/OIDC
- **Access Token**: Token to access resources (OAuth)
- **ID Token**: Token containing user identity (OIDC)

## Next Steps

1. Read `AUTHENTICATION_FLOWS.md` to see detailed flow diagrams
2. Understand the difference between SAML and OAuth flows
3. Learn about tokens and assertions
4. See implementation examples

## Summary

**SSO = Log in once, access many applications**

- **SAML 2.0**: Enterprise, XML-based, secure
- **OAuth 2.0 / OIDC**: Modern, JSON-based, flexible
- **Benefits**: Better UX, improved security, reduced costs
- **Use Case**: When users need to access multiple applications
