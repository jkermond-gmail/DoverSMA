
Update [DoverSma].[dbo].[SmaFlows]
      set 
	  [OpProductTypeAssets] = null
      ,[OpProductTypeGrossFlows] = null
      ,[OpProductTypeRedemptions] = null
      ,[OpProductTypeFinalNetFlows] = null
      ,[OpMorningstarClassAssets] = null
      ,[OpMorningstarClassGrossFlows] = null
      ,[OpMorningstarClassRedemptions] = null
      ,[OpMorningstarClassFinalNetFlows] = null
      ,[OpSponsorAssets] = null
      ,[OpSponsorGrossFlows] = null
      ,[OpSponsorRedemptions] = null
      ,[OpSponsorFinalNetFlows] = null
	  ,[AssetsByManager] = null
      ,[FinalNetByManager] = null
      ,[AssetShareByProductType] = null
      ,[AssetShareByMorningstarClass] = null
      ,[AssetShareBySponsor] = null
      ,[AssetShareByManager] = null
      ,[FinalNetShareByProductType] = null
      ,[FinalNetShareByMorningstarClass] = null
      ,[FinalNetShareBySponsor] = null
      ,[FinalNetShareByManager] = null
      ,[NumAssetsByProductType] = null
      ,[NumAssetsByMorningstarClass] = null
      ,[NumAssetsBySponsor] = null
      ,[NumFinalNetByProductType] = null
      ,[NumFinalNetByMorningstarClass] = null
      ,[NumFinalNetBySponsor] = null
      ,[RankAssetsByProductType] = null
      ,[RankAssetsByMorningstarClass] = null
      ,[RankAssetsBySponsor] = null
      ,[RankAssetsByManager] = null
      ,[RankFinalNetByProductType] = null
      ,[RankFinalNetByMorningstarClass] = null
      ,[RankFinalNetBySponsor] = null
      ,[RankFinalNetByManager] = null
      ,[NumManagersByProductType] = null
      ,[NumManagersByMorningstarClass] = null
      ,[NumManagersBySponsor] = null
      ,[NumManagersBySponsorFinalNet] = null

Update [DoverSma].[dbo].[SmaFlows]
      set 	  
	  [RankFinalNetBySponsorManager] = null,
	  [NumFinalNetBySponsorManager] = null
  