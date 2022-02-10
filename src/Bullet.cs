using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace platformerYT.src
{
    public class Bullet
    {
        private Texture2D bulletTexture;
        private float speed;
        public Rectangle hitbox;

        public Bullet(Texture2D bulletTexture,float speed,Rectangle hitbox)
        {
            this.bulletTexture = bulletTexture;
            this.speed = speed;
            this.hitbox = hitbox;
        }

        public void Update()
        {
            hitbox.X+=(int)speed;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture,hitbox,Color.White);
        }
    }
}
