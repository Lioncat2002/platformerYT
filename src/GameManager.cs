using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace platformerYT.src
{
    public class GameManager
    {
        private Rectangle endRectangle;
        public GameManager(Rectangle endRectangle)
        {
            this.endRectangle = endRectangle;   
        }

        public bool hasGameEnded(Rectangle playerHitbox)
        {
            return endRectangle.Intersects(playerHitbox);
        }
    }
}
