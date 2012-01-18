﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

namespace Bowerbird.Core.Test.DomainModels
{
    #region Namespaces

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Bowerbird.Core.DomainModels;
    using Bowerbird.Test.Utils;
    using Bowerbird.Core.DesignByContract;
    using Bowerbird.Core.Extensions;

    #endregion
    
    public class TeamTest
    {
        #region Test Infrastructure

        const string additionalString = "_";

        [SetUp]
        public void TestInitialize() { }

        [TearDown]
        public void TestCleanup() { }

        #endregion

        #region Test Helpers

        private static Team TestTeam()
        {
            return new Team(
                FakeObjects.TestUser(),
                FakeValues.Name,
                FakeValues.Description,
                FakeValues.Website
                );
        }

        #endregion

        #region Constructor tests

        [Test]
        [Category(TestCategory.Unit)]
        public void Team_Constructor_Populates_Property_Values()
        {
            var testTeam = new Team(FakeObjects.TestUser(), FakeValues.Name, FakeValues.Description, FakeValues.Website);

            Assert.AreEqual(testTeam.Name, FakeValues.Name);
            Assert.AreEqual(testTeam.Description, FakeValues.Description);
            Assert.AreEqual(testTeam.Website, FakeValues.Website);
        }

        #endregion

        #region Method tests

        [Test]
        [Category(TestCategory.Unit)]
        public void Team_UpdateDetails_Populates_Properties_With_Values()
        {
            var testTeam = new Team(FakeObjects.TestUser(), FakeValues.Name, FakeValues.Description, FakeValues.Website);

            testTeam.UpdateDetails(
                FakeObjects.TestUser(),
                FakeValues.Name.AppendWith(additionalString),
                FakeValues.Description.AppendWith(additionalString),
                FakeValues.Website.AppendWith(additionalString));

            Assert.AreEqual(testTeam.Name, FakeValues.Name.AppendWith(additionalString));
            Assert.AreEqual(testTeam.Description, FakeValues.Description.AppendWith(additionalString));
            Assert.AreEqual(testTeam.Website, FakeValues.Website.AppendWith(additionalString));
        }

        #endregion
    }
}