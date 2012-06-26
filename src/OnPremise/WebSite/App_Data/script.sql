CREATE TABLE [dbo].[Certificates](
	[Name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SubjectDistinguishedName] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
ALTER TABLE [dbo].[Certificates] ADD  CONSTRAINT [PK_Certificates] PRIMARY KEY 
(
	[Name]
)
GO

CREATE TABLE [dbo].[ClientCertificates](
	[Id] [int] IDENTITY(31,1) NOT NULL,
	[UserName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Thumbprint] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[ClientCertificates] ADD  CONSTRAINT [PK_ClientCertificates] PRIMARY KEY 
(
	[Id]
)
GO

CREATE TABLE [dbo].[Delegation](
	[Id] [int] IDENTITY(11,1) NOT NULL,
	[UserName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Realm] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)

GO
ALTER TABLE [dbo].[Delegation] ADD  CONSTRAINT [PK_Delegation] PRIMARY KEY 
(
	[Id]
)
GO

CREATE TABLE [dbo].[Endpoints](
	[Name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WSFederation] [bit] NOT NULL,
	[WSTrustMessage] [bit] NOT NULL,
	[WSTrustMixed] [bit] NOT NULL,
	[FederationMetadata] [bit] NOT NULL,
	[HttpPort] [int] NOT NULL,
	[HttpsPort] [int] NOT NULL,
	[SimpleHttp] [bit] NOT NULL,
	[OAuthWRAP] [bit] NOT NULL,
	[OAuth2] [bit] NOT NULL,
	[JSNotify] [bit] NOT NULL
)

GO
ALTER TABLE [dbo].[Endpoints] ADD  CONSTRAINT [PK__Endpoints__000000000000007C] PRIMARY KEY 
(
	[Name]
)
GO
INSERT INTO [dbo].[Endpoints]([Name],[WSFederation],[WSTrustMessage],[WSTrustMixed],[FederationMetadata],[HttpPort],[HttpsPort],[SimpleHttp],[OAuthWRAP],[OAuth2],[JSNotify]) VALUES (N'Standard',1,0,0,1,80,443,0,0,0,0)
GO

CREATE TABLE [dbo].[Global](
	[Name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SiteName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IssuerUri] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[IssuerContactEmail] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DefaultTokenType] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RequireSsl] [bit] NOT NULL,
	[RequireEncryption] [bit] NOT NULL,
	[RequireReplyToWithinRealm] [bit] NOT NULL,
	[RequireSignInConfirmation] [bit] NOT NULL,
	[AllowKnownRealmsOnly] [bit] NOT NULL,
	[AllowReplyTo] [bit] NOT NULL,
	[DefaultTokenLifetime] [int] NOT NULL,
	[MaximumTokenLifetime] [int] NOT NULL,
	[EnableClientCertificates] [bit] NOT NULL,
	[EnableDelegation] [bit] NOT NULL,
	[SsoCookieLifetime] [int] NOT NULL,
	[EnableFederationMessageTracing] [bit] NOT NULL,
	[EnableStrongEpiForSsl] [bit] NOT NULL,
	[EnforceUsersGroupMembership] [bit] NOT NULL
)

GO
ALTER TABLE [dbo].[Global] ADD  CONSTRAINT [PK__Global__000000000000008A] PRIMARY KEY 
(
	[Name]
)
GO
INSERT INTO [dbo].[Global]([Name],[SiteName],[IssuerUri],[IssuerContactEmail],[DefaultTokenType],[RequireSsl],[RequireEncryption],[RequireReplyToWithinRealm],[RequireSignInConfirmation],[AllowKnownRealmsOnly],[AllowReplyTo],[DefaultTokenLifetime],[MaximumTokenLifetime],[EnableClientCertificates],[EnableDelegation],[SsoCookieLifetime],[EnableFederationMessageTracing],[EnableStrongEpiForSsl],[EnforceUsersGroupMembership]) VALUES (N'Standard',N'thinktecture identity server 1.0',N'http://identityserver.thinktecture.com/trust/initial',N'identityserver@thinktecture.com',N'urn:oasis:names:tc:SAML:2.0:assertion',1,0,1,0,1,0,10,24,0,0,10,0,0,1)
GO
CREATE TABLE [dbo].[RelyingParties](
	[Name] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Realm] [nvarchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ReplyTo] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[EncryptingCertificate] [nvarchar](2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SymmetricSigningKey] [nvarchar](2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Id] [int] IDENTITY(18,1) NOT NULL,
	[ExtraData1] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ExtraData2] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ExtraData3] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)

GO
ALTER TABLE [dbo].[RelyingParties] ADD  CONSTRAINT [PK__RelyingParties__0000000000000115] PRIMARY KEY 
(
	[Id]
)
GO

ALTER TABLE [dbo].[Certificates] ADD  CONSTRAINT [UQ__Certificates__000000000000009C] UNIQUE 
(
	[Name]
)
GO
ALTER TABLE [dbo].[ClientCertificates] ADD  CONSTRAINT [UQ__ClientCertificates__00000000000000EC] UNIQUE 
(
	[Thumbprint]
)
GO
ALTER TABLE [dbo].[RelyingParties] ADD  CONSTRAINT [UQ__RelyingParties__00000000000000B2] UNIQUE 
(
	[Realm]
)
GO
ALTER TABLE [dbo].[RelyingParties] ADD  CONSTRAINT [UQ__RelyingParties__000000000000011A] UNIQUE 
(
	[Name]
)
GO
