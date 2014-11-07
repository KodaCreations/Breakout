using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Breakout
{
    public class Bricks
    {
        Texture2D brickTex;
        Vector2 brickPos;


        //****************************************************
        public Bricks(Texture2D brickTex, Vector2 brickPos)
        {
            this.brickTex = brickTex;
            this.brickPos = brickPos;
        }


        //****************************************************
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)brickPos.X, (int)brickPos.Y, brickTex.Width, brickTex.Height);
            }
        }


        //****************************************************
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(brickTex, brickPos, Color.White);
        }

    }
}
