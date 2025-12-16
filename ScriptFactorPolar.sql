USE [FactorPolar]
GO

/****** Object:  Table [dbo].[Beneficiarios]    Script Date: 15/12/2025 10:06:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Beneficiarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[Denominacion] [nvarchar](50) NULL,
	[CedulaIdentidad] [nvarchar](50) NULL,
	[FullName] [nvarchar](200) NULL,
	[Gender] [nvarchar](50) NULL,
	[FechaNacimiento] [datetime] NULL,
	[Edad] [int] NULL,
	[TipoInstitucion] [nvarchar](10) NULL,
	[NombreInstitucion] [nvarchar](500) NULL,
	[RifInstitucion] [nvarchar](20) NULL,
	[NivelEducativo] [nvarchar](10) NULL,
	[GradoEducativo] [nvarchar](10) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[Documento1] [nvarchar](10) NULL,
	[Documento2] [nvarchar](10) NULL,
	[Documento3] [nvarchar](10) NULL,
	[RutaNotas] [nvarchar](200) NULL,
	[RutaVideo] [nvarchar](200) NULL,
	[Promedio] [int] NULL,
 CONSTRAINT [PK_Beneficiarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Credentials]    Script Date: 15/12/2025 10:06:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Credentials](
	[UserId] [uniqueidentifier] NOT NULL,
	[AccessToken] [nvarchar](1000) NULL,
	[RefreshToken] [nvarchar](1000) NULL,
	[ExpiresInSeconds] [bigint] NULL,
	[IdToken] [varchar](max) NULL,
	[IssuedUtc] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Employees]    Script Date: 15/12/2025 10:06:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[EmployeeNumber] [bigint] NULL,
	[FullName] [nvarchar](200) NULL,
	[Gender] [nvarchar](50) NULL,
	[Status] [nvarchar](50) NULL,
	[CodigoTipo] [nvarchar](10) NULL,
	[DescipcionTipo] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Usuarios]    Script Date: 15/12/2025 10:06:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](150) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Beneficiarios]  WITH CHECK ADD  CONSTRAINT [FK_Beneficiarios_Employees] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([Id])
GO

ALTER TABLE [dbo].[Beneficiarios] CHECK CONSTRAINT [FK_Beneficiarios_Employees]
GO


