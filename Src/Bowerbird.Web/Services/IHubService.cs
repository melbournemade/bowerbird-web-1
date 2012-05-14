﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

Developers:
* Frank Radocaj : frank@radocaj.com
* Hamish Crittenden : hamish.crittenden@gmail.com
Project Manager:
* Ken Walker : kwalker@museum.vic.gov.au
Funded by:
* Atlas of Living Australia
*/

using System.Collections;
using System.Collections.Generic;
using Bowerbird.Core.DomainModels;
using Bowerbird.Core.Indexes;

namespace Bowerbird.Web.Services
{
    public interface IHubService
    {
        dynamic GetUserProfile(string userId);

        object GetUserAvatar(User user);

        IEnumerable<All_Chats.Results> GetClientsForChat(string chatId);

        void UpdateUserOnline(string clientId, string userId);

        void PersistChatMessage(string chatId, string userId, string targetUserId, string message);

        void UpdateChatUserStatus(string chatId, string clientId, string userId, int status);

        string DisconnectClient(string clientId);

        IEnumerable GetConnectedUserClientIds();

        string GetClientsUserId(string clientId);

        IEnumerable GetConnectedClientIdsForAUser(string userId);

        string GetGroupName(string chatId);

        IEnumerable GetChatMessages(string chatId);
    }
}