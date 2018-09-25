	

Update MorningstarClassifications
set CategoryGroup = 'Municipal Bond', AssetClass = 'Municipal Fixed Income', AssetType = 'Fixed Income'
WHERE     (Code IN (
'ML',
'MI',
'MS',
'HM',
'SL',
'SI',
'SL',
'MC',
'MF',
'MT',
'SM',
'MJ',
'MY',
'MN',
'MO',
'MP'
))


SELECT     TOP (200) Code, CodeDesc, CategoryGroup, AssetClass, AssetType
FROM         MorningstarClassifications
WHERE      (Code IN (
'ML',
'MI',
'MS',
'HM',
'SL',
'SI',
'SL',
'MC',
'MF',
'MT',
'SM',
'MJ',
'MY',
'MN',
'MO',
'MP'
))
ORDER BY CodeDesc