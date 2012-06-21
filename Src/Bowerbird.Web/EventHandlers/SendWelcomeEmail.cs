﻿/* Bowerbird V1 

 Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using Bowerbird.Core.Events;
using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.DomainModels;
using Bowerbird.Core.Extensions;
using Bowerbird.Core.Services;
using Bowerbird.Core.Config;

namespace Bowerbird.Core.EventHandlers
{
    public class SendWelcomeEmail : IEventHandler<DomainModelCreatedEvent<User>>
    {

        #region Members

        private readonly IEmailService _emailService;
        private readonly IConfigService _configService;

        #endregion

        #region Constructors

        public SendWelcomeEmail(
            IEmailService emailService,
            IConfigService configService)
        {
            Check.RequireNotNull(emailService, "emailService");
            Check.RequireNotNull(configService, "configService");

            _emailService = emailService;
            _configService = configService;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Handle(DomainModelCreatedEvent<User> userCreatedEvent)
        {
            Check.RequireNotNull(userCreatedEvent, "userCreatedEvent");

            var message = Email
                .From(_configService.GetEmailAdminAccount(), "Bowerbird")
                .To(userCreatedEvent.DomainModel.Email)
                .Subject("Bowerbird account verification")
                .UsingTemplate("WelcomeEmail", new { userCreatedEvent.DomainModel.FirstName })
                .Message;

            _emailService.SendMailMessage(message);
        }

        #endregion      

    }
}