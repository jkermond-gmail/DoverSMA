
declare @SponsorFirmCode varchar(80);
set @SponsorFirmCode = 'Ameriprise';
declare @FlowDate date;
set @FlowDate = '03/31/2016';
declare @MorningstarClassId varchar(80);
set @MorningstarClassId = 'CI'


select sum(AssetsD) as Opportunity FROM SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
and AssetsD is not null

select sum(FinalNetFlowsD) as Opportunity FROM SmaOfferings o
inner join SmaFlows on o.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and MorningstarClassId = @MorningstarClassId 
and FinalNetFlowsD is not null
