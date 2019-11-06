using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLive.Web.Models
{
    public class GameState : GameStateShortViewModel
    {
        /// <summary>
        /// Жизнь
        /// </summary>
        public IEnumerable<Point> Live
        { get; set; }

        /// <summary>
        /// Итерация жизни
        /// </summary>
        public Int32 IterationCount
        { get; set; }
    }
}