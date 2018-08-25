USE [master]
GO

/****** Object:  Database [DoverSma]    Script Date: 8/25/2018 12:59:24 PM ******/
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

ALTER DATABASE [DoverSma] SET  READ_WRITE 
GO

