/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [SmaFlowId]
      ,[SmaOfferingId]
      ,[AssetManagerCode]
      ,[FlowDate]
      ,[Assets]
      ,[GrossFlows]
      ,[Redemptions]
      ,[NetFlows]
      ,[DerivedFlows]
      ,[AssetsD]
      ,[GrossFlowsD]
      ,[RedemptionsD]
      ,[NetFlowsD]
      ,[DerivedFlowsD]
      ,[DoverDerivedFlowsD]
      ,[FinalNetFlowsD]
      ,[PerformanceImpactD]
      ,[OpProductTypeAssets]
      ,[OpProductTypeGrossFlows]
      ,[OpProductTypeRedemptions]
      ,[OpProductTypeFinalNetFlows]
      ,[OpMorningstarClassAssets]
      ,[OpMorningstarClassGrossFlows]
      ,[OpMorningstarClassRedemptions]
      ,[OpMorningstarClassFinalNetFlows]
      ,[OpSponsorAssets]
      ,[OpSponsorGrossFlows]
      ,[OpSponsorRedemptions]
      ,[OpSponsorFinalNetFlows]
      ,[AssetsByManager]
      ,[FinalNetByManager]
      ,[AssetShareByProductType]
      ,[AssetShareByMorningstarClass]
      ,[AssetShareBySponsor]
      ,[AssetShareByManager]
      ,[FinalNetShareByProductType]
      ,[FinalNetShareByMorningstarClass]
      ,[FinalNetShareBySponsor]
      ,[FinalNetShareByManager]
      ,[NumAssetsByProductType]
      ,[NumAssetsByMorningstarClass]
      ,[NumAssetsBySponsor]
      ,[NumFinalNetByProductType]
      ,[NumFinalNetByMorningstarClass]
      ,[NumFinalNetBySponsor]
      ,[RankAssetsByProductType]
      ,[RankAssetsByMorningstarClass]
      ,[RankAssetsBySponsor]
      ,[RankAssetsByManager]
      ,[RankFinalNetByProductType]
      ,[RankFinalNetByMorningstarClass]
      ,[RankFinalNetBySponsor]
      ,[RankFinalNetByManager]
      ,[NumManagersByProductType]
      ,[NumManagersByMorningstarClass]
      ,[NumManagersBySponsor]
      ,[NumManagersBySponsorFinalNet]
  FROM [DoverSma].[dbo].[SmaFlows]

  /*
SELECT agent_code, 
SUM (advance_amount) 
FROM orders 
GROUP BY agent_code;
 */
 use DoverSma
 select AssetManagerCode, FlowDate, sum(AssetsD) as TheSum
 FROM [DoverSma].[dbo].[SmaFlows]
 group by AssetManagerCode, FlowDate
 order by AssetManagerCode, FlowDate asc

 update SmaFlows set AssetsByManager = @Amount
 where AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate 
 and AssetsD > 0

 select distinct AssetManagerCode, FlowDate, AssetsByManager, 
 AssetsTotal = (select sum(AssetsByManager) 
 FROM [DoverSma].[dbo].[SmaFlows]
 where AssetsByManager is not null and FlowDate = '03/31/2016')
 FROM [DoverSma].[dbo].[SmaFlows]
 where AssetsByManager is not null and FlowDate = '03/31/2016'
 
 
 order by AssetManagerCode, FlowDate, AssetsByManager

 select sum(AssetsByManager) , FlowDate
 FROM [DoverSma].[dbo].[SmaFlows]
 where AssetsByManager is not null
 group by FlowDate
 order by FlowDate

 use DoverSma
 select AssetManagerCode, FlowDate, sum(FinalNetFlowsD) as TheSum
 FROM [DoverSma].[dbo].[SmaFlows]
 group by AssetManagerCode, FlowDate
 order by AssetManagerCode, FlowDate
 
 update SmaFlows set FinalNetByManager = @Amount
 where AssetManagerCode = @AssetManagerCode and FlowDate = @FlowDate 
 and FinalNetFlowsD > 0

 select distinct AssetManagerCode, FlowDate, FinalNetByManager
 FROM [DoverSma].[dbo].[SmaFlows]
 where FinalNetByManager is not null
 order by AssetManagerCode, FlowDate, FinalNetByManager