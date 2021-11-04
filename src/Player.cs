using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platformerYT.src
{
    public class Player:Entity
    {
        public currentAnimation anim;
        public Vector2 velocity;
        public float playerSpeed = 2;
        public Animation playerAnimation;
        public Player(Texture2D runSprite)
        {
     
            velocity = new Vector2();
            playerAnimation = new Animation(runSprite);
           
            anim = currentAnimation.Idle;
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            
            

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
             
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
               
            }

            postion = velocity;
        }
        public override void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            

           playerAnimation.Draw(spriteBatch, postion,gameTime,100);
            //spriteBatch.Draw(spritesheet, postion, Color.White);
        }
    }
}
