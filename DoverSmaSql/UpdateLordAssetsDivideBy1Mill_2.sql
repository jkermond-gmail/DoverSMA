select '12/31/2017', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '12/31/2017') and AssetManagerCode = 'lord'

Update [DoverSma].[dbo].[SmaFlows]
set AssetsD = AssetsD/1000000
where (AssetsD is not null) and (AssetManagerCode = 'lord')
