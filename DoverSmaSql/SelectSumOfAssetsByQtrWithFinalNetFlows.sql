
  select '03/31/2016', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '03/31/2016')

  select '06/30/2016', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '06/30/2016')

  select '09/30/2016', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '09/30/2016')

  select '12/31/2016', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '12/31/2016')


  select '03/31/2017', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '03/31/2017')

  select '06/30/2017', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '06/30/2017')

  select '09/30/2017', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '09/30/2017')

  select '12/31/2017', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '12/31/2017')

  select '03/31/2018', sum(AssetsD) as SumOfAssets, sum(FinalNetFlowsD) as SumOfFinalNetFlows
  FROM [DoverSma].[dbo].[SmaFlows]
  where (FinalNetFlowsD is not null) and (FlowDate = '03/31/2018')
