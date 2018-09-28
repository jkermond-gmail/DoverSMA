SELECT distinct SponsorFirmCode
FROM SmaOfferings o order by SponsorFirmCode

/*
INSERT INTO SponsorFirms (SponsorFirmCode)
SELECT distinct SponsorFirmCode
FROM SmaOfferings o
*/
select * from SponsorFirms
where InManagerSponsorScorecard = 'Y'
order by SponsorFirmCode

select SponsorFirmCode from SponsorFirms
where InManagerSponsorScorecard = 'Y'
order by SponsorFirmCode


update SponsorFirms set InManagerSponsorScorecard = 'N' where
SponsorFirmCode not in ( 
'Merrill'
,'Citi'
,'Raymond James'
,'Adhesion'
,'MSWM'
,'LPL'
,'Stifel'
,'Edward Jones'
,'Janney'
,'PNC'
,'RBC'
,'Oppenheimer'
,'UBS'
,'Natixis'
,'Ameriprise'
)
/*
update SmaOfferings set SponsorFirmCode = 'SunTrust'
where SponsorFirmCode = 'SunTRust'
*/

update SponsorFirms set InSponsorAmountsScorecard = 'Y' where
SponsorFirmCode in ( 
'Adhesion'
,'Ameriprise'
,'Baird'
,'BB&T'
,'Brinker'
,'Capital One'
,'Cetera'
,'CIBC'
,'Citi'
,'Davidson'
,'Deutsche'
,'Edward Jones'
,'Envestnet'
,'Fidelity'
,'Folio'
,'Goldman'
,'Janney'
,'JPM'
,'Lincoln'
,'Lockwood'
,'LPL'
,'Merrill'
,'MSWM'
,'Natixis'
,'Northern Trust'
,'Oppenheimer'
,'Other'
,'Pershing'
,'Placemark'
,'PNC'
,'Prudential'
,'Raymond James'
,'RBC'
,'Richardson'
,'Schwab'
,'SEI'
,'Snowden'
,'Stephens'
,'Stifel'
,'SunTrust'
,'TD Ameritrade'
,'UBS'
,'Wells Fargo'
,'William Blair'
)

(select SponsorFirmCode from SponsorFirms where InSponsorAmountsScorecard = 'Y')

SELECT TOP 1000 [SponsorFirmCode]
      ,[InManagerSponsorScorecard]
      ,[InSponsorAmountsScorecard]
  FROM [DoverSma].[dbo].[SponsorFirms]
  where InSponsorAmountsScorecard = 'y'

  SELECT TOP 1000 [SponsorFirmCode]
    FROM [DoverSma].[dbo].[SponsorFirms]
  where InSponsorAmountsScorecard = 'y'

