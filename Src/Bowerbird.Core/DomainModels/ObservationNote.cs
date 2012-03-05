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
using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.Events;
using Bowerbird.Core.DomainModels.DenormalisedReferences;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Bowerbird.Core.DomainModels
{
    public class ObservationNote : Contribution
    {
        #region Members

        #endregion

        #region Constructors

        protected ObservationNote() 
            : base() 
        {
            InitMembers();
        }

        public ObservationNote(
            User createdByUser,
            Observation observation,
            string commonName, 
            string scientificName, 
            string taxonomy,
            string tags,
            IDictionary<string, string> descriptions,
            IDictionary<string, string> references,
            string notes,
            DateTime createdOn)
            : base(
            createdByUser,
            createdOn)
        {
            Check.RequireNotNull(observation, "observation");
            Check.RequireNotNull(descriptions, "descriptions");
            Check.RequireNotNull(references, "references");

            Observation = observation;

            SetDetails(
                commonName,
                scientificName,
                taxonomy,
                tags,
                descriptions,
                references,
                notes);

            var eventMessage = string.Format(
                ActivityMessage.CreatedAnObservationNote,
                createdByUser.GetName(),
                observation.Title
                );

            EventProcessor.Raise(new DomainModelCreatedEvent<ObservationNote>(this, createdByUser, eventMessage));
        }

        #endregion

        #region Properties

        public DenormalisedObservationReference Observation { get; private set; }

        public string ScientificName { get; private set; }

        public string CommonName { get; private set; }

        public string Taxonomy { get; private set; }

        public string Tags { get; private set; }

        public Dictionary<string, string> Descriptions { get; private set; }

        public Dictionary<string, string> References { get; private set; }

        public string Notes { get; private set; }

        #endregion

        #region Methods

        public override string ContributionType()
        {
            return "ObservationNote";
        }

        public override string ContributionTitle()
        {
            return Observation.Title;
        }

        private void InitMembers()
        {
            Descriptions = new Dictionary<string, string>();

            References = new Dictionary<string, string>();
        }

        protected void SetDetails(string commonName, string scientificName, string taxonomy, string tags, IDictionary<string, string> descriptions, IDictionary<string, string> references, string notes)
        {
            Check.RequireNotNull(descriptions, "descriptions");
            Check.RequireNotNull(references, "references");

            CommonName = commonName;
            ScientificName = scientificName;
            Taxonomy = taxonomy;
            Tags = tags;
            Notes = notes;
            Descriptions = descriptions.ToDictionary(x => x.Key, x => x.Value);
            References = references.ToDictionary(x => x.Key, x => x.Value);
        }

        public ObservationNote UpdateDetails(User updatedByUser, string commonName, string scientificName, string taxonomy, string tags, IDictionary<string, string> descriptions, IDictionary<string, string> references, string notes)
        {
            Check.RequireNotNull(updatedByUser, "updatedByUser");
            Check.RequireNotNull(descriptions, "descriptions");
            Check.RequireNotNull(references, "references");

            SetDetails(
                commonName,
                scientificName,
                taxonomy,
                tags,
                descriptions,
                references,
                notes);

            var eventMessage = string.Format(
                ActivityMessage.UpdatedAnObservationNote,
                updatedByUser.GetName(),
                ContributionTitle()
                );

            EventProcessor.Raise(new DomainModelUpdatedEvent<ObservationNote>(this, updatedByUser, eventMessage));

            return this;
        }

        #endregion
    }
}