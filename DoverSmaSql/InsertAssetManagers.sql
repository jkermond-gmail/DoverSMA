/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [SmaOfferingId]
      ,[AssetManagerCode]
      ,[SponsorFirm]
      ,[AdvisoryPlatform]
      ,[AdvisoryPlatformCode]
      ,[SmaStrategy]
      ,[SmaProductType]
      ,[SmaProductTypeCode]
      ,[TampRIAPlatform]
      ,[ManagerClass]
      ,[SmaOfferingKeyId]
      ,[SponsorFirmId]
      ,[MorningstarStrategyID]
      ,[MorningStarClass]
      ,[MorningstarClassId]
      ,[TotalAccounts]
      ,[CsvFileRow]
  FROM [DoverSma].[dbo].[SmaOfferings]

  SELECT distinct 
      [AssetManagerCode]
  FROM [DoverSma].[dbo].[SmaOfferings]

use DoverSma
INSERT INTO [DoverSma].[dbo].[AssetManagers] (AssetManagerCode)
  SELECT distinct 
      [AssetManagerCode]
  FROM [DoverSma].[dbo].[SmaOfferings]

  /*
  AssetManagerCode AssetManager
---------------- --------------------------------------------------
alli             Allianz Global Investors
anch             Anchor Capital LLC
bran             Brandes
cong             Congress Asset Management
fran             Franklin Templeton
gwnk             GW&K Investment Management  
inve             Invesco
laza             Lazard Asset Management LLC
legg             Legg Mason
prin             Principal
rena             Renaissance Investment Management

(11 row(s) affected)
  */