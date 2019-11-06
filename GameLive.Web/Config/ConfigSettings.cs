using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GameLive.Web.Config
{
    public class ConfigSettings
    {
        public ConfigSettings()
        {
            WorldSize = new Size() { Height = 30, Width = 30 };
            WorldUpdateIntervalMs = 500;
            WorldRepositoryTimeoutSec = 10 * 60;
        }

        /// <summary>
        /// Размеры мира
        /// </summary>
        public Size WorldSize
        { get; set; }

        /// <summary>
        /// Интервал обновления мира
        /// </summary>
        [Range(10, 1000)]
        public int WorldUpdateIntervalMs
        { get; set; }

        /// <summary>
        /// Время жизни мира в репозитории
        /// </summary>
        [Range(60, int.MaxValue)]
        public int WorldRepositoryTimeoutSec
        { get; set; }

        public IEnumerable<string> Validate()
        {
            var validations = new List<ValidationResult>();
            var resultMessages = new List<String>();

            {
                var contextMain = new ValidationContext(this, serviceProvider: null, items: null);
                Validator.TryValidateObject(this, contextMain, validations, true);
            }
            {
                var contextSize = new ValidationContext(this.WorldSize, serviceProvider: null, items: null);
                Validator.TryValidateObject(this.WorldSize, contextSize, validations, true);
            }

            foreach (var validationResult in validations)
            { resultMessages.Add(validationResult.ErrorMessage); }

            return resultMessages;
        }
    }
}