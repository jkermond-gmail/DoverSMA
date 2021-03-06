/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [SmaOfferingId]
      ,[AssetManagerCode]
      ,[SponsorFirm]
      ,[AdvisoryPlatform]
      ,[SmaStrategy]
      ,[SmaProductType]
      ,[TampRIAPlatform]
      ,[ManagerClass]
      ,[SponsorFirmId]
      ,[MorningstarStrategyID]
      ,[MorningStarClass]
      ,[MorningstarClassId]
      ,[TotalAccounts]
      ,[CsvFileRow]
  FROM [DoverSma].[dbo].[SmaOfferings]
  /*
  delete FROM [DoverSma].[dbo].[SmaOfferings]
  
  delete FROM [DoverSma].[dbo].[SmaFlows]
  
  delete FROM [DoverSma].[dbo].[SmaStrategies]
  
  delete FROM [DoverSma].[dbo].[SmaReturns]
 

  delete FROM [DoverSma].[dbo].[SmaOfferings]
  where AssetManagerCode = 'lord'

  delete FROM [DoverSma].[dbo].[SmaFlows]
  where AssetManagerCode = 'lord'

  delete FROM [DoverSma].[dbo].[SmaStrategies]
  where AssetManagerCode = 'alli'

  delete FROM [DoverSma].[dbo].[SmaReturns]
  where AssetManagerCode = 'alli'

  */