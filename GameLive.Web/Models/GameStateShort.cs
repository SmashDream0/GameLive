using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLive.Web.Models
{
    public class GameStateShortViewModel
    {
        /// <summary>
        /// Произведена ли остановка игры
        /// </summary>
        public Boolean IsStoped
        { get; set; }

        /// <summary>
        /// Ошибка
        /// </summary>
        public EGameStateError GameStateError { get; set; }
    }

    public enum EGameStateError
    {
        /// <summary>
        /// Все хорошо
        /// </summary>
        None,

        /// <summary>
        /// Игра не существует
        /// </summary>
        GameNotExist,

        /// <summary>
        /// Нет аутентификации
        /// </summary>
        NoAuthentificated,
    }
}