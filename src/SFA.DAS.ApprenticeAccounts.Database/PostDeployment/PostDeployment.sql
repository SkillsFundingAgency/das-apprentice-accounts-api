/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/


/* Reporter Role Access */

IF DATABASE_PRINCIPAL_ID('Reporter') IS NULL
BEGIN
    CREATE ROLE [Reporter]
END

GRANT SELECT ON [DashboardReporting].ApprenticeDashboardView TO Reporter

GRANT SELECT ON [DashboardReporting].ApprenticeshipDashboardView TO Reporter

GRANT SELECT ON [DashboardReporting].CommitmentstatementDashboardView TO Reporter

GRANT SELECT ON [DashboardReporting].RegistrationDashboardView TO Reporter


:r .\MoveLastViewedToRevision_CS_1006.sql