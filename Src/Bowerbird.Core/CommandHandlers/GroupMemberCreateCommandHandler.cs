﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using System.Linq;
using Bowerbird.Core.Commands;
using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.DomainModels;
using Bowerbird.Core.DomainModels.Members;
using Raven.Client;
using Raven.Client.Linq;

namespace Bowerbird.Core.CommandHandlers
{
    public class GroupMemberCreateCommandHandler : ICommandHandler<GroupMemberCreateCommand>
    {
        #region Members

        private readonly IDocumentSession _documentSession;

        #endregion

        #region Constructors

        public GroupMemberCreateCommandHandler(
            IDocumentSession documentSession)
        {
            Check.RequireNotNull(documentSession, "documentSession");

            _documentSession = documentSession;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Handle(GroupMemberCreateCommand groupMemberCreateCommand)
        {
            Check.RequireNotNull(groupMemberCreateCommand, "groupMemberCreateCommand");

            var groupMember = new GroupMember(
                _documentSession.Load<User>(groupMemberCreateCommand.CreatedByUserId),
                _documentSession.Load<Group>(groupMemberCreateCommand.GroupId),
                _documentSession.Load<User>(groupMemberCreateCommand.UserId),
                _documentSession.Query<Role>()
                    .Where(x => x.Id.In(groupMemberCreateCommand.Roles))
                    .ToList()
                );

            _documentSession.Store(groupMember);
        }

        #endregion
		
    }
}