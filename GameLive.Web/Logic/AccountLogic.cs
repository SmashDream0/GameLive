using GameLive.Web.Hubs;
using GameLive.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GameLive.Web.Controllers;

namespace GameLive.Web.Logic
{
    public class AccountLogic
    {
        public const String _solt = "_ulala";

        /// <summary>
        /// Генерировать ключ пользователя
        /// </summary>
        /// <returns></returns>
        public String GenerateCode()
        {
            var guid = Guid.NewGuid().ToString();

            return guid + _solt;
        }

        /// <summary>
        /// Валидировать ключ пользователя
        /// </summary>
        /// <param name="id">Ключ пользователя</param>
        /// <returns></returns>
        public Boolean Validate(String id)
        {
            var result = id.EndsWith(_solt);

            return result;
        }
    }
}