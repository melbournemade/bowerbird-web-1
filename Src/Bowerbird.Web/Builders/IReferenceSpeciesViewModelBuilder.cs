﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using Bowerbird.Web.ViewModels;

namespace Bowerbird.Web.Builders
{
    public interface IReferenceSpeciesViewModelBuilder : IBuilder
    {
        object BuildReferenceSpecies(IdInput idInput);

        object BuildReferenceSpeciesList(PagingInput pagingInput);

        object BuildGroupReferenceSpeciesList(PagingInput pagingInput);
    }
}