/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

namespace Bowerbird.Web.ViewModels.Members
{
    #region Namespaces

    using System.Collections.Generic;

    using Core.DomainModels;

    #endregion

    public class ProjectIndex : IViewModel
    {
        #region Members

        #endregion

        #region Constructors

        #endregion

        #region Properties

        public Project Project { get; set; }

        public List<Observation> Observations { get; set; }
         
        #endregion

        #region Methods

        #endregion
    }
}