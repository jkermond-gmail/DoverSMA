/****** Script for SelectTopNRows command from SSMS  ******/

INSERT INTO SmaFlows (
      [SmaOfferingId]
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
      ,[OpProdTypeAssets]
      ,[OpProdTypeGross]
      ,[OpProdTypeRedemptions]
      ,[OpProdTypeFinalNetFlows]
)
SELECT 
      [SmaOfferingId]
      ,[AssetManagerCode]
      ,'03/31/2018'
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
      ,[OpProdTypeAssets]
      ,[OpProdTypeGross]
      ,[OpProdTypeRedemptions]
      ,[OpProdTypeFinalNetFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  where AssetManagerCode = 'laza' and FlowDate = '12/31/2017'

  /*
  delete
  FROM [DoverSma].[dbo].[SmaFlows]
  where AssetManagerCode = 'laza' and FlowDate = '03/31/2018'
  */

  SELECT 
      [SmaOfferingId]
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
      ,[OpProdTypeAssets]
      ,[OpProdTypeGross]
      ,[OpProdTypeRedemptions]
      ,[OpProdTypeFinalNetFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  where AssetManagerCode = 'laza' and FlowDate = '03/31/2018'
