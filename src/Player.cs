using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platformerYT.src
{
    public class Player:Entity
    {
        
        public Vector2 velocity;
        public float playerSpeed = 2;
        public Animation[] playerAnimation;
        public currentAnimation playerAnimationController;
        public Player( Texture2D idleSprite,Texture2D runSprite)
        {
            playerAnimation = new Animation[2];
            velocity = new Vector2();
            playerAnimation[0] = new Animation(idleSprite);
            playerAnimation[1] = new Animation(runSprite);
           
          
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();

            playerAnimationController = currentAnimation.Idle;

            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= playerSpeed;
                playerAnimationController = currentAnimation.Run;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += playerSpeed;
                playerAnimationController = currentAnimation.Run;
            }

            postion = velocity;
        }
        public override void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {

            switch (playerAnimationController)
            {
                case currentAnimation.Idle:
                    playerAnimation[0].Draw(spriteBatch, postion, gameTime, 100);
                    break;
                case currentAnimation.Run:
                    playerAnimation[1].Draw(spriteBatch, postion, gameTime, 100);
                    break;
            }
          
            //spriteBatch.Draw(spritesheet, postion, Color.White);
        }
    }
}
