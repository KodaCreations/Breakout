using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Texture2D ballTex, brickTex, paddleTex, gameOver, gameWon, gameMenu, background;
        public Vector2 ballPos, platePos, textPos;
        public SpriteFont text;
        bool gamewon = false;
        bool gameover = false;
        float currentScore, finalScore, bonusScore;

        KeyboardState keyState;

        enum GameState { GameMenu, InGame, GameOver }
        GameState currentGameState = GameState.GameMenu;
        
        List<Bricks> bricks = new List<Bricks>();
        Plate plate;
        Ball ball;

        


        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
        }


        
        public void Score(int score)
        { currentScore += score; }


        
        public void LoadLevel()
        {
            FileStream file = new FileStream(@"Content/Level.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            int numberOfRows;
            string rowOfBricks;
            numberOfRows = int.Parse(reader.ReadLine());

            for (int i = 0; i < numberOfRows; i++)
            {
                rowOfBricks = reader.ReadLine();
                string[] entries = rowOfBricks.Split(',');

                for (int j = 0; j < entries.Length; j++)
                {
                    bricks.Add(new Bricks(brickTex, new Vector2(j * 62 + 78, i * 32 + 78)));
                }
            }
            file.Close();
            reader.Close();
        }


        
        protected override void Initialize()
        {
            base.Initialize();
        }


        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTex = Content.Load<Texture2D>("Ball_new");
            brickTex = Content.Load<Texture2D>("Brick_new");
            paddleTex = Content.Load<Texture2D>("Plate_new");
            gameOver = Content.Load<Texture2D>("Gameover_new");
            gameWon = Content.Load<Texture2D>("Gamewon_new");
            gameMenu = Content.Load<Texture2D>("Gamemenu_new");
            background = Content.Load<Texture2D>("Background_new");
            text = Content.Load<SpriteFont>("SpriteFont");

            LoadLevel();
            
            plate = new Plate(paddleTex);
            ball = new Ball(ballTex, ballPos, text, platePos, paddleTex);
        }


        
        protected override void UnloadContent()
        {
        }


        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            keyState = Keyboard.GetState();

            switch (currentGameState)
            {

            //***Start***//
                case GameState.GameMenu:

                    ball.isBallMoving = false;
                    bricks.Clear();
                    LoadLevel();
                    gameover = false;
                    gamewon = false;
                    
                    currentScore = 0;
                    ball.currentLives = 3;

                    if (keyState.IsKeyDown(Keys.F1))
                    {
                        currentGameState = GameState.InGame;
                        ball.isBallMoving = false;
                    }

                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }

                    break;


            //***InGame***//
                case GameState.InGame:

                    for (int j = 0; j < bricks.Count; j++)
                    {
                        if (ball.CollisionBrick(bricks[j].BoundingBox, brickTex) == true)
                        {
                            bricks.RemoveAt(j);
                            currentScore = currentScore + 5;
                            break;
                        }
                    }

                    plate.Update();
                    ball.Update(plate.platePos, plate);

                    //Checks if you've won the game
                    if (currentScore == 560)
                    {
                        gamewon = true;
                        finalScore = currentScore + (ball.currentLives * 100);
                        bonusScore = ball.currentLives * 100;
                        currentGameState = GameState.GameOver;
                    }

                    //Checks if you're out of lives
                    if (ball.currentLives == 0)
                    {
                        gameover = true;
                        
                        currentGameState = GameState.GameOver;
                    }

                    if (keyState.IsKeyDown(Keys.F2))
                    {
                        currentGameState = GameState.GameMenu;
                    }

                    break;


            //***GameOver***//
                case GameState.GameOver:

                    ball.isBallMoving = false;
                    

                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        this.Exit();
                    }

                    if (keyState.IsKeyDown(Keys.F2))
                    {
                        currentGameState = GameState.GameMenu;
                    }

                    break;
            }

            base.Update(gameTime);
        }


        
        public void FinalScore()
        {
            spriteBatch.DrawString(text,
                "Lives Left: " + ball.currentLives.ToString(),
                new Vector2(Window.ClientBounds.Width / 2 - text.MeasureString("Lives Left: X").X / 2, Window.ClientBounds.Height / 2),
                Color.Black);

            spriteBatch.DrawString(text,
                "Bonus Score: " + bonusScore.ToString(),
                new Vector2(Window.ClientBounds.Width / 2 - text.MeasureString("Bonus Score: XXX").X / 2, Window.ClientBounds.Height / 2 + 25),
                Color.Black);

            spriteBatch.DrawString(text,
                "Final Score: " + finalScore.ToString(),
                new Vector2(Window.ClientBounds.Width / 2 - text.MeasureString("Final Score: XXX").X / 2, Window.ClientBounds.Height / 2 + 50),
                Color.Black);
        }


        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            switch (currentGameState)
            {

            //***Start***//
                case GameState.GameMenu:

                    spriteBatch.Draw(gameMenu,
                        Vector2.Zero,
                        Color.White);

                    break;


            //***InGame***//
                case GameState.InGame:

                    spriteBatch.Draw(background,
                        Vector2.Zero,
                        Color.White);

                    foreach (Bricks brick in bricks)
                    {
                        brick.Draw(spriteBatch);
                    }

                    plate.Draw(spriteBatch);
                    ball.Draw(spriteBatch);

                    spriteBatch.DrawString(text,
                        "Score: " +currentScore.ToString(),
                        new Vector2(150, 740),
                        Color.Black);

                    spriteBatch.DrawString(text,
                        "Press F2 to return to the Main Menu",
                        new Vector2(675, 740),
                        Color.Black);

                    break;


            //***GameOver***//
                case GameState.GameOver:

                    if (gamewon == true)
                    {
                        spriteBatch.Draw(gameWon,
                            Vector2.Zero,
                            Color.White);

                        FinalScore();
                    }

                    if (gameover == true)
                    {
                        spriteBatch.Draw(gameOver,
                            Vector2.Zero,
                            Color.White);

                        FinalScore();
                    }

                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
