use DoverSma
SELECT distinct a.AssetManager, o.AssetManagerCode, FlowDate, SponsorFirmCode, FinalNetBySponsorManager, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
inner join AssetManagers a on o.AssetManagerCode = a.AssetManagerCode
where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where InManagerSponsorScorecard = 'Y')  
and RankFinalNetBySponsorManager > 0 and NumFinalNetBySponsorManager > 0
order by FlowDate, SponsorFirmCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
