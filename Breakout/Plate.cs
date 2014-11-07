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
    class Plate
    {
        Texture2D paddleTex;
        public Vector2 platePos;


        //****************************************************
        public Plate(Texture2D paddleTex)
        {
            this.paddleTex = paddleTex;
        }


        //****************************************************
        public Rectangle Collision()
        {
            return new Rectangle((int)platePos.X, (int)platePos.Y, paddleTex.Width, paddleTex.Height);
        }


        //****************************************************
        public void Update()
        {
            MouseState mouseState = Mouse.GetState();

            platePos.X = mouseState.X - (paddleTex.Width/2);
            platePos.Y = 690;
        }


        //****************************************************
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(paddleTex, platePos, Color.White);
        }

    }
}
