create sequence [dbo].[ApprenticeshipIdNumbers]
		as bigint
		start with 10000
		increment by 1
		no maxvalue
		no cycle
		cache 10
