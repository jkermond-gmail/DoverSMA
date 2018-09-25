use doversma

SELECT distinct 
      [AssetManagerCode]
      ,[SmaStrategy]
      ,[MorningstarStrategyID]
      ,[MorningStarClass]
      ,[MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where [MorningstarClassId] in 
(SELECT       
	distinct [MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
where [MorningstarClassId] not in (
SELECT     distinct Code
FROM         MorningstarClassifications))
order by MorningstarClassId

Update MorningstarClassifications
set CategoryGroup = 'U.S. Equity', AssetClass = 'Domestic Equity', AssetType = 'Equity'
WHERE     (Code IN (
'all-cap'
))

/*
MorningstarClassId = 'all-cap'
 */

 Update SmaOfferings set MorningstarClassId = 'all-cap'
 where MorningstarClassId = 'allcap'

Update SmaOfferings set MorningstarClassId = 'other'
 where MorningstarClassId  in
 (
 '', '0', 'AG', 'CUSTBAL', 'CUSTEQ', 'CUSTMUNI', 'DAC', 'MZ', 'SW', 'TBD', 'WB')