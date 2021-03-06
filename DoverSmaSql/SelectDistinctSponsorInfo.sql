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
      [SponsorFirm]
      ,[SponsorFirmCode]
      ,[SponsorFirmId]
      ,[DoverSponsorFirmId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  order by SponsorFirmCode

