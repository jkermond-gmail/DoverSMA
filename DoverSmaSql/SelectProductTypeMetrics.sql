/****** Script for SelectTopNRows command from SSMS  ******/
use DoverSma
SELECT distinct 
      [DoverSponsorFirmId]
      ,[SmaProductTypeCode]
      ,[MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where DoverSponsorFirmId not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
  order by DoverSponsorFirmId, SmaProductTypeCode, MorningstarClassId

SELECT distinct 
      [DoverSponsorFirmId]
      ,[SmaProductTypeCode]
      ,[MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where DoverSponsorFirmId not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
  order by MorningstarClassId

SELECT distinct 
      [DoverSponsorFirmId]
      ,[SmaProductTypeCode]
      ,[MorningstarClassId]
  FROM [DoverSma].[dbo].[SmaOfferings]
  where DoverSponsorFirmId not in ('', 'tbd') and MorningstarClassId not in  ( '0', '', 'tbd')
  order by SmaProductTypeCode

select * from SmaOfferings
where DoverSponsorFirmId = 't1248' and SmaProductTypeCode = 'Trad' and MorningstarClassId = 'MA'

select SmaOfferingId from SmaOfferings
where DoverSponsorFirmId = 't1248' and SmaProductTypeCode = 'Trad' and MorningstarClassId = 'MA'

select * from SmaFlows where SmaOfferingId in 
(select SmaOfferingId from SmaOfferings
where DoverSponsorFirmId = 't1248' and SmaProductTypeCode = 'Trad' and MorningstarClassId = 'MA')

select sum(AssetsD) from SmaFlows where SmaOfferingId in 
(select SmaOfferingId from SmaOfferings
where DoverSponsorFirmId = 't1248' and SmaProductTypeCode = 'Trad' and MorningstarClassId = 'MA' and FlowDate = '03/31/2016')


/*
SELECT *
  FROM [DoverSma].[dbo].[SmaOfferings]
  where MorningstarClassId in  ( 'allcap')
  */
  

  select sum(AssetsD) as TheSum
  from SmaFlows where SmaOfferingId in 
  (select SmaOfferingId from SmaOfferings 
   where SponsorFirmCode = 'Merrill' and SmaProductTypeCode = 'dual' and MorningstarClassId = 'CI') and FlowDate = '03/31/2016'

select * from SmaFlows where SmaOfferingId in 
(select SmaOfferingId from SmaOfferings 
   where SponsorFirmCode = 'Merrill' and SmaProductTypeCode = 'dual' and MorningstarClassId = 'EM') and FlowDate = '03/31/2016'

select * from SmaFlows where SmaOfferingId in 
(select SmaOfferingId from SmaOfferings 
   where SponsorFirmCode = 'Merrill' and SmaProductTypeCode = 'dual' and MorningstarClassId = 'LG') and FlowDate = '03/31/2016'


select * from SmaFlows where SmaOfferingId in 
(select SmaOfferingId from SmaOfferings 
   where SponsorFirmCode = 'Merrill' and SmaProductTypeCode = 'dual' and MorningstarClassId = 'CI') and FlowDate = '03/31/2016'

