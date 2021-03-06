/****** Script for SelectTopNRows command from SSMS  ******/
Select * FROM [DoverSma].[dbo].[SmaFlows]
where assets in
(
SELECT distinct [Assets]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(Assets)=0
  )
  order by assets desc

SELECT distinct [GrossFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(GrossFlows)=0

SELECT distinct [Redemptions]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(Redemptions)=0

SELECT distinct [DerivedFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(DerivedFlows)=0


Select * FROM [DoverSma].[dbo].[SmaFlows]
where NetFlows in
(
SELECT distinct [NetFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(NetFlows)=0
)
order by netflows asc


  /*
  where AssetManagerCode = 'alli'
  where AssetManagerCode = 'anch'
  where AssetManagerCode = 'bran'
  where AssetManagerCode = 'cong'
  where AssetManagerCode = 'dela'
  where AssetManagerCode = 'fran'
  where AssetManagerCode = 'gwnk'
  where AssetManagerCode = 'inve'
  where AssetManagerCode = 'laza'
  where AssetManagerCode = 'legg'
  where AssetManagerCode = 'nuve'
  where AssetManagerCode = 'prin'
  where AssetManagerCode = 'rena'
  */

  Update [DoverSma].[dbo].[SmaFlows]
  set AssetsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set GrossFlowsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set RedemptionsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set NetFlowsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set DerivedFlowsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set DoverDerivedFlowsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set FinalNetFlowsD = Null

  Update [DoverSma].[dbo].[SmaFlows]
  set PerformanceImpactD = Null


Update [DoverSma].[dbo].[SmaFlows]
set assets = '0' where assets in
(
SELECT distinct [Assets]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(Assets)=0
  )
  
Update [DoverSma].[dbo].[SmaFlows]
set GrossFlows = '0' where GrossFlows in
(
SELECT distinct [GrossFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(GrossFlows)=0
  )

Update [DoverSma].[dbo].[SmaFlows]
set Redemptions = '0' where Redemptions in
(
SELECT distinct [Redemptions]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(Redemptions)=0
  )

Update [DoverSma].[dbo].[SmaFlows]
set NetFlows = '0' where NetFlows in
(
SELECT distinct [NetFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(NetFlows)=0
  )

Update [DoverSma].[dbo].[SmaFlows]
set DerivedFlows = '0' where DerivedFlows in
(
SELECT distinct [DerivedFlows]
  FROM [DoverSma].[dbo].[SmaFlows]
  WHERE ISNUMERIC(DerivedFlows)=0
  )

