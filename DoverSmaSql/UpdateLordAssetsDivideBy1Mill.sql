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
  FROM [DoverSma].[dbo].[SmaFlows]
    where AssetManagerCode = 'lord' and
	SmaFlowId = '293567'

	/* flow id 293564 
	   flwo id 293567 is null*/
	/* assetsD 15513444.66000000 */

	update [DoverSma].[dbo].[SmaFlows] set assetsD = assetsD/1000000
	where AssetManagerCode = 'lord'