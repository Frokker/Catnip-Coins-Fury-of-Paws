using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CatnipCoinsFuryOfPaws
{
    public partial class Game : Form
    {
        private Timer timer;
        private Point playerPosition;
        private Point playerOffset;
        private char[,] map;
        private Image[] catFrames;
        private Image spiderImage;
        private Image grassImage;
        private Image stoneImage;
        private int currentFrameIdx;
        private int tileWidth = 32;
        private int tileHeight = 32;
        private int viewWidth = 10;
        private int viewHeight = 10;
        private Dictionary<Keys, Point> keyMovements;

        public Game()
        {
            InitializeComponent();
            LoadResources();
            LoadMap("Levels/firstLevel.txt");
            InitializeGame();
        }

      
        private void LoadResources()
        {
            catFrames = new Image[8];
            for (int i = 0; i < 8; i++)
                catFrames[i] = Image.FromFile($"Assets/Images/Cat/CatWalking/CatWalkingFrame{i + 1}.png");
            
            spiderImage = Image.FromFile("Assets/Images/Enemies/Spider/SpiderWalking/SpiderWalkingFrame1.png");
            grassImage = Image.FromFile("Assets/Images/Background/Grass/PinkFlower.png");
            stoneImage = Image.FromFile("Assets/Images/Background/Wall.png");
        }

        private void LoadMap(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            map = new char[lines.Length, lines[0].Length];
            
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    map[y, x] = lines[y][x];
                }
            }
        }

        private void InitializeGame()
        {
            this.DoubleBuffered = true;
            this.Width = viewWidth * tileWidth + 16;
            this.Height = viewHeight * tileHeight + 39;
            this.playerPosition = new Point(1, 1);

            keyMovements = new Dictionary<Keys, Point>
            {
                { Keys.W, new Point(0, -1) },
                { Keys.A, new Point(-1, 0) },
                { Keys.S, new Point(0, 1) },
                { Keys.D, new Point(1, 0) }
            };

            timer = new Timer { Interval = 100 };
            timer.Tick += Timer_Tick;
            timer.Start();
            this.Paint += GameForm_Paint;
            this.KeyDown += GameForm_KeyDown;
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            int startX = playerPosition.X - viewWidth / 2;
            int startY = playerPosition.Y - viewHeight / 2;

            for (int y = 0; y < viewHeight; y++)
            {
                for (int x = 0; x < viewWidth; x++)
                {
                    int mapY = startY + y;
                    int mapX = startX + x;

                    if (mapY >= 0 && mapY < map.GetLength(0) && mapX >= 0 && mapX < map.GetLength(1))
                    {
                        char tile = map[mapY, mapX];
                        Image image = tile switch
                        {
                            '#' => stoneImage,
                            ' ' => grassImage,
                            '*' => spiderImage,
                            _ => grassImage,
                        };

                        e.Graphics.DrawImage(image, x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                    }
                }
            }

            e.Graphics.DrawImage(catFrames[currentFrameIdx], (viewWidth / 2) * tileWidth, (viewHeight / 2) * tileHeight, tileWidth, tileHeight);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            currentFrameIdx = (currentFrameIdx + 1) % catFrames.Length;
            this.Invalidate();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyMovements.TryGetValue(e.KeyCode, out Point movement))
            {
                Point new_position = new Point(playerPosition.X + movement.X, playerPosition.Y + movement.Y);

                if (new_position.Y >= 0 && new_position.Y < map.GetLength(0) && 
                    new_position.X >= 0 && new_position.X < map.GetLength(1) &&
                    map[new_position.Y, new_position.X] != '#')
                {
                    playerPosition = new_position;

                    if (map[playerPosition.Y, playerPosition.X] == '*')
                    {
                        MessageBox.Show("You are killed by a spider!");
                        playerPosition = new Point(1, 1); // Reset position
                    }
                }
            }
        }

        
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game));
            this.timer1 = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.SynchronizingObject = this;
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 493);
            this.MaximumSize = new System.Drawing.Size(960, 540);
            this.MinimumSize = new System.Drawing.Size(960, 540);
            this.Name = "Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Catnip Coins: Fury of Paws";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Game_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Timers.Timer timer1;
    }
}