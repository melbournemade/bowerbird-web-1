﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.Extensions;

namespace Bowerbird.Core.DomainModels
{
    public abstract class CommentBase
    {
        #region Members

        [Raven.Imports.Newtonsoft.Json.JsonIgnore]
        private List<CommentNew> _comments;

        #endregion

        #region Constructors

        protected CommentBase()
        {
            InitMembers();
        }

        protected CommentBase(
            CommentBase parentComment)
            : base()
        {
            InitMembers();

            SequentialId = parentComment.GetNextChildCommentSequentialId();
            Id = parentComment.GetNextChildCommentId();
        }

        #endregion

        #region Properties

        public string Id { get; protected set; }

        public int SequentialId { get; protected set; }

        public IEnumerable<CommentNew> Comments
        {
            get { return _comments; }
            private set { _comments = new List<CommentNew>(value); }
        }

        #endregion

        #region Methods

        private void InitMembers()
        {
            Comments = new List<CommentNew>();
        }

        public CommentNew AddComment(
            string message,
            User createdByUser,
            DateTime createdDateTime,
            string parentCommentId = "")
        {
            if (parentCommentId == Id)
            {
                var comment = new CommentNew(createdByUser, createdDateTime, message, this);
                _comments.Add(comment);

                return comment;
            }

            foreach (var childComment in _comments)
            {
                var comment = childComment.AddComment(message, createdByUser, createdDateTime, parentCommentId);

                if (comment != null)
                {
                    return comment;
                }
            }

            return null;
        }

        public void RemoveComment(string id)
        {
            if (_comments.RemoveAll(x => x.Id == id) == 0)
            {
                foreach (var childComment in _comments)
                {
                    childComment.RemoveComment(id);
                }                
            }
        }

        public int GetNextChildCommentSequentialId()
        {
            var currentLastSequentialId = 0;

            if (_comments.Any())
            {
                currentLastSequentialId = _comments
                    .OrderByDescending(x => x.SequentialId)
                    .First()
                    .SequentialId;
            }

            return currentLastSequentialId + 1;
        }

        public string GetNextChildCommentId()
        {
            return string.Format("{0}{1}{2}",
                Id,
                string.IsNullOrWhiteSpace(Id) ? string.Empty : ".",
                GetNextChildCommentSequentialId());
        }

        #endregion
    }
}