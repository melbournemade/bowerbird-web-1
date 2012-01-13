/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

namespace Bowerbird.Core.Test.CommandHandlers
{
    #region Namespaces

    using System;
    using System.Collections.Generic;

    using NUnit.Framework;
    using Moq;

    using Bowerbird.Core.Commands;
    using Bowerbird.Core.CommandHandlers;
    using Bowerbird.Core.DesignByContract;
    using Bowerbird.Core.DomainModels;
    using Bowerbird.Core.Repositories;
    using Bowerbird.Test.Utils;

    #endregion

    [TestFixture]
    public class PostCommentUpdateCommandHandlerTest
    {
        #region Test Infrastructure

        //private Mock<IDefaultRepository<User>> _mockUserRepository;
        //private Mock<User> _mockUser;
        private PostCommentUpdateCommandHandler _commandHandler;

        [SetUp]
        public void TestInitialize()
        {
            //_mockUserRepository = new Mock<IDefaultRepository<User>>();
            //_mockUser = new Mock<User>();
            _commandHandler = new PostCommentUpdateCommandHandler();
        }

        [TearDown]
        public void TestCleanup()
        {
        }

        #endregion

        #region Test Helpers

        private PostCommentUpdateCommand TestPostCommentUpdateCommand()
        {
            return new PostCommentUpdateCommand()
                       {

                       };
        }

        #endregion

        #region Constructor tests

        [Test]
        [Category(TestCategory.Unit)]
        public void PostCommentUpdateCommandHandler_Constructor_Passing_Null_Something_Throws_DesignByContractException()
        {
           // Assert.IsTrue(BowerbirdThrows.Exception<DesignByContractException>(() => new PostCommentUpdateCommandHandler(null)));
        }

        #endregion

        #region Property tests

        #endregion

        #region Method tests

        [Test]
        [Category(TestCategory.Unit)]
        public void PostCommentUpdateCommandHandler_Handle_Passing_Null_PostCommentUpdate_Throws_DesignByContractException()
        {
            Assert.IsTrue(BowerbirdThrows.Exception<DesignByContractException>(() => _commandHandler.Handle(null)));
        }

        #endregion
    }
}