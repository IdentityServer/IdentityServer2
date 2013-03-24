This sample is intended to show using the ADFS integration in Identity Server. We have 5 players (beyond the end user):

1) ADFS
2) Identity Server
3) WebRP (MVC app)
4) WebAPI RP1 (WebAPI app)
5) WebAPI RP2 (WebAPI app)

The way this app works is that the WebRP is expecting a SAML token via WS-Fed from ADFS.

The WebRP app then wants to be able to logically delegate the end user's credentials when invoking WebAPI RP1. This will involve contacting Identity Server's ADFS integration endpoint. It will pass the original SAML token (held via WIF's bootstrap token) and the realm ID for WebAPI RP1. Internally, Identity Server will contact ADFS as a claims provider, send the realm requested and a SAML token signed by Identity Server with all the same claims in the original SAML token (the one from ADFS). ADFS will then issue a new SAML token for the realm requested. Identity Server will then convert the SAML token into a JWT and sign and return it to the WebRP. The WebRP will then set that JWT as a Bearer token in a HTTP request to WebAPI RP1.

Inside WebAPI RP1, it is configured to accept JWT tokens signed by Identity Server and does so via a route-specific message handler that only accepts tokens for its own audience (WebAPI RP1). Its only method simply returns the list of claims for the current user (essentially the claims contained in the JWT token it receives). WebAPI RP1 also accepts a boolean query string flag indicating if it should then invoke WebAPI RP2, delegating the user's credentials (via the ADFS integration). If this flag is set, then it will take the same steps the WebRP does above to contact Identity Server to obtain a JWT for WebAPI RP2, with the only difference being that it passes a JWT and not a SAML token.

WebAPI RP2 is configured similarly to WebAPI RP1 except that it's configured to only accept JWT tokens for its own audience (WebAPI RP2). Its implementation simply echos back the claims contained in the JWT (much like WebAPI RP1).

When opening the code in Visual Studio, given that we're assuming IIS and SSL, the solution needs to be run as an administrator.

There are several configuration items that are needed to make this sample work:
1) ADFS
  - claims provider trust needs to be configured for identity server
  - relying parties needs to be configured for all three of the RPs (WebRP, WebAPI RP1 and WebAPI RP2). These identifiers used are:
    - WebRP identifier : http://localhost/rp-adfs
    - WebRP redirect URL : https://localhost/rp-adfs
    - WebAPI RP1 : http://localhost/rp-adfs-webapi1
    - WebAPI RP2 : http://localhost/rp-adfs-webapi2
2) Identity Server
  - Adfs Integration protocol needs to be enabled and the various flags need to be configured for ADFS. Also, ensure that the "pass thru token" is not checked (otherwise Identity Server will not convert the SAML tokens from ADFS into JWTs and instead will just pass back the SAML tokens as-is).
3) WebRP
  - needs to be configured to trust ADFS as its identity provider (standard WS-Fed config in web.config)
  - must also be configured to save the bootstrap token (via <identityConfiguration saveBootstrapContext="true"> in web.config)
  - URIs for the Identity Server and WebWPI RP endpoints need to be configured in HomeController.cs.
4) WebAPI RPs
  - Identity Server identifier and signing certificate need to be configured in App_Start/WebApiConfig.cs. The identity server signing certificate needs to be deployed to the WebAPI RP machine and loaded somehow (the sample is looking for it in the Machine Certificate Store under Trusted People).

If there are any questions or issues, please submit them to: 
https://github.com/thinktecture/Thinktecture.IdentityServer.v2/issues
