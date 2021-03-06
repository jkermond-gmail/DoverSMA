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
  FROM [DoverSma].[dbo].[SmaFlows]

  use DoverSma
  select distinct AssetManagerCode, SmaOfferingId
  from SmaFlows

  SELECT [SmaFlowId]
      ,[SmaOfferingId]
      ,[AssetManagerCode]
      ,[FlowDate]
      ,[Assets]
      ,[GrossFlows]
      ,[Redemptions]
      ,[NetFlows]
      ,[DerivedFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  Where AssetManagerCode = 'alli'
  order by SmaOfferingId, FlowDate

  SELECT sum(cast(Assets as float))
  FROM [DoverSma].[dbo].[SmaFlows]
  Where AssetManagerCode = 'alli'


  select sum(cast(columnname as int)) from TableName

  select distinct assets FROM [DoverSma].[dbo].[SmaFlows]
  order by assets

