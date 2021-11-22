using System;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeCommitments.Data.Models;

namespace SFA.DAS.ApprenticeCommitments.UnitTests.RenewingRevisionTests
{
    public class CourseComparison
    {
        Fixture _f = new Fixture();
        private CourseDetails _course;

        [SetUp]
        public void Arrange()
        {
            _course = _f.Create<CourseDetails>();
        }

        [Test]
        public void When_all_fields_are_the_same_Then_new_course_is_equivalent()
        {
            var newCourse = _course.Clone();

            _course.IsEquivalent(newCourse).Should().BeTrue();
        }

        [Test]
        public void When_name_is_different_Then_new_course_is_not_equivalent()
        {
            var newCourse = _course.Clone();
            newCourse.SetProperty(x => x.Name, _f.Create<string>());

            _course.IsEquivalent(newCourse).Should().BeFalse();
        }

        [Test]
        public void When_level_is_different_Then_new_course_is_not_equivalent()
        {
            var newCourse = _course.Clone();
            newCourse.SetProperty(x => x.Level, _f.Create<int>());

            _course.IsEquivalent(newCourse).Should().BeFalse();
        }

        [Test]
        public void When_option_is_different_Then_new_course_is_not_equivalent()
        {
            var newCourse = _course.Clone();
            newCourse.SetProperty(x => x.Option, _f.Create<string>());

            _course.IsEquivalent(newCourse).Should().BeFalse();
        }

        [Test]
        public void When_start_date_is_different_Then_new_course_is_not_equivalent()
        {
            var newCourse = _course.Clone();
            newCourse.SetProperty(x => x.PlannedStartDate, _f.Create<DateTime>());

            _course.IsEquivalent(newCourse).Should().BeFalse();
        }

        [Test]
        public void When_end_date_is_different_Then_new_course_is_not_equivalent()
        {
            var newCourse = _course.Clone();
            newCourse.SetProperty(x => x.PlannedEndDate, _f.Create<DateTime>());

            _course.IsEquivalent(newCourse).Should().BeFalse();
        }
    }
}