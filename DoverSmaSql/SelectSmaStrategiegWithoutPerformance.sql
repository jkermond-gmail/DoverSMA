select distinct SmaStrategy FROM [DoverSma].[dbo].[SmaOfferings] where SmaStrategy not in
(select SmaStrategy from [DoverSma].[dbo].[SmaStrategies])
order by SmaStrategy

select SmaStrategy FROM [DoverSma].[dbo].[SmaStrategies] where SmaStrategy not in
(select SmaStrategy from [DoverSma].[dbo].[SmaOfferings])