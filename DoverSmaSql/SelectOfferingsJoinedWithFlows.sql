/****** Script for SelectTopNRows command from SSMS  ******/
use DoverSma
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
	  ,[AssetsD]
	  ,[GrossFlowsD]
	  ,[RedemptionsD]
	  ,[NetFlowsD]
	  ,[DerivedFlowsD]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  order by AssetManagers.AssetManager,  SmaOfferings.SmaOfferingId, SmaFlows.SmaFlowId

