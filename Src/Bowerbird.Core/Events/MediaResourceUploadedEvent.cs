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

using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.DomainModels;

namespace Bowerbird.Core.Events
{
    public class MediaResourceUploadedEvent : DomainEventBase
    {
        #region Members

        #endregion

        #region Constructors

        public MediaResourceUploadedEvent(
            User createdByUser,
            DomainModel sender
            ):base(createdByUser, sender)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion
    }
}