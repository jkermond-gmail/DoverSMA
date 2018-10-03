use DoverSma

SELECT o.AssetManagerCode, FlowDate, SponsorFirmCode, SmaStrategy, MorningstarClassId, m.CodeDesc as MorningstarClassDesc, sum(AssetsD) as AssetsTotal, sum(FinalNetFlowsD) as FinalNetTotal
FROM SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
inner join MorningstarClassifications m on o.MorningstarClassId = m.Code
inner join AssetManagers a on o.AssetManagerCode = a.AssetManagerCode
where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where InSponsorAmountsScorecard = 'Y')
and AssetsD is not null 
group by o.AssetManagerCode, SponsorFirmCode, o.MorningstarClassId, m.CodeDesc,SmaStrategy, FlowDate
order by SponsorFirmCode, o.MorningstarClassId,  m.CodeDesc, AssetsTotal desc, o.AssetManagerCode, SmaStrategy, FlowDate

SELECT top 20 SponsorFirmCode, sum(AssetsD) as AssetsTotal
FROM SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
where FlowDate = '03/31/2018'
group by SponsorFirmCode 
order by AssetsTotal desc


declare @SponsorFirmCode varchar(80);
set @SponsorFirmCode = 'Schwab';
declare @FlowDate date;
set @FlowDate = '03/31/2018';
declare @MorningstarClassId varchar(80);
set @MorningstarClassId = 'AL'

--SELECT o.AssetManagerCode, FlowDate, SmaStrategy, MorningstarClassId, m.CodeDesc as MorningstarClassDesc, 
--sum(AssetsD) as AssetsTotal, sum(FinalNetFlowsD) as FinalNetTotal
--FROM SmaOfferings o
--inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
--inner join MorningstarClassifications m on o.MorningstarClassId = m.Code
--inner join AssetManagers a on o.AssetManagerCode = a.AssetManagerCode
--where SponsorFirmCode = @SponsorFirmCode
--and AssetsD is not null and AssetsD > 0 
--group by o.AssetManagerCode, o.MorningstarClassId, m.CodeDesc, SmaStrategy, FlowDate
--order by o.MorningstarClassId,  o.AssetManagerCode, SmaStrategy, FlowDate 

select count(*)  FROM (select distinct SmaStrategy, SponsorFirmCode, MorningstarClassId from SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
and AssetsD is not null and AssetsD > 0) as theCount

select distinct SmaStrategy, SponsorFirmCode, MorningstarClassId  FROM SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
and AssetsD is not null and AssetsD > 0
order by SmaStrategy

select SmaStrategy, SponsorFirmCode, MorningstarClassId, sum(AssetsD) as AssetsTotal FROM SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
and AssetsD is not null and AssetsD > 0
group by SmaStrategy, SponsorFirmCode, MorningstarClassId
order by AssetsTotal desc


