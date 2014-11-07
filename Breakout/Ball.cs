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
    class Ball
    {
        Texture2D ballTex, plateTex;
        Vector2 ballPos;
        Vector2 ballVelocity = new Vector2(0f, 0f); //Starting speed
        Vector2 ballSpeed = new Vector2(0, -5); //Starting direction
        Point ballPoint, platePoint;
        SpriteFont text;
        public Rectangle ballRec;
        public float currentLives = 3;
        public bool isBallMoving = false;

        public void Lives(int lives)
        { currentLives += lives; }


        //****************************************************
        public Ball(Texture2D ballTex, Vector2 ballPos, SpriteFont text, Vector2 platePos, Texture2D plateTex)
        {
            this.ballTex = ballTex;
            this.ballPos = ballPos;
            this.text = text;
            this.plateTex = plateTex;
        }


        //****************************************************
        public void CollisionPlate(Plate plate, Vector2 platePos, Texture2D plateTex)
        {
            ballRec = new Rectangle((int)ballPos.X, (int)ballPos.Y, ballTex.Width, ballTex.Height);
            
            //Checks if the ball collides with the controlplate
            if (plate.Collision().Intersects(ballRec))
            {
                //Calculates at which angle the ball is reflected if there is a collision
                platePoint.X = (int)(platePos.X + (plateTex.Width / 2));
                ballPoint.X = (int)(ballPos.X + (ballTex.Width / 2));
                ballSpeed.X = (ballPoint.X-platePoint.X)/10;
                ballSpeed.Y = -ballSpeed.Y;
            }    
        }


        //****************************************************
        public bool CollisionBrick(Rectangle brickRec, Texture2D brickTex)
        {
            Vector2 brickPos = new Vector2(brickRec.X, brickRec.Y);
            
            //Checks if a collision has happened
            if (ballRec.Intersects(brickRec))
            {
                //Checks if the collision happened on the left or right sides of the brick
                if ((ballPos.Y > brickPos.Y && ballPos.Y + 12 < brickPos.Y + brickTex.Height) || (ballPos.Y + 12 < brickPos.Y && ballPos.Y > brickPos.Y + brickTex.Height))
                {
                    ballSpeed.X = -ballSpeed.X;
                }
                
                //Checks if the collision happened on the top or bottom sides of the brick
                else if ((ballPos.X > brickPos.X && ballPos.X + 12 < brickPos.X + brickTex.Width) || (ballPos.X + 12 > brickPos.X && ballPos.X < brickPos.X + brickTex.Width))
                {
                    ballSpeed.Y = -ballSpeed.Y;
                }

                return true;

            }

            return false;
        }


        //****************************************************
        public void Update(Vector2 platePos, Plate plate)
        {

            //Sets the ball in motion if space is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                isBallMoving = true;
            }

            if (isBallMoving == true)
            {
                ballVelocity = new Vector2(ballSpeed.X,ballSpeed.Y);
                ballPos = ballPos + ballVelocity;
            }


            //Sets the ball to center above the controlplate if it's not moving
            if (isBallMoving == false)
            {
                ballPos.X = (int)platePos.X + (plateTex.Width/2) - (ballTex.Width/2);
                ballPos.Y = (int)platePos.Y - 12;
            }


            //Checks if the ball hits the top, left or right sides of the gamefield
            if (ballPos.X < 25 || ballPos.X > 987)
            {
                ballSpeed.X = ballSpeed.X * -1;
            }

            if (ballPos.Y < 25)
            {
                ballSpeed.Y = ballSpeed.Y * -1;
            }

            //Checks if the ball hits the bottom of the gamefield
            if (ballPos.Y > 710)
            {
                isBallMoving = false;
                currentLives = currentLives - 1;
                ballSpeed = new Vector2(0, -5);
            }

            //Checks the Collision methods if there's a collision
            CollisionPlate(plate, platePos, plateTex);
        }


        //****************************************************
        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(ballTex,
                    ballPos,
                    Color.White);

                spriteBatch.DrawString(text,
                    "Lives: " + currentLives.ToString(),
                    new Vector2(30, 740),
                    Color.Black);
        }

    }
}