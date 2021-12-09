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
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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


            #region Player
            player = new Player(
                new Vector2(startRect.X,startRect.Y),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Idle (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\1 - Agent_Mike_Running (32 x 32)"),
                Content.Load<Texture2D>("Sprite Pack 4\\Agent_Mike_Jump"),
                Content.Load<Texture2D>("Sprite Pack 4\\Agent_Mike_Falling")
             );
            #endregion
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var initPos = player.position;
            player.Update();
            #region Player Collisions
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

            _spriteBatch.Begin();

            tilemapManager.Draw(_spriteBatch);
            player.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
