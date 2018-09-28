use DoverSma
SELECT distinct SponsorFirmCode, FlowDate, o.AssetManagerCode, AssetsBySponsorManager, AssetShareBySponsorManager, RankAssetsBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('')  and AssetsBySponsorManager > 0
order by SponsorFirmCode, FlowDate, AssetShareBySponsorManager desc

/*					and flowdate = '09/30/2016' */

                    SELECT sum(AssetsBySponsorManager) as AssetsBySponsor
                    FROM SmaOfferings o
                    Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
                    where AssetsBySponsorManager > 0
                    and SponsorFirmCode = @SponsorFirmCode and FlowDate = @FlowDate

                    SELECT sum(distinct AssetsBySponsorManager) as AssetsBySponsor
                    FROM SmaOfferings o
                    Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
                    where AssetsBySponsorManager > 0
                    and SponsorFirmCode = '1st Global' and FlowDate = '09/30/2016'

SELECT distinct SponsorFirmCode, FlowDate, o.AssetManagerCode, AssetsBySponsorManager, AssetShareBySponsorManager, RankAssetsBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode not in ('')  and AssetsBySponsorManager > 0
order by SponsorFirmCode, FlowDate, AssetShareBySponsorManager desc

SELECT distinct o.AssetManagerCode, FlowDate, SponsorFirmCode, FinalNetBySponsorManager, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where IncludeInScorecard = 'Y')  
and RankFinalNetBySponsorManager > 0 and NumFinalNetBySponsorManager > 0
order by o.AssetManagerCode, FlowDate, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager

SELECT distinct a.AssetManager, o.AssetManagerCode, FlowDate, SponsorFirmCode, FinalNetBySponsorManager, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager
FROM SmaOfferings o
Inner join SmaFlows f on o.SmaOfferingId = f.SmaOfferingId
inner join AssetManagers a on o.AssetManagerCode = a.AssetManagerCode
where SponsorFirmCode in (select SponsorFirmCode from SponsorFirms where IncludeInScorecard = 'Y')  
and RankFinalNetBySponsorManager > 0 and NumFinalNetBySponsorManager > 0
order by FlowDate, SponsorFirmCode, RankFinalNetBySponsorManager, NumFinalNetBySponsorManager



