using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyRecommend.SpotifyService.Helpers
{
    public static class OffsetSelector
    {
        public static int GetRandomOffset()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 5);
            return randomNumber;
        }
    }
}