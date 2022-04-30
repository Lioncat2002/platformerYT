using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace platformerYT.src
{
    public class Coin
    {
        private Animation coinAnim;
        public Coin(Texture2D sprite)
        {
            coinAnim = new Animation(sprite,16,16);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            coinAnim.Draw(spriteBatch,position,gameTime);
        }
    }
}
