/* Bowerbird V1 

 Licensed under MIT 1.1 Public License

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
using Bowerbird.Core.Indexes;
using Raven.Client;
using Raven.Client.Linq;

namespace Bowerbird.Core.CommandHandlers
{
    public class CommentCreateCommandHandler : ICommandHandler<CommentCreateCommand>
    {
        #region Members

        private readonly IDocumentSession _documentSession;

        #endregion

        #region Constructors

        public CommentCreateCommandHandler(
            IDocumentSession documentSession)
        {
            Check.RequireNotNull(documentSession, "documentSession");

            _documentSession = documentSession;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Handle(CommentCreateCommand command)
        {
            Check.RequireNotNull(command, "command");

            var contribution = _documentSession.Query<Contribution, All_Contributions>()
                .Where(x => x.Id == command.ContributionId)
                .FirstOrDefault();

            contribution.AddComment(
                command.Comment,
                _documentSession.Load<User>(command.UserId),
                command.CommentedOn
                );

            _documentSession.Store(contribution);

            _documentSession.SaveChanges();
        }

        #endregion

    }
}