using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace platformerYT.src
{
    public class Enemy:Entity
    {
        private Animation enemyAnim;
        private Rectangle pathway;
        private float speed = 2;
        private bool isFacingRight = true;
        public Enemy(Texture2D enemySpriteSheet, Rectangle pathway, float speed=2)
        {
            enemyAnim=new Animation(enemySpriteSheet);
            this.pathway=pathway;

            position=new Vector2(pathway.X, pathway.Y);
            hitbox=new Rectangle(pathway.X, pathway.Y, 16, 16);
            this.speed=speed;
        }
        public override void Update()
        {
            if(!pathway.Contains(hitbox))
            {
                speed=-speed;
                isFacingRight=!isFacingRight;
            }
            position.X+=speed;

            hitbox.X=(int)position.X;
            hitbox.Y=(int)position.Y;

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (isFacingRight)
                enemyAnim.Draw(spriteBatch, position, gameTime, 150);
            else
                enemyAnim.Draw(spriteBatch, position, gameTime, 150, SpriteEffects.FlipHorizontally);
        }
    }
}
