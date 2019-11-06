using GameLive.Web.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLive.Web.Controllers
{
    public class SettingsController : Controller
    {
        public SettingsController(IOptions<ConfigSettings> config)
        {
            _configSettings = config.Value;
        }

        private readonly ConfigSettings _configSettings;

        /// <summary>
        /// Поулчить настройки
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSettings()
        {
            return Json(_configSettings);
        }
    }
}