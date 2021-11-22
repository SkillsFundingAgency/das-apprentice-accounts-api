/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if exists(
    select RegistrationId
    from Registration
    where UserIdentityId in (select Id from Apprentice)
        and ApprenticeId in (select Id from Apprentice)
        and UserIdentityId <> ApprenticeId
)
begin
    raiserror (N'There are existing rows whose valid ApprenticeId would be overwritten.', 16, 127) WITH NOWAIT
end

if exists(
    select RegistrationId from Registration
    where UserIdentityId <> ApprenticeId
        and UserIdentityId in (select Id from Apprentice)
)
begin

    print 'Reconciling Apprentice IDs'

    update Registration 
    set ApprenticeId = UserIdentityId
    where UserIdentityId <> ApprenticeId
    and UserIdentityId in (select Id from Apprentice)

end

if exists(
    select RegistrationId from Registration
    where RegistrationId = ApprenticeId and 
        UserIdentityId is null and 
        ApprenticeId not in (select id from Apprentice)
)
begin

    print 'Removing unmatched Apprentice IDs'

    update Registration
    set ApprenticeId = null
    where
        RegistrationId = ApprenticeId and 
        UserIdentityId is null and 
        ApprenticeId not in (select id from Apprentice)

    print concat('Removed ', @@ROWCOUNT, ' Apprentice IDs')
end

