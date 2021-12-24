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
        public static float screenWidth;
        public static float screenHeight;

        #region Managers
        private GameManager _gameManager;
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
            _graphics.PreferredBackBufferHeight = 450;
            _graphics.PreferredBackBufferWidth = 500;
            _graphics.ApplyChanges();
            screenHeight = _graphics.PreferredBackBufferHeight; 
            screenWidth = _graphics.PreferredBackBufferWidth;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);



            #region Tilemap
            map = new TmxMap("Content\\level1.tmx");
            tileset = Content.Load<Texture2D>("Cave Tileset\\"+map.Tilesets[0].Name.ToString());
            int tileWidth = map.Tilesets[0].TileWidth;
            int tileHeight = map.Tilesets[0].TileHeight;
            int tilesetTileWidth = tileset.Width / tileWidth;

            tilemapManager = new TilemapManager(map,tileset,tilesetTileWidth,tileWidth,tileHeight);
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
            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #region Enemy
            foreach(var enemy in enemyList)
            {
                enemy.Update();
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            tilemapManager.Draw(_spriteBatch);
            #region Enemy
            foreach (var enemy in enemyList)
            {
                enemy.Draw(_spriteBatch,gameTime);
            }
            #endregion
            player.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
