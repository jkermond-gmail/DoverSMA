/****** Script for SelectTopNRows command from SSMS  ******/

SELECT [SmaOfferings].[SmaOfferingId]
      ,[SmaOfferingKeyId]
	  ,[FlowDate]
	  ,[AssetManager]
      ,[SmaOfferings].[AssetManagerCode]
      ,[TampRIAPlatform]
      ,[SponsorFirm]
      ,[SponsorFirmId]
      ,[AdvisoryPlatform]
      ,[AdvisoryPlatformCode]
      ,[SmaStrategy]
      ,[MorningstarStrategyID]
      ,[SmaProductType]
      ,[SmaProductTypeCode]
      ,[MorningStarClass]
      ,[MorningstarClassId]
      ,[ManagerClass]
      ,[TotalAccounts]
      ,[CsvFileRow]
	  ,[Assets]
	  ,[GrossFlows]
	  ,[Redemptions]
	  ,[NetFlows]
	  ,[DerivedFlows]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  order by AssetManagers.AssetManager,  SmaOfferings.SmaOfferingId, SmaFlows.SmaFlowId

