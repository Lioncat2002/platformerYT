using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TiledSharp;

namespace platformerYT.src
{
    public class TilemapManager
    {
        
        TmxMap map;
        Texture2D tileset;
        RenderTarget2D renderTarget;
       
        int tilesetTilesWide;
        int tileWidth;
        int tileHeight;

        public TilemapManager(TmxMap _map, Texture2D _tileset, int _tilesetTilesWide, int _tileWidth, int _tileHeight,GraphicsDevice graphicsDevice,SpriteBatch spriteBatch)

        {
            
            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            

            renderTarget=new RenderTarget2D(graphicsDevice, 1024, 512);
            DrawTilemap(graphicsDevice,spriteBatch);
        }

        public void DrawTilemap(GraphicsDevice graphicsDevice,SpriteBatch spriteBatch)
        {
            graphicsDevice.SetRenderTarget(renderTarget); 
            graphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            for (var i = 0; i < map.TileLayers.Count; i++)
            {
                for (var j = 0; j < map.TileLayers[i].Tiles.Count; j++)
                {
                    int gid = map.TileLayers[i].Tiles[j].Gid;
                    if (gid == 0)
                    {
                        //do nothing
                    }
                    else
                    {
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                        float x = (j % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                        Rectangle tilesetRec = new Rectangle((tileWidth) * column, (tileHeight) * row, tileWidth, tileHeight);
                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                    }
                }
            }
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);

        }

    }
}
