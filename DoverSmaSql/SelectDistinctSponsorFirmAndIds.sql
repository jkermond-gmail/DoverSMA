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
  where AssetManagerCode = 'Nuve'

 select count(*)  FROM [DoverSma].[dbo].[SmaFlows]
  where AssetManagerCode = 'Nuve'

use Doversma
select distinct SponsorFirm, SponsorFirmId from [DoverSma].[dbo].[SmaOfferings]
order by SponsorFirmId 