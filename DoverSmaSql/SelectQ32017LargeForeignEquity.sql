use DoverSma
SELECT [SmaOfferings].[SmaOfferingId]
      ,[SmaOfferingKeyId]
	  ,[FlowDate]
	  ,[AssetManager]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SmaStrategy]
      ,[MorningstarStrategyID]
      ,[SmaProductType]
      ,[SmaProductTypeCode]
      ,[MorningStarClass]
      ,[MorningstarClassId]
	  ,[AssetsD]
	  ,[GrossFlowsD]
	  ,[RedemptionsD]
	  ,[NetFlowsD]
	  ,[DerivedFlowsD]
	  ,[DoverDerivedFlowsD]
	  ,[PerformanceImpactD]
	  ,[FinalNetFlowsD]
  FROM [DoverSma].[dbo].[SmaOfferings]
  INNER join [DoverSma].[dbo].[AssetManagers] on AssetManagers.AssetManagerCode = SmaOfferings.AssetManagerCode
  Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  where FlowDate = '09/30/2017' and MorningstarClassId = 'FB'
  order by AssetManagers.AssetManager,  SmaOfferings.SmaOfferingId, SmaFlows.SmaFlowId
