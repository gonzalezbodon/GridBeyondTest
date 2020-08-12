USE [master]
GO
/****** Object:  Database [GridBeyondTest]    Script Date: 11/08/2020 22:48:13 ******/
CREATE DATABASE [GridBeyondTest]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GridBeyondTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GridBeyondTest.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GridBeyondTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\GridBeyondTest_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [GridBeyondTest] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GridBeyondTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GridBeyondTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GridBeyondTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GridBeyondTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GridBeyondTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GridBeyondTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [GridBeyondTest] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GridBeyondTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GridBeyondTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GridBeyondTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GridBeyondTest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GridBeyondTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GridBeyondTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GridBeyondTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GridBeyondTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GridBeyondTest] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GridBeyondTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GridBeyondTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GridBeyondTest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GridBeyondTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GridBeyondTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GridBeyondTest] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GridBeyondTest] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GridBeyondTest] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GridBeyondTest] SET  MULTI_USER 
GO
ALTER DATABASE [GridBeyondTest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GridBeyondTest] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GridBeyondTest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GridBeyondTest] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GridBeyondTest] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GridBeyondTest] SET QUERY_STORE = OFF
GO
USE [GridBeyondTest]
GO
/****** Object:  Table [dbo].[MarketRegister]    Script Date: 11/08/2020 22:22:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarketRegister](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[Price] [float] NOT NULL,
 CONSTRAINT [UK_MarketRegister_RegistrationDate] UNIQUE NONCLUSTERED 
(
	[RegistrationDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[MarketHourlyPrices]    Script Date: 11/08/2020 22:22:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[MarketHourlyPrices]
AS
SELECT Id1, Id2, StartDate, ISNULL(Price1, 0) + ISNULL(Price2, 0) AS HourPrice
FROM     (SELECT mr1.Id AS Id1, mr1.RegistrationDate AS StartDate, mr1.Price AS Price1, mr2.Id AS Id2, mr2.Price AS Price2
                  FROM      dbo.MarketRegister AS mr1 LEFT OUTER JOIN
                                    dbo.MarketRegister AS mr2 ON CONVERT(CHAR(16), DATEADD(MINUTE, 30, mr1.RegistrationDate), 120) = CONVERT(CHAR(16), mr2.RegistrationDate, 120)) AS MarketHourlyPrices_1
GO
/****** Object:  View [dbo].[CommonStatistics]    Script Date: 11/08/2020 22:22:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CommonStatistics]
AS
SELECT TOP (1) StartDate AS MostExpensiveHourDate, HourPrice AS MostExpensiveHourPrice,
                      (SELECT MIN(Price) AS Expr1
                       FROM      dbo.MarketRegister) AS MinPrice,
                      (SELECT MAX(Price) AS Expr1
                       FROM      dbo.MarketRegister AS MarketRegister_2) AS MaxPrice,
                      (SELECT AVG(Price) AS Expr1
                       FROM      dbo.MarketRegister AS MarketRegister_1) AS AvgPrice
FROM     dbo.MarketHourlyPrices
ORDER BY MostExpensiveHourPrice DESC
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique row per date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MarketRegister', @level2type=N'CONSTRAINT',@level2name=N'UK_MarketRegister_RegistrationDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "MarketHourlyPrices"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CommonStatistics'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CommonStatistics'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "MarketHourlyPrices_1"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 242
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1176
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1356
         SortOrder = 1416
         GroupBy = 1350
         Filter = 1356
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MarketHourlyPrices'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MarketHourlyPrices'
GO
