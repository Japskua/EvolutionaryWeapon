using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAPlatformerLevelEditor
{
    using Image = System.Drawing.Image;

    public partial class Form1 : Form
    {
        enum EditTool
        {
            Paint,
            Erase,
        }
        EditTool editTool = EditTool.Paint;

        #region Fields

        //The number of cells to fill when using the Fill tool. Set this too high and you'll get overflow errors.
        const int MaxFillCells = 500;

        readonly List<char> AnimatedSpriteChars = new List<char> { '1', 'A', 'B', 'C', 'D' };
        readonly Dictionary<char, string> CharactersDictionary = new Dictionary<char, string>()
        { 
            {'.', null},
            {'X', "/Tiles/Exit.png"},
            {'G', "/Sprites/Gem.png"},
            {'-', "/Tiles/Platform.png"},
            {'A', "/Sprites/MonsterA/Idle.png"},
            {'B', "/Sprites/MonsterB/Idle.png"},
            {'C', "/Sprites/MonsterC/Idle.png"},
            {'D', "/Sprites/MonsterD/Idle.png"},
            {'~', "/Tiles/BlockB0.png"},
            {':', "/Tiles/BlockB1.png"},
            {'1', "/Sprites/Player/Idle.png"},
            {'#', "/Tiles/BlockA0.png"},
            {'Q', "/Tiles/Block1.png"},
            {'W', "/Tiles/Block2.png"},
            {'E', "/Tiles/Block3.png"},
        };

        string pathToContentFolder = "D:/Projects/EvolutionaryWeapon/Platformer/Content";
        //string pathToContentFolder = "C:/Users/Jeff/Desktop/Nomis_LegacyIslands/Content";
        
        SpriteBatch spriteBatch;
        Texture2D gridTexture;
        
        //Camera so we can scroll the level if it's bigger than the screen
        Camera camera = new Camera();

        //A list of each char and the corresponding texture.
        Dictionary<char, Texture2D> tileDictionary = new Dictionary<char, Texture2D>();

        //Keep a list of levels so we don't have to load them each time we change levels in the list box.
        List<Tile[,]> levels = new List<Tile[,]>();
        Tile[,] currentLevel;

        //This contains the images used to display a picture for each tile in the bottom-right
        ImageList imageList = new ImageList();

        //Used for Painting/Erasing/Filling
        bool isMouseDown = false;
        int cellX, cellY;
        int fillCounter = MaxFillCells;

        //Scrollbar variables
        int maxWidth = 0, maxHeight = 0;

        #endregion

        #region Properties

        public int LevelWidthInTiles
        {
            get { return currentLevel.GetLength(0); }
        }
        public int LevelWidthInPixels
        {
            get { return LevelWidthInTiles * Tile.Width; }
        }

        public int LevelHeightInTiles
        {
            get { return currentLevel.GetLength(1); }
        }
        public int LevelHeightInPixels
        {
            get { return LevelHeightInTiles * Tile.Height; }
        }

        /// <summary>
        /// Gets the GraphicsDevice needed to draw our Tiles
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return tileDisplay1.GraphicsDevice; }
        }

        #endregion

        #region Initialization

        public Form1()
        {
            InitializeComponent();

            tileDisplay1.OnInitialize += new EventHandler(tileDisplay1_OnInitialize);
            tileDisplay1.OnDraw += new EventHandler(tileDisplay1_OnDraw);

            Application.Idle += delegate { tileDisplay1.Invalidate(); };

            saveFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.Title = "Find your 'Content' folder";
            folderBrowserDialog1.Description = "Choose your Content folder. To use the Default, go to where you extracted the files and navigate to XNAPlatformerLevelEditor/Platformer/Content";

            Mouse.WindowHandle = tileDisplay1.Handle;
        }

        private void tileDisplay1_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //We need to grab the directory of our Editor project so we can load the grid texture.
            //Environment.CurrentDirectory brings us to the 
            //      ".../XNAPlatformerLevelEditor/XNAPlatformerLevelEditor/bin/Debug"
            //folder so we want to remove the "bin/Debug" part
            string currentDirectory = Environment.CurrentDirectory;
            int index = currentDirectory.IndexOf("bin");

            //Remove everything after, and including, the index (removes "bin/Debug")
            string pathToEditorFolder = currentDirectory.Remove(index);

            //Since loading textures is relative to the Content folder, let's keep it for later!
            if(string.IsNullOrEmpty(pathToContentFolder))
            {
                DirectoryInfo dInfo = new DirectoryInfo(pathToEditorFolder);
                pathToContentFolder = dInfo.Parent.FullName + "/Platformer/Content";
            }
                
            //Now that we know where the Content folder is, load the textures that we're assigned at the very top!
            LoadAllTextures();

            //Load the texture that displays the grid lines
            using (Stream stream = File.OpenRead(pathToEditorFolder + "gridTexture.png"))
                gridTexture = Texture2D.FromStream(GraphicsDevice, stream);

            openFileDialog1.Filter = "Text Files|*.txt";
            openFileDialog1.Multiselect = true;
            openFileDialog1.InitialDirectory = pathToContentFolder;

            //Set some defaults for our image list
            imageList.ImageSize = new System.Drawing.Size(Tile.Width, Tile.Height); //Each image will be the same size as it appears in-game
            imageList.ColorDepth = ColorDepth.Depth32Bit; //The quality of the images shown.

            //And make sure to assign our image list to the large image list of our textureListView!
            //This will draw the large image for each texture in the textureListView
            //so we can see what texture we're painting with
            textureListView.LargeImageList = imageList;
        }

        #endregion

        #region Update/Draw

        void tileDisplay1_OnDraw(object sender, EventArgs e)
        {
            //To keep things organized we'll split our Update/Draw 
            //logic into different methods to keep things readable.
            UpdateLoop();
            DrawLoop();
        }

        private void UpdateLoop()
        {
            //Hook our camera's position to the scroll bars.
            camera.Position.X = hScrollBar1.Value * Tile.Width;
            camera.Position.Y = vScrollBar1.Value * Tile.Height;

            //Save the mouse coordinates
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            if (levelsListBox.SelectedItem != null)
            {
                if (mouseX >= 0 && mouseX < tileDisplay1.Width &&
                    mouseY >= 0 && mouseY < tileDisplay1.Height)
                {
                    //Gets the currently selected cell (in tile space)
                    cellX = mouseX / Tile.Width;
                    cellY = mouseY / Tile.Height;

                    //Keep the cell aligned when we scroll from side to side
                    cellX += hScrollBar1.Value;
                    cellY += vScrollBar1.Value;

                    //Keep the current cell inside of the level boundaries
                    cellX = (int)MathHelper.Clamp(cellX, 0, LevelWidthInTiles - 1);
                    cellY = (int)MathHelper.Clamp(cellY, 0, LevelHeightInTiles - 1);

                    if (isMouseDown)
                    {
                        if (editTool == EditTool.Paint &&
                            textureListView.SelectedItems.Count == 1 &&
                            textureListView.SelectedItems[0] != null)
                        {
                            //Grab the texture from the currently selected item in our textureListView
                            char tileType = textureListView.SelectedItems[0].Text[0];
                            
                            Texture2D texture = tileDictionary[tileType];
                            
                            //Fill up to 'MaxFillCells' cells
                            if (fillButton.Checked)
                            {
                                fillCounter = MaxFillCells;
                                FillCell(cellX, cellY, tileType);
                            }
                            else
                            {
                                //Otherwise just fill the individual cell
                                //currentLayer.SetCellAndCollisionIndex(cellX, cellY, cellIndex, collisionIndex);
                                currentLevel[cellX, cellY].Texture = texture;
                                currentLevel[cellX, cellY].TileType = tileType;
                            }
                        }
                        else if (editTool == EditTool.Erase)
                        {
                            if (fillButton.Checked)
                                FillCell(cellX, cellY, '.');
                            else
                            {
                                //Setting the texture to null will signify a blank spot
                                currentLevel[cellX, cellY].Texture = null;

                                //Assign the blank char to this tile. By default this is a period '.'
                                currentLevel[cellX, cellY].TileType = '.';
                            }
                        }
                    }
                }
                else
                {
                    cellX = cellY = -1;
                }
            }
        }

        /// <summary>
        /// Recursive method use to "fill" all adjacent cells with the same texture.
        /// </summary>
        /// <param name="x">X location (in tiles).</param>
        /// <param name="y">Y location (in tiles).</param>
        /// <param name="desiredChar">The character to change this specific location to.</param>
        public void FillCell(int x, int y, char desiredChar)
        {
            char oldChar = currentLevel[x, y].TileType;

            if (desiredChar == oldChar || fillCounter == 0)
                return;

            fillCounter--;

            currentLevel[x, y].TileType = desiredChar;
            currentLevel[x, y].Texture = tileDictionary[desiredChar];

            if (x > 0 && currentLevel[x - 1, y].TileType == oldChar)
                FillCell(x - 1, y, desiredChar);
            if (x < LevelWidthInTiles - 1 && currentLevel[x + 1, y].TileType == oldChar)
                FillCell(x + 1, y, desiredChar);

            if (y > 0 && currentLevel[x, y - 1].TileType == oldChar)
                FillCell(x, y - 1, desiredChar);
            if (y < LevelHeightInTiles - 1 && currentLevel[x, y + 1].TileType == oldChar)
                FillCell(x, y + 1, desiredChar);
        }

        private void DrawLoop()
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // For each tile position
            if (levelsListBox.SelectedItem != null)
            {
                for (int y = 0; y < LevelHeightInTiles; ++y)
                {
                    for (int x = 0; x < LevelWidthInTiles; ++x)
                    {
                        // If there is a visible tile in that position
                        Texture2D texture = currentLevel[x, y].Texture;
                        Vector2 position = new Vector2(x, y) * Tile.Size - camera.Position;

                        if (texture != null)
                        {                            
                            //We're also drawing the player/enemies in here so we need some
                            //special draw cases for them since they are not handling their
                            //own drawing in the Editor.
                            if (AnimatedSpriteChars.Contains(currentLevel[x, y].TileType))
                            {
                                //Be default, each frame in a spritesheet is the same width as height
                                //so if we grab the height value we can use that for the width as well
                                int height = currentLevel[x, y].Texture.Height;

                                position.Y -= (height / 2);
                                position.X -= (height / 4);

                                Rectangle sourceRect = new Rectangle(
                                    0,
                                    0,
                                    height,
                                    height);

                                spriteBatch.Draw(texture, position, sourceRect, Color.White);
                            
                                //If you wanted to resize the enemies so they fit inside of a single
                                //tile space you could use the following spriteBatch call instead:
                                //spriteBatch.Draw(
                                //    texture,
                                //    new Rectangle(
                                //        (int)position.X,
                                //        (int)position.Y,
                                //        Tile.Width,
                                //        Tile.Height),
                                //    sourceRect,
                                //    Color.White);
                            }
                            else
                            {
                                //Draw non sprite animated tiles here. Notice how we simply
                                //draw each tile at the specified location without a destination
                                //rectangle. This allows us to draw tiles that are bigger than 
                                //a single tile.
                                spriteBatch.Draw(texture, position, Color.White);

                                //If you wanted to resize the tiles so they fit inside of a single
                                //tile space you could use the following spriteBatch call instead:
                                //spriteBatch.Draw(
                                //    texture, 
                                //    new Rectangle(
                                //        (int)position.X,
                                //        (int)position.Y,
                                //        Tile.Width,
                                //        Tile.Height), 
                                //    Color.White);
                            }
                        }
                        else
                        {
                            //Draw the grid lines
                            spriteBatch.Draw(
                                gridTexture,
                                new Rectangle(
                                    (int)position.X,
                                    (int)position.Y,
                                    Tile.Width,
                                    Tile.Height),
                                Color.White);
                        }
                    }
                }

                if (cellX != -1 && cellY != -1)
                {
                    //Draw the red grid texture where your mouse is
                    spriteBatch.Draw(
                        gridTexture,
                        new Rectangle(
                            cellX * Tile.Width - (int)camera.Position.X,
                            cellY * Tile.Height - (int)camera.Position.Y,
                            Tile.Width,
                            Tile.Height),
                        Color.Red);
                }
            }

            spriteBatch.End();
        }

        #endregion

        #region Menu Strip Events

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewLevelForm newLevelForm = new NewLevelForm();

            newLevelForm.ShowDialog();

            if (newLevelForm.OKPressed)
            {
                int levelText;

                //By default, levels are simply a number with the .txt extension. (i.e. "0.txt")
                //so we'll use Int.TryParse to make sure they enter a valid name. We'll add the
                //".txt" extension ourself once we verify the format is correct.
                //The following are VALID names to enter: 1, 5, 17, 29
                //The following are NOT VALID names to enter: 1.txt, 2.2, 
                if (int.TryParse(newLevelForm.nameTextBox.Text, out levelText))
                {
                    string name = levelText + ".txt";

                    //The value here are hardcoded since the basic Platformer is designed to show
                    //the entire level on one screen (no scrolling). If your levels are a different
                    //size you can change that here, or you change the "widthTextBox" and "heightTextBox"
                    //ReadOnly property to "false" instead of "true". You could then use 
                    //newLevelForm.widthTextBox.Text and use that as your Width value below.
                    int width = int.Parse(newLevelForm.widthTextBox.Text);
                    int height = int.Parse(newLevelForm.heightTextBox.Text);
                    Tile[,] level = new Tile[width, height];

                    for (int y = 0; y < height; y++)
                        for (int x = 0; x < width; x++)
                            level[x, y].TileType = '.';

                    if (!levelsListBox.Items.Contains(name))
                    {
                        //Add the level to our list so we don't have load it later on.
                        levels.Add(level);

                        //Add it to the levelsListBox so it shows up on the right hand side
                        levelsListBox.Items.Add(name);

                        //Set the newly added level as the currently selected level.
                        levelsListBox.SelectedItem = levelsListBox.Items[levelsListBox.Items.Count - 1];
                        currentLevel = levels[levelsListBox.SelectedIndex];
                    }
                    else
                        MessageBox.Show(String.Format("{0} already exists.", name));
                }
                else
                {
                    MessageBox.Show("Please enter a valid name. If you want to create '5.txt' just enter '5'");
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //We reach this code once the User presses "OK" to open the .txt file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadSelectedLevels();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (levelsListBox.SelectedItem != null)
            {
                string filename = levelsListBox.SelectedItem as string;
                saveFileDialog1.FileName = filename;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                    {
                        for (int y = 0; y < LevelHeightInTiles; y++)
                        {
                            for (int x = 0; x < LevelWidthInTiles; x++)
                            {
                                //Write each character on this line
                                writer.Write(currentLevel[x, y].TileType);
                            }

                            //Jump to the next line
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show(String.Format("{0} saved.", saveFileDialog1.FileName));
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Sidebar Events

        private void addLevelButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LoadSelectedLevels();

                AdjustScrollBars();
            }
        }

        private void removeLevelButton_Click(object sender, EventArgs e)
        {
            if (levelsListBox.SelectedItem != null)
            {
                levels.RemoveAt(levelsListBox.SelectedIndex);
                levelsListBox.Items.Remove(levelsListBox.SelectedItem);
            }
        }

        private void AdjustScrollBars()
        {
            if (LevelWidthInPixels > tileDisplay1.Width)
            {
                maxWidth = (int)Math.Max(LevelWidthInTiles, maxWidth);
                hScrollBar1.Visible = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = maxWidth;
            }
            else
            {
                maxWidth = 0;
                hScrollBar1.Visible = false;
            }

            if (LevelHeightInPixels > tileDisplay1.Height)
            {
                maxHeight = (int)Math.Max(LevelHeightInTiles, maxHeight);
                vScrollBar1.Visible = true;
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = maxHeight;
            }
            else
            {
                maxHeight = 0;
                vScrollBar1.Visible = false;
            }
        }

        #endregion

        #region Loading - Levels/Textures/Images

        /// <summary>
        /// Loads the selected levels into the Editor.
        /// </summary>
        private void LoadSelectedLevels()
        {
            //Since we have Multiselect on we can add more than one level at a time
            foreach (string file in openFileDialog1.FileNames)
            {
                //Grab just the file name instead of the full path.
                //So "C:\Users\Jeff\My Documents\My Dropbox\XNAPlatformerLevelEditor\Platformer\Content\Levels\0.txt"
                //will become simply "0.txt"
                string filename = Path.GetFileName(file);

                //If the level doesn't already exist in our list, then add it.
                //This assumes you don't have multiple levels with the same name.
                //If you do then this will require some modification to work.
                if (!levelsListBox.Items.Contains(filename))
                {
                    int width;
                    List<string> lines = new List<string>();

                    //Open a StreamReader to read the file and make sure each row is the same length.
                    using (StreamReader reader = new StreamReader(file))
                    {
                        string line = reader.ReadLine();
                        width = line.Length;

                        while (line != null)
                        {
                            lines.Add(line);

                            //If a line has a different width than the first line then that's no good!
                            if (line.Length != width)
                                throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));

                            //Read the next line. If we just read the last line then this will be null
                            line = reader.ReadLine();
                        }
                    }

                    //Create our array of tiles wince we have the width/height values now
                    Tile[,] level = new Tile[width, lines.Count];

                    //Assign a texture to each array position
                    for (int y = 0; y < lines.Count; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            //Get the 'char' at this location and assign it to our tile
                            char tileType = lines[y][x];
                            level[x, y].TileType = tileType;

                            //Assign the texture to each tile based on which char it's using
                            level[x, y].Texture = tileDictionary[tileType];
                        }
                    }

                    //Add it to our listbox so we can select it
                    levelsListBox.Items.Add(filename);

                    //Add the level we just loaded to our list of levels
                    levels.Add(level);
                }
            }

            levelsListBox.SelectedItem = levelsListBox.Items[levelsListBox.Items.Count - 1];
            currentLevel = levels[levelsListBox.SelectedIndex];
        }

        private void LoadAllTextures()
        {
            //Check to make sure the directory assigned exists.
            if (!Directory.Exists(pathToContentFolder))
            {
                //If the path to the Content folder doesn't exist, let the User know.
                MessageBox.Show(String.Format("The {0} directory does not exist. Please check that 'pathToContentFolder' is correctly assigned.", pathToContentFolder));

                //There's no need to continue if we have the wrong folder, so just close the application.
                Close();
            }
            
            //For all tiles we have in the game, let's load their textures!
            foreach (char c in CharactersDictionary.Keys)
            {
                //Make sure the image exists before we load it!
                if (!File.Exists(pathToContentFolder + CharactersDictionary[c]))
                {
                    //Blank tiles (.) are null because they don't have a texture, so skip them
                    if (CharactersDictionary[c] != null)
                    {
                        //If it doesn't exist, let the User know so they can change it!
                        MessageBox.Show(String.Format(
                            "The file path for tile '{0}' does not exist. You tried to load {1}. Make sure 'pathToContentFolder' and the path for tile '{0}' in CharactersDictionary are correctly assigned.",
                            c,
                            pathToContentFolder + CharactersDictionary[c]));

                        //Close the application so they can change change it to the correct value.
                        Close();
                    }
                } 
                tileDictionary.Add(c, GetTileTexture(c));

                //Load an image so we can show a preview of it in our texture list view
                Image image = GetTileImage(c);

                if (image != null)
                {
                    //Here we have to work some magic since _most_ of our players/enemies are in the
                    //form of spritesheets instead of a single image. What we want to do is get the
                    //first frame of the image and use that for our image. We use our List of PlayerAndEnemyChars
                    //in the Tile.cs class to find out if we're dealing with a player/enemy image.
                    if (AnimatedSpriteChars.Contains(c))
                    {
                        System.Drawing.Bitmap original = new System.Drawing.Bitmap(image);
                        System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(0, 0, image.Height, image.Height);
                        image = (Image)original.Clone(srcRect, original.PixelFormat);
                    }

                    //Add it to our image list which we use to draw each texture in the texture list view
                    imageList.Images.Add(c.ToString(), image);
                }

                ListViewItem item = new ListViewItem(c.ToString(), imageList.Images.IndexOfKey(c.ToString()));

                //And finally, add it to our list of textures to show up in the texture list view
                textureListView.Items.Add(item);
            }
        }

        private Image GetTileImage(char tileType)
        {
            Image image = null;
            string filePathFromContentDirectory = CharactersDictionary[tileType];

            if (!string.IsNullOrEmpty(filePathFromContentDirectory))
            {
                //Open a new stream so we can load the image
                using (Stream stream = File.OpenRead(pathToContentFolder + filePathFromContentDirectory))
                    image = Image.FromStream(stream);
            }

            return image;
        }
        private Texture2D GetTileTexture(char tileType)
        {
            Texture2D texture = null;
            string filePathFromContentDirectory = CharactersDictionary[tileType];

            if (!string.IsNullOrEmpty(filePathFromContentDirectory))
            {
                //Open a new stream so we can load the texture
                using (Stream stream = File.OpenRead(pathToContentFolder + filePathFromContentDirectory))
                    texture = Texture2D.FromStream(GraphicsDevice, stream);
            }

            return texture;
        }

        #endregion

        #region Sidebar Events

        private void paintButton_Click(object sender, EventArgs e)
        {
            editTool = EditTool.Paint;
            UpdateToolButtons();
        }

        private void eraseButton_Click(object sender, EventArgs e)
        {
            editTool = EditTool.Erase;
            UpdateToolButtons();
        }

        private void UpdateToolButtons()
        {
            paintButton.Checked = editTool == EditTool.Paint;
            paintButton.Enabled = true;

            eraseButton.Checked = editTool == EditTool.Erase;
            eraseButton.Enabled = true;
        }

        private void loadTexturesButton_Click(object sender, EventArgs e)
        {
            //Since we don't support adding textures on the fly (they're pre-set by default in code)
            //we only need to load our textures once.
            LoadAllTextures();
        }

        private void levelsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (levelsListBox.SelectedItem != null)
            {
                currentLevel = levels[levelsListBox.SelectedIndex];
            }
        }

        #endregion

        private void tileDisplay1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
        }
        private void tileDisplay1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
    }
}
