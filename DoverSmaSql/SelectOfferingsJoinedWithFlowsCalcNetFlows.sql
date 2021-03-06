/****** Script for SelectTopNRows command from SSMS  ******/
use DoverSma
SELECT [SmaOfferings].[SmaOfferingId]
      ,[SmaOfferingKeyId]
	  ,[SmaOfferings].[AssetManagerCode]
	  ,[FlowDate]
      ,[SmaOfferings].[AssetManagerCode]
      ,[MorningstarStrategyID]
	  ,[AssetsD]
	  ,[GrossFlowsD]
	  ,[RedemptionsD]
	  ,[NetFlowsD]
	  ,[DerivedFlowsD]
  FROM [DoverSma].[dbo].[SmaOfferings]
  inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
  order by SmaOfferings.AssetManagerCode, SmaOfferings.SmaOfferingId, SmaFlows.SmaFlowId, SmaFlows.FlowDate

