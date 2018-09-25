
declare @FlowDate date;
SET @FlowDate = '03/31/2016';

select distinct SponsorFirmCode FROM SmaOfferings
Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
where FlowDate = @FlowDate and FinalNetFlowsD is not null and SponsorFirmCode not in ('')
order by SponsorFirmCode



DECLARE @SponsorFirmCode varchar(80);
set @SponsorFirmCode = 'Wells Fargo'


select count(*) as Num from
(SELECT Distinct
FlowDate, SmaOfferings.AssetManagerCode, SponsorFirmCode, RankFinalNetBySponsorManager
FROM SmaOfferings
Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and RankFinalNetBySponsorManager > 0) As Num2


declare @Num as int;
set @Num = 11;

update SmaFlows set NumFinalNetBySponsorManager = @Num where SmaFlowId in
(SELECT f.SmaFlowId
FROM SmaFlows f
Inner join SmaOfferings o on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate 
and RankFinalNetBySponsorManager > 0 )


SELECT Distinct
FlowDate, SmaOfferings.AssetManagerCode, SponsorFirmCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
FROM SmaOfferings
Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
where SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate and NumFinalNetBySponsorManager > 0
order by RankFinalNetBySponsorManager

SELECT Distinct
FlowDate, SmaOfferings.AssetManagerCode, SponsorFirmCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
FROM SmaOfferings
Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
order by FlowDate, SponsorFirmCode, SmaOfferings.AssetManagerCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager

SELECT Distinct
FlowDate, SmaOfferings.AssetManagerCode, SponsorFirmCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
FROM SmaOfferings
Inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
where RankFinalNetBySponsorManager is not NULL and  SmaOfferings.AssetManagerCode = 'alli' and SponsorFirmCode = 'wells fargo' 
order by SmaOfferings.AssetManagerCode, FlowDate, SponsorFirmCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager