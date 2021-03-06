
SELECT count(*)
  FROM [DoverSma].[dbo].[SmaFlows]
  where 
      [OpProductTypeAssets] is not null OR
      [OpProductTypeGrossFlows] is not null OR
      [OpProductTypeRedemptions] is not null OR
      [OpProductTypeFinalNetFlows] is not null OR
      [OpMorningstarClassAssets] is not null OR
      [OpMorningstarClassGrossFlows] is not null OR
      [OpMorningstarClassRedemptions] is not null OR
      [OpMorningstarClassFinalNetFlows] is not null OR
      [OpSponsorAssets] is not null OR
      [OpSponsorGrossFlows]is not null OR
      [OpSponsorRedemptions] is not null OR
      [OpSponsorFinalNetFlows] is not null
