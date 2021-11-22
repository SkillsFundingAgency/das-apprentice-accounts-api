using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Data.Models;
using System;
using System.Linq;
using SFA.DAS.ApprenticeCommitments.Data.FuzzyMatching;

namespace SFA.DAS.ApprenticeCommitments.Api.AcceptanceTests.WorkflowTests
{
    public class FuzzyMatchingTests
    {
        private int _similarityThreshold = 49;
        private FuzzyMatcher _sut;

        [SetUp]
        public void Arrange()
        {
            _sut = new FuzzyMatcher(_similarityThreshold);
        }

        [TestCase("Ahmed", "Ahmed")]
        [TestCase("Ahmed", "Ahmad")]
        [TestCase("Ahmed", "Ahmet")]
        
        [TestCase("Smith", "Smith")]
        [TestCase("Smith", "Smyth")]
        [TestCase("Smith", "Smithe")]
        [TestCase("Smith", "Smythe")]

        [TestCase("Tayler", "Tayler")]
        [TestCase("Tayler", "Tayla")]
        [TestCase("Tayler", "Tailor")]
        [TestCase("Tayler", "Taylour")]
        [TestCase("Tayler", "Tailor")]
        
        [TestCase("Patel", "Patel")]
        [TestCase("Patel", "Patell")]
        [TestCase("Patel", "Putel")]
        [TestCase("Patel", "Putell")]
        [TestCase("Patel", "Patill")]
        
        [TestCase("Taylor-Jones", "Taylor")]
        [TestCase("Taylor-Jones", "Jones")]
        [TestCase("Taylor-Jones", "Taylor Jones")]
        [TestCase("Taylor-Jones", "Taylor-Jones")]
        
        [TestCase("Edward-Cooper", "Edward")]
        [TestCase("Edward-Cooper", "Cooper")]
        [TestCase("Edward-Cooper", "Edward Cooper")]
        [TestCase("Edward-Cooper", "Edward-Cooper")]

        [TestCase("Humpherys Smith", "Humpherys")]
        [TestCase("Humpherys Smith", "Smith")]
        [TestCase("Humpherys Smith", "Humpherys-Smith")]
        [TestCase("Humpherys Smith", "Humpherys Smith")]
        
        [TestCase("Al’fredo", "Al’fredo")]
        [TestCase("Al’fredo", "Fredo")]
        [TestCase("Al’fredo", "Alfredo")]

        [TestCase("Tayla-Smith", "Tailor")]
        [TestCase("Tayla-Smith", "Smith")]

        [TestCase("Taylor", "taylor")]
        [TestCase("Taylor", "taYlor")]
        [TestCase("taylor", "TAYLOR")]
        [TestCase("Taylor", "tayla")]

        [TestCase("Thereph", "Theirf")]

        public void PositiveSpellingTestCases(string string1, string string2)
        {
            Assert.IsTrue(_sut.IsSimilar(string1, string2));
        }

        [TestCase("Harry", "Sally")]
        [TestCase("Edwards", "Eddyshaw")]

        public void NegativeSpellingTestCases(string string1, string string2)
        {
            Assert.IsFalse(_sut.IsSimilar(string1, string2));
        }
    }
}
