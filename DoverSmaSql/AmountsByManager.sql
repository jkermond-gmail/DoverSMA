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

use DoverSma
update SmaFlows set AssetsBySponsorManager = '4.05117904' where SmaFlowId in 
(SELECT 
f.SmaFlowId
FROM [DoverSma].[dbo].[SmaOfferings] o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode = 'Adhesion' and f.AssetManagerCode = 'prin' and FlowDate = '03/31/2017' and AssetsD > 0
)

SELECT 
SponsorFirmCode, FlowDate, o.AssetManagerCode, f.SmaFlowId, AssetsD, AssetsBySponsorManager, AssetShareBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('')  and AssetsBySponsorManager > 0
order by SponsorFirmCode, FlowDate, o.AssetManagerCode, f.SmaFlowId

SELECT distinct
SponsorFirmCode, FlowDate, o.AssetManagerCode, AssetShareBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('')  and AssetShareBySponsorManager > 0
order by SponsorFirmCode, FlowDate, AssetShareBySponsorManager desc


                    SELECT distinct SponsorFirmCode, FlowDate, o.AssetManagerCode, f.SmaFlowId, AssetsBySponsorManager
                    FROM SmaOfferings o
                    Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
                    where SponsorFirmCode not in ('')  and AssetsBySponsorManager > 0
                    order by SponsorFirmCode, FlowDate, o.AssetManagerCode, f.SmaFlowId

                    SELECT distinct SponsorFirmCode, FlowDate, o.AssetManagerCode, AssetsBySponsorManager
                    FROM SmaOfferings o
                    Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
                    where SponsorFirmCode not in ('')  and AssetsBySponsorManager > 0
                    order by SponsorFirmCode, FlowDate, o.AssetManagerCode

					SELECT o.SponsorFirmCode, SmaFlowId
                    FROM SmaFlows f
                    Inner join SmaOfferings o on o.SmaOfferingId = f.SmaOfferingId
                    order by SponsorFirmCode, FlowDate, o.AssetManagerCode

                    where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and f.AssetManagerCode = @AssetManagerCode

SELECT 
SponsorFirmCode, FlowDate, sum(AssetsBySponsorManager) as AssetsBySponsor
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and AssetsBySponsorManager > 0
group by [SponsorFirmCode], FlowDate
order by [SponsorFirmCode], FlowDate

SELECT 
sum(AssetsBySponsorManager) as AssetsBySponsor
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and AssetsBySponsorManager > 0
and SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate
group by [SponsorFirmCode], FlowDate
order by [SponsorFirmCode], FlowDate

SELECT 
sum(AssetsBySponsorManager) as AssetsBySponsor
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and AssetsBySponsorManager > 0
and SponsorFirmCode = 'Adhesion' and FlowDate = '03/31/2017'
group by [SponsorFirmCode], FlowDate
order by [SponsorFirmCode], FlowDate



SELECT 
[SponsorFirmCode], FlowDate, o.AssetManagerCode, sum(AssetsD) as TheSum
FROM [DoverSma].[dbo].[SmaOfferings] o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and AssetsD > 0
group by [SponsorFirmCode], FlowDate, o.AssetManagerCode
order by [SponsorFirmCode], FlowDate, o.AssetManagerCode

SELECT 
[SponsorFirmCode], FlowDate, sum(AssetsD) as AssetsBySponsor
FROM [DoverSma].[dbo].[SmaOfferings] o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and AssetsD > 0
group by [SponsorFirmCode], FlowDate
order by [SponsorFirmCode], FlowDate



 select AssetManagerCode, FlowDate, sum(AssetsD) as TheSum
 FROM [DoverSma].[dbo].[SmaFlows]
 group by AssetManagerCode, FlowDate

use DoverSma

SELECT 
[SponsorFirmCode],  o.AssetManagerCode, FlowDate, sum(FinalNetFlowsD) as FinalNetFlowsQtr
FROM [DoverSma].[dbo].[SmaOfferings] o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and SponsorFirmCode = 'wells fargo' and FinalNetFlowsD is not null
group by [SponsorFirmCode], FlowDate, o.AssetManagerCode
order by [SponsorFirmCode], FlowDate, sum(FinalNetFlowsD) desc, o.AssetManagerCode

SELECT 
[SponsorFirmCode],  o.AssetManagerCode, FlowDate, sum(FinalNetFlowsD) as FinalNetFlowsQtr
FROM [DoverSma].[dbo].[SmaOfferings] o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('') and SponsorFirmCode = 'wells fargo' and o.AssetManagerCode = 'alli' and FinalNetFlowsD is not null
group by [SponsorFirmCode], FlowDate, o.AssetManagerCode
order by [SponsorFirmCode], FlowDate, sum(FinalNetFlowsD) desc, o.AssetManagerCode

