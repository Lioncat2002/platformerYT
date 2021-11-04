using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace platformerYT.src
{
    public abstract class Entity
    {
       
        public Vector2 postion;
       
        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch,GameTime gameTime);
    }
}
