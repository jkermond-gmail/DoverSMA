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


