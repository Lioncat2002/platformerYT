using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace platformerYT.src
{
    public class Player:Entity
    {
        
        public Vector2 velocity;
        public Rectangle playerFallRect;
        
        public float playerSpeed = 1;
        public float fallSpeed=5;
        public float jumpSpeed = -14;
        public float startY;

        public bool isFalling = true;
        public bool isJumping;

        public Animation[] playerAnimation;
        public currentAnimation playerAnimationController;
        public Player( Texture2D idleSprite,Texture2D runSprite)
        {
            playerAnimation = new Animation[2];

            position = new Vector2(100);
            velocity = new Vector2();
            

            playerAnimation[0] = new Animation(idleSprite);
            playerAnimation[1] = new Animation(runSprite);
            hitbox = new Rectangle((int)position.X, (int)position.Y, 32, 25 );
          playerFallRect= new Rectangle((int)position.X+3, (int)position.Y+32, 32, (int)fallSpeed);
        }
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            
            playerAnimationController = currentAnimation.Idle;
            position = velocity;
            
            if (isFalling)
                velocity.Y += fallSpeed;
            
            startY = position.Y;
            Move(keyboard);
            Jump(keyboard);
            

            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
            playerFallRect.X= (int)position.X;
            playerFallRect.Y= (int)(velocity.Y+34);
        }
        private void Move(KeyboardState keyboard)
        {
            
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
        }
        private void Jump(KeyboardState keyboard)
        {
            if (isJumping)
            {
                velocity.Y += jumpSpeed;//Making it go up
                jumpSpeed += 1;//Some math (explained later)
                Move(keyboard);
                if (velocity.Y >= startY)
                //If it's farther than ground
                {
                    velocity.Y = startY;//Then set it on
                    isJumping = false;
                }
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Space)&& !isFalling)
                {
                    isJumping = true;
                    isFalling = false;
                    jumpSpeed = -14;//Give it upward thrust
                }
            }
            Console.WriteLine(position.Y);
        }
        public override void Draw(SpriteBatch spriteBatch,GameTime gameTime)
        {

            switch (playerAnimationController)
            {
                case currentAnimation.Idle:
                    playerAnimation[0].Draw(spriteBatch, position, gameTime, 500);
                    break;
                case currentAnimation.Run:
                    playerAnimation[1].Draw(spriteBatch, position, gameTime, 100);
                    break;
            }
          
        }
    }
}
