using System.ComponentModel.DataAnnotations;

namespace GameLive.Web.Config
{
    public class Size
    {
        [Range(10, int.MaxValue)]
        public int Width { get; set; }
        [Range(10, int.MaxValue)]
        public int Height { get; set; }
    }
}
