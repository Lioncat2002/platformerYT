using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace platformerYT.src
{
    public class Camera
    {

        public Matrix Transform;

        public Matrix Follow(Rectangle target)
        {
            target.X=MathHelper.Clamp(target.X, (int)Game1.screenWidth/2-270, (int)Game1.screenWidth/2+240);
            target.Y=(int)Game1.screenHeight/2;

            Vector3 translation = new Vector3(-target.X-target.Width/2,
                                        -target.Y-target.Height/2,0);

          


            Vector3 offset = new Vector3(Game1.screenWidth/4, Game1.screenHeight/2, 0);
            
            Transform=Matrix.CreateTranslation(translation)*Matrix.CreateTranslation(offset);

            return Transform;
        }
    }
}
