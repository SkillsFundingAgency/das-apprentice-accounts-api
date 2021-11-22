using AutoFixture;
using AutoFixture.Kernel;
using System.Net.Mail;
using System.Reflection;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    internal class EmailPropertyCustomisation : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is PropertyInfo pip)) return new NoSpecimen();
            if (pip.Name != "Email") return new NoSpecimen();
            return context.Create<MailAddress>().ToString();
        }
    }
}