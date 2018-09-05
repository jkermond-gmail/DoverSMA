USE [master]
GO
/****** Object:  Database [DoverSma]    Script Date: 9/5/2018 12:41:59 PM ******/
CREATE DATABASE [DoverSma]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DoverSma', FILENAME = N'C:\A_Development\SQL\DoverSma\Db\DoverSma.mdf' , SIZE = 18432KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DoverSma_log', FILENAME = N'C:\A_Development\SQL\DoverSma\Db\DoverSma_log.ldf' , SIZE = 39296KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DoverSma] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DoverSma].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DoverSma] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DoverSma] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DoverSma] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DoverSma] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DoverSma] SET ARITHABORT OFF 
GO
ALTER DATABASE [DoverSma] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DoverSma] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DoverSma] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DoverSma] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DoverSma] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DoverSma] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DoverSma] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DoverSma] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DoverSma] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DoverSma] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DoverSma] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DoverSma] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DoverSma] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DoverSma] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DoverSma] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DoverSma] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DoverSma] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DoverSma] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DoverSma] SET  MULTI_USER 
GO
ALTER DATABASE [DoverSma] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DoverSma] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DoverSma] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DoverSma] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DoverSma] SET DELAYED_DURABILITY = DISABLED 
GO
USE [DoverSma]
GO
/****** Object:  Table [dbo].[AssetManagers]    Script Date: 9/5/2018 12:41:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssetManagers](
	[AssetManagerCode] [varchar](10) NOT NULL,
	[AssetManager] [varchar](50) NOT NULL CONSTRAINT [DF_AssetManagers_AssetManager]  DEFAULT ('')
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SmaFlows]    Script Date: 9/5/2018 12:41:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SmaFlows](
	[SmaFlowId] [int] IDENTITY(1000,1) NOT NULL,
	[SmaOfferingId] [int] NOT NULL,
	[AssetManagerCode] [varchar](10) NOT NULL CONSTRAINT [DF_SmaFlows_AssetManagerCode_1]  DEFAULT (''),
	[FlowDate] [date] NOT NULL,
	[Assets] [varchar](50) NOT NULL CONSTRAINT [DF_SmaFlows_Assets]  DEFAULT (''),
	[GrossFlows] [varchar](50) NOT NULL CONSTRAINT [DF_SmaFlows_GrossFlows]  DEFAULT (''),
	[Redemptions] [varchar](50) NOT NULL CONSTRAINT [DF_SmaFlows_Redemptions]  DEFAULT (''),
	[NetFlows] [varchar](50) NOT NULL CONSTRAINT [DF_SmaFlows_NetFlows]  DEFAULT (''),
	[DerivedFlows] [varchar](50) NOT NULL CONSTRAINT [DF_SmaFlows_DerivedFlows]  DEFAULT (''),
	[AssetsD] [decimal](18, 8) NULL,
	[GrossFlowsD] [decimal](18, 8) NULL,
	[RedemptionsD] [decimal](18, 8) NULL,
	[NetFlowsD] [decimal](18, 8) NULL,
	[DerivedFlowsD] [decimal](18, 8) NULL,
	[DoverDerivedFlowsD] [decimal](18, 8) NULL,
	[FinalNetFlowsD] [decimal](18, 8) NULL,
	[PerformanceImpactD] [decimal](18, 8) NULL,
 CONSTRAINT [PK_SmaFlows] PRIMARY KEY CLUSTERED 
(
	[SmaOfferingId] ASC,
	[AssetManagerCode] ASC,
	[FlowDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SmaOfferings]    Script Date: 9/5/2018 12:41:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SmaOfferings](
	[SmaOfferingId] [int] IDENTITY(10001,1) NOT NULL,
	[AssetManagerCode] [varchar](10) NOT NULL CONSTRAINT [DF_SmaOfferings_AssetManagerCode]  DEFAULT (''),
	[SponsorFirm] [varchar](80) NOT NULL,
	[AdvisoryPlatform] [varchar](80) NOT NULL CONSTRAINT [DF_SmaOfferings_AdvisoryPlatform]  DEFAULT (''),
	[AdvisoryPlatformCode] [varchar](80) NOT NULL CONSTRAINT [DF_SmaOfferings_AdvisoryPlatformCode]  DEFAULT (''),
	[SmaStrategy] [varchar](80) NOT NULL,
	[SmaProductType] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_SmaProductType]  DEFAULT (''),
	[SmaProductTypeCode] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_SmaProductTypeCode]  DEFAULT (''),
	[TampRIAPlatform] [varchar](80) NOT NULL CONSTRAINT [DF_SmaOfferings_TampRIAPlatform]  DEFAULT (''),
	[ManagerClass] [varchar](125) NOT NULL CONSTRAINT [DF_SmaOfferings_ManagerClass]  DEFAULT (''),
	[SmaOfferingKeyId] [int] NOT NULL CONSTRAINT [DF_SmaOfferings_SmaOfferingKeyId]  DEFAULT ('0'),
	[SponsorFirmCode] [varchar](80) NOT NULL CONSTRAINT [DF_SmaOfferings_SponsorFirmCode]  DEFAULT (''),
	[SponsorFirmId] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_SponsorFirmId]  DEFAULT (''),
	[DoverSponsorFirmId] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_DoverSponsorFirmId]  DEFAULT (''),
	[MorningstarStrategyID] [varchar](100) NOT NULL CONSTRAINT [DF_SmaOfferings_SmaStrategyID]  DEFAULT (''),
	[MorningStarClass] [varchar](80) NOT NULL CONSTRAINT [DF_SmaOfferings_MorningStarClass]  DEFAULT (''),
	[MorningstarClassId] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_MorningstarClassId]  DEFAULT (''),
	[LadderCode] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_LadderCode]  DEFAULT (''),
	[SmidCode] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_SmidCode]  DEFAULT (''),
	[EsgCode] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_EsgCode]  DEFAULT (''),
	[SmartBetaCode] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_SmartBetaCode]  DEFAULT (''),
	[TotalAccounts] [varchar](50) NOT NULL CONSTRAINT [DF_SmaOfferings_TotalAccounts]  DEFAULT (''),
	[CsvFileRow] [int] NOT NULL CONSTRAINT [DF_SmaOfferings_Row]  DEFAULT ((0)),
 CONSTRAINT [PK_SmaOfferings] PRIMARY KEY CLUSTERED 
(
	[AssetManagerCode] ASC,
	[SponsorFirm] ASC,
	[AdvisoryPlatform] ASC,
	[AdvisoryPlatformCode] ASC,
	[SmaStrategy] ASC,
	[SmaProductType] ASC,
	[SmaProductTypeCode] ASC,
	[TampRIAPlatform] ASC,
	[ManagerClass] ASC,
	[SmaOfferingKeyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SmaReturns]    Script Date: 9/5/2018 12:41:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SmaReturns](
	[SmaReturnId] [int] IDENTITY(1001,1) NOT NULL,
	[SmaStrategyId] [int] NOT NULL,
	[AssetManagerCode] [nchar](10) NOT NULL,
	[ReturnType] [varchar](50) NOT NULL,
	[ReturnDate] [date] NOT NULL,
	[ReturnValue] [varchar](50) NOT NULL CONSTRAINT [DF_SmaReturns_Return]  DEFAULT (''),
	[ReturnValueD] [decimal](18, 8) NULL,
 CONSTRAINT [PK_SmaReturns] PRIMARY KEY CLUSTERED 
(
	[SmaStrategyId] ASC,
	[AssetManagerCode] ASC,
	[ReturnType] ASC,
	[ReturnDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SmaStrategies]    Script Date: 9/5/2018 12:41:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SmaStrategies](
	[SmaStrategyId] [int] IDENTITY(10001,1) NOT NULL,
	[AssetManagerCode] [varchar](10) NOT NULL CONSTRAINT [DF_SmaStrategies_AssetManager]  DEFAULT (''),
	[SmaStrategy] [varchar](80) NOT NULL CONSTRAINT [DF_SmaStrategies_SmaStrategy]  DEFAULT (''),
	[MorningstarStrategyId] [varchar](100) NOT NULL CONSTRAINT [DF_SmaStrategies_MorningstarClassId]  DEFAULT (''),
	[MorningstarClass] [varchar](80) NOT NULL,
	[MorningstarClassId] [varchar](50) NOT NULL CONSTRAINT [DF_SmaStrategies_MorningstaClassId]  DEFAULT (''),
	[ManagerClass] [varchar](120) NOT NULL CONSTRAINT [DF_SmaStrategies_ManagerClass]  DEFAULT (''),
	[InceptionDate] [date] NOT NULL CONSTRAINT [DF_SmaStrategies_InceptionDate]  DEFAULT ('01/01/1900'),
 CONSTRAINT [PK_SmaStrategies] PRIMARY KEY CLUSTERED 
(
	[AssetManagerCode] ASC,
	[SmaStrategy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [DoverSma] SET  READ_WRITE 
GO
