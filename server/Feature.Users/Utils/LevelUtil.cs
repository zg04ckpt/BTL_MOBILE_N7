using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feature.Users.Utils
{
    public class LevelUtil
    {
        private static Func<int,int> _calcNextExpScore = currentLevel =>
        {
            return 100 * currentLevel;
        };

        public static (int NewLevel, int NewScore) plusExpScore(int currentLevel, int currentScore, int earnExp)
        {
            currentScore += earnExp;

            while (true)
            {
                int limit = _calcNextExpScore(currentLevel);

                if (currentScore < limit)
                    break;

                currentScore -= limit;
                currentLevel++;
            }

            return (currentLevel, currentScore);
        }

        public static int getLimit(int currentLevel) => _calcNextExpScore(currentLevel);
    }
}