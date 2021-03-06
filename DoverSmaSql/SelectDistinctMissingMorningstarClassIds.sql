/****** Script for SelectTopNRows command from SSMS  ******/
SELECT distinct 
      [AssetManagerCode]
      ,[SponsorFirm]
      ,[SponsorFirmCode]
      ,[SponsorFirmId]
      ,[DoverSponsorFirmId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  order by SponsorFirmCode, AssetManagerCode


  SELECT distinct 
      [AssetManagerCode]
      ,[SponsorFirm]
      ,[SponsorFirmCode]
      ,[SponsorFirmId]
      ,[DoverSponsorFirmId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  order by SponsorFirm, AssetManagerCode




  SELECT distinct 
      [SmaOfferingId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where [MorningstarClassId] in ( '0', 'TBD', '')
  order by SmaOfferingId


  SELECT distinct  [SmaOfferings].[SmaOfferingId]
      ,[SmaOfferingKeyId]
      ,[SmaOfferings].[AssetManagerCode]
      ,[SponsorFirmCode]
      ,[DoverSponsorFirmId]
      ,[SmaStrategy]
      ,[MorningstarStrategyID]
      ,[SmaProductType]
      ,[SmaProductTypeCode]
      ,[MorningStarClass]
      ,[MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where 
[SmaOfferings].[SmaOfferingId] in 
(  SELECT distinct 
      [SmaOfferingId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where [MorningstarClassId] in ( '0', 'TBD', '')
)
