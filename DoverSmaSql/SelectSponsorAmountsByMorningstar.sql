/****** Script for SelectTopNRows command from SSMS  ******/
use DoverSma
SELECT FlowDate, SponsorFirmCode, MorningstarClassId, m.CodeDesc, sum(AssetsD) as AssetsTotal, sum(FinalNetFlowsD) as FinalNetTotal
FROM SmaOfferings
inner join SmaFlows on SmaOfferings.SmaOfferingId = SmaFlows.SmaOfferingId
inner join MorningstarClassifications m on SmaOfferings.MorningstarClassId = m.Code
where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where InSponsorAmountsScorecard = 'Y')
group by SponsorFirmCode, SmaOfferings.MorningstarClassId, m.CodeDesc,FlowDate
order by SponsorFirmCode, SmaOfferings.MorningstarClassId, FlowDate
  

