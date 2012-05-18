﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using Bowerbird.Core.Config;

namespace Bowerbird.Web.ViewModels
{
    public class PagingInput
    {
        #region Fields

        #endregion

        #region Constructors

        public PagingInput()
        {
            InitMembers();
        }

        #endregion

        #region Properties

        public string Id { get; set; }

        public int Page { get; set; }
        
        public int PageSize { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public string SearchQuery { get; set; }

        #endregion

        #region Methods

        public void InitMembers()
        {
            Page = Default.PageStart;
            PageSize = Default.PageSize;
        }

        #endregion
    }
}