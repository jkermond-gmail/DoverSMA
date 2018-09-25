use DoverSma
SELECT [SmaOfferings].[SmaOfferingId]
      ,[SmaOfferingKeyId]
	  ,[FlowDate]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SponsorFirmCode]
      ,[SmaStrategy]
      ,[MorningstarStrategyID]
      ,[SmaProductType]
      ,[SmaProductTypeCode]
      ,[MorningstarClassId]
	  ,[AssetsD]
	  ,[FinalNetFlowsD]
      ,[AssetsByManager]
	  ,[FinalNetByManager]
      ,[AssetsBySponsorManager]
      ,[FinalNetBySponsorManager]
      ,[AssetShareBySponsor]
      ,[AssetShareByManager]
      ,[AssetShareBySponsorManager]
      ,[FinalNetShareBySponsorManager]
      ,[FinalNetShareBySponsor]
      ,[FinalNetShareByManager]
      ,[NumAssetsBySponsor]
      ,[NumFinalNetBySponsor]
      ,[RankAssetsBySponsor]
      ,[RankAssetsByManager]
      ,[RankFinalNetBySponsor]
      ,[RankFinalNetByManager]
      ,[RankAssetsBySponsorManager]
      ,[RankRankFinalNetBySponsorManager]
      ,[NumManagersBySponsor]
      ,[NumManagersBySponsorFinalNet]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where SmaOfferings.AssetManagerCode = 'alli' and SponsorFirmCode = 'wells fargo'
  and FlowDate = '03/31/2016' and FinalNetFlowsD is not null
  order by AssetManagers.AssetManager,  SmaOfferings.SmaOfferingId, SmaFlows.SmaFlowId

  SELECT Distinct 
	   [FlowDate]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SponsorFirmCode]
      ,[AssetsBySponsorManager]
      ,[AssetShareBySponsorManager]
      ,[RankAssetsBySponsorManager]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where SmaOfferings.AssetManagerCode = 'alli' and SponsorFirmCode = 'wells fargo'
  and FlowDate = '03/31/2016' and FinalNetFlowsD is not null
  order by RankAssetsBySponsorManager, [SmaOfferings].[AssetManagerCode]

  SELECT Distinct 
	   [FlowDate]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SponsorFirmCode]
      ,[FinalNetBySponsorManager]
      ,[RankFinalNetBySponsorManager]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where /* SmaOfferings.AssetManagerCode = 'alli' and */ SponsorFirmCode = 'wells fargo'
  and FlowDate = '03/31/2016' and FinalNetFlowsD is not null
  order by FinalNetBySponsorManager desc, [SmaOfferings].[AssetManagerCode]

  SELECT Distinct 
	   [FlowDate]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SponsorFirmCode]
      ,[FinalNetBySponsorManager]
      ,[FinalNetShareBySponsorManager]
      ,[RankFinalNetBySponsorManager]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where /* SmaOfferings.AssetManagerCode = 'alli' and */ SponsorFirmCode = 'wells fargo'
  and FlowDate = '03/31/2016' and FinalNetFlowsD is not null

select distinct SponsorFirmCode
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where FlowDate = '03/31/2016' and FinalNetFlowsD is not null and SponsorFirmCode not in ('')
  order by SponsorFirmCode

(select count(*) from 
  (SELECT Distinct 
	   [FlowDate]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SponsorFirmCode]
      ,[FinalNetBySponsorManager]
      ,[FinalNetShareBySponsorManager]
      ,[RankFinalNetBySponsorManager]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where /* SmaOfferings.AssetManagerCode = 'alli' and */ SponsorFirmCode = 'wells fargo'
  and FlowDate = '03/31/2016' and FinalNetFlowsD is not null) as TheCount

/*
SELECT COUNT(*) 
FROM (SELECT DISTINCT DocumentId, DocumentSessionId
      FROM DocumentOutputItems) AS internalQuery  
	  */