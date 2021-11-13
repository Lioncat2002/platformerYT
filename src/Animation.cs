using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace platformerYT.src
{
    public class Animation
    {
        Texture2D spritesheet;
        int frames;
        int rows=0;
        int c = 0;
        float timeSinceLastFrame = 0;
        public Animation(Texture2D spritesheet,float width=32,float height=32)
        {
            this.spritesheet = spritesheet;
            frames = (int)(spritesheet.Width / width);
            
        }

        public void Draw(SpriteBatch spriteBatch,Vector2 position,GameTime gameTime,float milisecondsperframes=500)
        {
            if (c < frames)
            {
                var rect = new Rectangle(32 * c, rows, 32, 32);
                spriteBatch.Draw(spritesheet, position, rect, Color.White);
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timeSinceLastFrame > milisecondsperframes)
                {
                    timeSinceLastFrame -= milisecondsperframes;
                    c++;
                    if (c == frames)
                    {
                        c = 0;
                    }
                }

            }
            
        }
    }
}
