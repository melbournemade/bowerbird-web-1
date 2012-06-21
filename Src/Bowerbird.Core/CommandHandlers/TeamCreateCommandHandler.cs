    /* Bowerbird V1 - Licensed under MIT 1.1 Public License

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
using Bowerbird.Core.Config;
using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.DomainModels;
using Raven.Client;
using System;
using Raven.Client.Linq;
using Bowerbird.Core.Factories;

namespace Bowerbird.Core.CommandHandlers
{
    public class TeamCreateCommandHandler : ICommandHandler<TeamCreateCommand>
    {
        #region Members

        private readonly IDocumentSession _documentSession;
        private readonly IAvatarFactory _avatarFactory;

        #endregion

        #region Constructors

        public TeamCreateCommandHandler(
            IDocumentSession documentSession,
            IAvatarFactory avatarFactory)
        {
            Check.RequireNotNull(documentSession, "documentSession");
            Check.RequireNotNull(avatarFactory, "avatarFactory");

            _documentSession = documentSession;
            _avatarFactory = avatarFactory;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        /// <summary>
        /// If Team has a parent OrganisationId: 
        ///     - Add Organisation to Team Ancestry
        ///     - Add Team to Organisation Descendants
        /// </summary>
        public void Handle(TeamCreateCommand command)
        {
            Check.RequireNotNull(command, "command");

            var parentGroup =
                !string.IsNullOrEmpty(command.OrganisationId)
                    ? (Group)_documentSession.Load<Organisation>(command.OrganisationId)
                    : (Group)_documentSession.Load<AppRoot>(Constants.AppRootId);

            var team = new Team(
                _documentSession.Load<User>(command.UserId), 
                command.Name, 
                command.Description, 
                command.Website,
                string.IsNullOrWhiteSpace(command.AvatarId) ? _avatarFactory.MakeDefaultAvatar(AvatarDefaultType.Team) : _documentSession.Load<MediaResource>(command.AvatarId),
                DateTime.UtcNow,
                parentGroup);

            _documentSession.Store(team);
            
            // If team is in an organisation, add team to organisation's Descendants
            if(!string.IsNullOrEmpty(command.OrganisationId))
            {
                parentGroup.AddDescendant(team);
                _documentSession.Store(parentGroup);
            }

            var user = _documentSession.Load<User>(command.UserId);
            var roles = _documentSession
                .Query<Role>()
                .Where(x => x.Id.In("roles/teamadministrator","roles/teammember"))
                .ToList();

            var teamAdministrator = new Member(
                user,
                user,
                team,
                roles);

            _documentSession.Store(teamAdministrator);
            
            var groupAssociation = new GroupAssociation(
                parentGroup,
                team,
                _documentSession.Load<User>(command.UserId),
                DateTime.UtcNow
                );

            _documentSession.Store(groupAssociation);
        }

        #endregion

    }
}