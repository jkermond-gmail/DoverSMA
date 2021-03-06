/****** Script for SelectTopNRows command from SSMS  ******/
use DoverSma
SELECT [SmaStrategies].[SmaStrategyId]
	  ,[AssetManager]
      ,[SmaStrategies].[AssetManagerCode]
      ,[SmaStrategy]
      ,[MorningstarStrategyId]
      ,[MorningstarClass]
      ,[MorningstarClassId]
      ,[ManagerClass]
      ,[InceptionDate]
	  ,[ReturnDate]
	  ,[ReturnType]
	  ,[ReturnValue]
  FROM [DoverSma].[dbo].[SmaStrategies]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaStrategies.AssetManagerCode
  Inner join [DoverSma].[dbo].[SmaReturns] on SmaReturns.SmaStrategyId = SmaStrategies.SmaStrategyId
  order by AssetManagers.AssetManager,  SmaStrategies.SmaStrategyId, SmaReturns.SmaReturnId

