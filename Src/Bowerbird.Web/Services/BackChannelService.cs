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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bowerbird.Core.CommandHandlers;
using Bowerbird.Core.Commands;
using Bowerbird.Core.DesignByContract;
using Bowerbird.Core.DomainModels;
using Bowerbird.Core.Extensions;
using Bowerbird.Core.Indexes;
using Bowerbird.Core.Services;
using Bowerbird.Web.Config;
using Raven.Client;
using Raven.Client.Linq;
using Bowerbird.Core.Config;
using Bowerbird.Web.Factories;
using SignalR;
using Bowerbird.Web.Hubs;
using SignalR.Hubs;
using System.Dynamic;

namespace Bowerbird.Web.Services
{
    public class BackChannelService : IBackChannelService
    {
        #region Fields

        private readonly IDocumentSession _documentSession;
        private readonly IConnectionManager _connectionManager;
        private static readonly object _chatHubLock = new object();

        #endregion

        #region Constructors

        public BackChannelService(
            IDocumentSession documentSession,
            IConnectionManager connectionManager
           )
        {
            Check.RequireNotNull(documentSession, "documentSession");
            Check.RequireNotNull(connectionManager, "connectionManager");

            _documentSession = documentSession;
            _connectionManager = connectionManager;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region User Channel

        public void AddUserToUserChannel(string userId, string connectionId)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Groups.Add(connectionId, "user-" + userId);
        }

        public void SendJoinedGroupToUserChannel(string userId, object group)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Clients["user-" + userId].joinedGroup(group);
        }

        public void SendOnlineUsersToUserChannel(string userId, object onlineUsers)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Clients["user-" + userId].setupOnlineUsers(onlineUsers);
        }

        public void NotifyChatJoinedToUserChannel(string userId, object chatDetails)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Clients["user-" + userId].chatJoined(chatDetails);
        }

        public void NotifyChatExitedToUserChannel(string userId, string chatId)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Clients["user-" + userId].chatExited(chatId);
        }

        #endregion

        #region Group Channel

        public void AddUserToGroupChannel(string groupId, string connectionId)
        {
            if (ChannelServiceOff()) return;

            GetHub<GroupHub>().Groups.Add(connectionId, "group-" + groupId);
        }

        public void SendActivityToGroupChannel(dynamic activity)
        {
            if (ChannelServiceOff()) return;

            var groupHub = GetHub<GroupHub>();

            foreach (var group in activity.Groups)
            {
                groupHub.Clients["group-" + group.Id].newActivity(activity);
            }
        }

        #endregion

        #region Online Users Channel

        public void AddUserToOnlineUsersChannel(string connectionId)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Groups.Add(connectionId, "online-users");
        }

        public void SendUserStatusUpdateToOnlineUsersChannel(object userStatus)
        {
            if (ChannelServiceOff()) return;

            GetHub<UserHub>().Clients["online-users"].userStatusUpdate(userStatus);
        }

        #endregion

        #region Chat Channel

        public void AddUserToChatChannel(string chatId, string connectionId)
        {
            if (ChannelServiceOff()) return;

            lock (_chatHubLock)
            {
                GetHub<ChatHub>().Groups.Add(connectionId, "chat-" + chatId);
            }
        }

        public void RemoveUserFromChatChannel(string chatId, string connectionId)
        {
            if (ChannelServiceOff()) return;

            lock (_chatHubLock)
            {
                GetHub<ChatHub>().Groups.Remove(connectionId, "chat-" + chatId);
            }
        }

        public void UserJoinedChatToChatChannel(string chatId, object chatMessageDetails)
        {
            if (ChannelServiceOff()) return;

            lock (_chatHubLock)
            {
                GetHub<ChatHub>().Clients["chat-" + chatId].userJoinedChat(chatMessageDetails);
            }
        }

        public void UserExitedChatToChatChannel(string chatId, object chatMessageDetails)
        {
            if (ChannelServiceOff()) return;

            lock (_chatHubLock)
            {
                GetHub<ChatHub>().Clients["chat-" + chatId].userExitedChat(chatMessageDetails);
            }
        }

        public void NewChatMessageToChatChannel(string chatId, object chatMessageDetails)
        {
            if (ChannelServiceOff()) return;

            lock (_chatHubLock)
            {
                GetHub<ChatHub>().Clients["chat-" + chatId].newChatMessage(chatMessageDetails);
            }
        }

        #endregion

        private IHubContext GetHub<T>() where T : IHub
        {
            return _connectionManager.GetHubContext<T>();
        }

        private bool ChannelServiceOff()
        {
            return !_documentSession.Load<AppRoot>(Constants.AppRootId).BackChannelServiceStatus;
        }

        #endregion
    }
}