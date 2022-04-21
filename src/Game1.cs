using Apos.Gui;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TiledSharp;
namespace platformerYT.src
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D renderTarget;

        public static float screenWidth;
        public static float screenHeight;

        #region UI
        IMGUI _ui;
        #endregion


        #region Managers
        private GameManager _gameManager;
        //private bool isGameOver = false;
        #endregion


        #region Tilemaps
        private TmxMap map;
        private TilemapManager tilemapManager;
        private Texture2D tileset;
        private List<Rectangle> collisionRects;
        private Rectangle startRect;
        private Rectangle endRect;
        #endregion

        #region Player 
        private Player player;
        private List<Bullet> bullets;
        private Texture2D bulletTexture;
        private int time_between_bullets;
        private int points=0;
        private int player_health = 10;
        private int time_between_hurt = 20;
        private int hit_counter = 0;
        #endregion

        #region Enemy
        private Enemy Martian;
        private List<Enemy> enemyList;
        private List<Rectangle> enemyPathway;
        #endregion

        #region Camera
        private Camera camera;
        private Matrix transformMatrix;
        #endregion
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 850;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();
            screenHeight = _graphics.PreferredBackBufferHeight; 
            screenWidth = _graphics.PreferredBackBufferWidth;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            #region UI
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dogicapixel.ttf"));
            GuiHelper.Setup(this, fontSystem);
            _ui=new IMGUI();
            #endregion

            #region Tilemap
            map = new TmxMap("Content\\level1.tmx");
            tileset = Content.Load<Texture2D>("Cave Tileset\\"+map.Tilesets[0].Name.ToString());
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            int tilesetTileWidth = tileset.Width / tileWidth;

            tilemapManager = new TilemapManager(map,tileset,tilesetTileWidth,tileWidth,tileHeight,GraphicsDevice,_spriteBatch);
            #endregion

            #region Collision
            collisionRects = new List<Rectangle>();
          
            foreach (var o in map.ObjectGroups["Collisions"].Objects)
            {
                if (o.Name == "")
                {
                    collisionRects.Add(new Rectangle((int)o.X,(int)o.Y,(int)o.Width,(int)o.Height));
                }
                if (o.Name == "start")
                {
                    startRect = new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);
                }
                if (o.Name == "end")
                {
                    endRect= new Rectangle((int)o.X, (int)o.Y, (int)o.Width, (int)o.Height);

                }
            }

            #endregion

            _gameManager=new GameManager(endRect);

            #region Player
            player = new Player(
                new Vector2(startRect.X,startRect.Y),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Idle (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Running (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\Agent_Mike_Jump"),
                Content.Load<Texture2D>("Sprite Pack 4\\Agent_Mike_Falling")
             );
            #region Bullet
            bullets=new List<Bullet>();
            bulletTexture=Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Bullet (16 x 16)");
            #endregion

            #endregion

            #region Camera
            camera=new Camera();
            #endregion

            #region Enemy
            enemyPathway=new List<Rectangle>();
            foreach(var o in map.ObjectGroups["EnemyPathways"].Objects)
            {
                enemyPathway.Add(new Rectangle((int)o.X,(int)o.Y,(int)o.Width,(int)o.Height));
            }
            enemyList=new List<Enemy>();
            Martian=new Enemy(
               Content.Load<Texture2D>("Sprite Pack 4\\2 - Martian_Red_Running (32 x 32)"),
               enemyPathway[0]
                );

            enemyList.Add(Martian);
            Martian=new Enemy(
               Content.Load<Texture2D>("Sprite Pack 4\\2 - Martian_Red_Running (32 x 32)"),
               enemyPathway[1]
                );
            enemyList.Add(Martian);
            #endregion

            renderTarget=new RenderTarget2D(GraphicsDevice, 1024, 850);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            #region Enemy
            foreach (var enemy in enemyList)
            {
                enemy.Update();

                if (enemy.hasHit(player.hitbox))
                {
                    hit_counter++;
                    if (hit_counter>time_between_hurt)
                    {
                        player_health--;
                        hit_counter=0;
                    }
                }



            }
            
            #endregion

            #region Camera update
            Rectangle target = new Rectangle((int)player.position.X,(int)player.position.Y,32,32);
            transformMatrix=camera.Follow(target);
            #endregion

            #region Managers
            if (_gameManager.hasGameEnded(player.hitbox))
            {
                Console.WriteLine("Game Ended!");
            }
            if (player_health<=0)
            {
                Console.WriteLine("Game Over");
            }
            #endregion

            #region Player

            #region Bullet

            if (player.isShooting)
            {
                if(time_between_bullets>5 && bullets.ToArray().Length<20)
                {
                    var temp_hitbox = new
                                Rectangle((int)player.position.X+7,
                                          (int)player.position.Y+15,
                                          bulletTexture.Width,
                                          bulletTexture.Height);
                    if (player.effects==SpriteEffects.None)
                    {
                        
                        bullets.Add(new Bullet(bulletTexture, 4, temp_hitbox));
                    }
                    if (player.effects==SpriteEffects.FlipHorizontally)
                    {

                        bullets.Add(new Bullet(bulletTexture, -4, temp_hitbox));
                    }
                    time_between_bullets=0;
                }
                else
                {
                    time_between_bullets++;
                }
                
            }

            foreach(var bullet in bullets.ToArray())
            {
                bullet.Update();

                foreach(var rect in collisionRects)
                {
                    if(rect.Intersects(bullet.hitbox))
                    {
                        bullets.Remove(bullet);
                        break;
                    }
                }
                foreach(var enemy in enemyList.ToArray())
                {
                    if (bullet.hitbox.Intersects(enemy.hitbox))
                    {
                        bullets.Remove(bullet);
                        enemyList.Remove(enemy);
                        points++;
                        break;
                    }
                }
            }
            
            #endregion


            #region Player Collisions
            var initPos = player.position;
            player.Update();
            //y axis

            foreach (var rect in collisionRects)
                {
                    if(!player.isJumping)
                    player.isFalling = true;
                    if (rect.Intersects(player.playerFallRect))
                    {
                        player.isFalling = false;
                        break;
                    }
                }
            
            
            //x axis
            foreach(var rect in collisionRects)
            {
                if (rect.Intersects(player.hitbox))
                {
                    player.position = initPos;
                    player.velocity = initPos;
                    break;
                }
            }
            #endregion

            #endregion

            #region UI
            GuiHelper.UpdateSetup(gameTime);
            _ui.UpdateAll(gameTime);

            Panel.Push().XY=new Vector2(10,10);

            Label.Put($"Points: {points}");
            Panel.Pop();
            Panel.Push().XY = new Vector2(10, 50);

            Label.Put($"Health: {player_health}");
            Panel.Pop();
            GuiHelper.UpdateCleanup();
            #endregion
            DrawLevel(gameTime);
            base.Update(gameTime);
        }

        public void DrawLevel(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            tilemapManager.Draw(_spriteBatch);
            #region Enemy
            foreach (var enemy in enemyList)
            {
                enemy.Draw(_spriteBatch, gameTime);
            }
            #endregion
            #region Player


            #region Bullets

            foreach (var bullet in bullets.ToArray())
            {
                bullet.Draw(_spriteBatch);
            }
            #endregion

            player.Draw(_spriteBatch, gameTime);

            #endregion
            _spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(renderTarget, new Vector2(0, 0), null, Color.White, 0f, new Vector2(), 2f, SpriteEffects.None, 0);
            _spriteBatch.End();
            _ui.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
