using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BattleshipGame
{
    public partial class Form1 : Form
    {
        private const int GridSize = 10;
        private const int CellSize = 30;
        private Button[,] btnPlayerGrid;
        private Button[,] btnComputerGrid;
        private Board playerBoard;
        private Board computerBoard;
        private Queue<Point> aiTargets;
        private Random rnd = new Random();
        private bool isHorizontal = true;
        private int currentShipIndex = 0;
        private GamePhase phase = GamePhase.Placing;
        private readonly int[] ShipLengths = new int[] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem новаГраToolStripMenuItem;
        private ToolStripMenuItem вихідToolStripMenuItem;

        // Визначаємо кольори клітинок відповідно до загального стилю
        private readonly Color EmptyCellColor = Color.LightSkyBlue;
        private readonly Color ShipCellColor = Color.Gray;
        private readonly Color HitCellColor = Color.Crimson;
        private readonly Color MissCellColor = Color.LightGray;
        private readonly Color GridBorderColor = Color.DarkBlue; // для меж клітинок (через FlatAppearance.BorderColor)

        public Form1()
        {
            InitializeComponent();

            // Якщо lblStatus створена у дизайнері, ховаємо її (ми використовуємо StatusStrip)
            if (lblStatus != null)
                lblStatus.Visible = false;

            // Встановлюємо параметри lblStatus, хоча він прихований
            lblStatus.AutoSize = false;
            lblStatus.Dock = DockStyle.Top;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Font = new Font(lblStatus.Font.FontFamily, 20);

            CreateStatusStrip();
            CreateMenu();

            InitializeGame();
            CreateGridButtons();

            новаГраToolStripMenuItem.Click += новаГраToolStripMenuItem_Click;
            вихідToolStripMenuItem.Click += вихідToolStripMenuItem_Click;

            btnRotate.Click += BtnRotate_Click;
            btnRandomPlace.Click += BtnRandomPlace_Click;
            btnStartGame.Click += BtnStartGame_Click;

            lblStatus.Text = $"Розміщення кораблів: корабель довжини {ShipLengths[currentShipIndex]}";
            btnStartGame.Enabled = false;
        }

        private void CreateStatusStrip()
        {
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();

            toolStripStatusLabel.Spring = true;
            toolStripStatusLabel.TextAlign = ContentAlignment.MiddleCenter;
            toolStripStatusLabel.Font = new Font(toolStripStatusLabel.Font.FontFamily, 20);

            statusStrip1.Items.Add(toolStripStatusLabel);
            this.Controls.Add(statusStrip1);
        }

        private void CreateMenu()
        {
            if (menuStrip1 != null)
                this.Controls.Remove(menuStrip1);

            menuStrip1 = new MenuStrip();
            menuStrip1.Dock = DockStyle.Top;
            menuStrip1.GripStyle = ToolStripGripStyle.Hidden;
            menuStrip1.BackColor = SystemColors.Control;
            menuStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;

            float fontSize = 16f;
            menuStrip1.Font = new Font(menuStrip1.Font.FontFamily, fontSize);

            новаГраToolStripMenuItem = new ToolStripMenuItem("Нова гра");
            вихідToolStripMenuItem = new ToolStripMenuItem("Вихід");

            новаГраToolStripMenuItem.AutoSize = true;
            вихідToolStripMenuItem.AutoSize = true;

            новаГраToolStripMenuItem.TextAlign = ContentAlignment.MiddleCenter;
            вихідToolStripMenuItem.TextAlign = ContentAlignment.MiddleCenter;

            menuStrip1.Items.Add(новаГраToolStripMenuItem);
            menuStrip1.Items.Add(вихідToolStripMenuItem);

            this.MainMenuStrip = menuStrip1;
            this.Controls.Add(menuStrip1);
            menuStrip1.BringToFront();

            новаГраToolStripMenuItem.Click -= новаГраToolStripMenuItem_Click;
            новаГраToolStripMenuItem.Click += новаГраToolStripMenuItem_Click;
            вихідToolStripMenuItem.Click -= вихідToolStripMenuItem_Click;
            вихідToolStripMenuItem.Click += вихідToolStripMenuItem_Click;

            AdjustMenuItemsCentering();

            this.Resize += (s, e) => AdjustMenuItemsCentering();
        }

        private void AdjustMenuItemsCentering()
        {
            if (menuStrip1 == null || menuStrip1.Items.Count == 0)
                return;

            menuStrip1.PerformLayout();

            int totalWidth = 0;
            foreach (ToolStripItem item in menuStrip1.Items)
            {
                totalWidth += item.Width;
            }
            int clientW = this.ClientSize.Width;
            int leftPadding = (clientW - totalWidth) / 2;
            if (leftPadding < 0) leftPadding = 0;
            menuStrip1.Padding = new Padding(leftPadding, 0, 0, 0);
            menuStrip1.Invalidate();
        }

        private void InitializeGame()
        {
            playerBoard = new Board(GridSize);
            computerBoard = new Board(GridSize);
            aiTargets = new Queue<Point>();
            currentShipIndex = 0;
            phase = GamePhase.Placing;
            isHorizontal = true;
        }

        private void UpdateStatus(string text)
        {
            lblStatus.Text = text;
            if (toolStripStatusLabel != null)
                toolStripStatusLabel.Text = text;
        }

        private void CreateGridButtons()
        {
            btnPlayerGrid = new Button[GridSize, GridSize];
            btnComputerGrid = new Button[GridSize, GridSize];

            panelPlayer.Controls.Clear();
            panelComputer.Controls.Clear();

            // Встановлюємо фон панелей, щоб контраст був із клітинками
            panelPlayer.BackColor = Color.AliceBlue;
            panelComputer.BackColor = Color.AliceBlue;

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    var btnP = new Button
                    {
                        Name = $"btnPlayer_{x}_{y}",
                        Size = new Size(CellSize - 2, CellSize - 2),
                        Location = new Point(x * CellSize, y * CellSize),
                        BackColor = EmptyCellColor,
                        Tag = new Point(x, y)
                    };
                    btnP.FlatStyle = FlatStyle.Flat;
                    btnP.FlatAppearance.BorderSize = 1;
                    btnP.FlatAppearance.BorderColor = GridBorderColor;
                    btnP.UseVisualStyleBackColor = false;
                    btnP.Click += BtnPlayerGrid_Click;
                    panelPlayer.Controls.Add(btnP);
                    btnPlayerGrid[x, y] = btnP;

                    var btnC = new Button
                    {
                        Name = $"btnComputer_{x}_{y}",
                        Size = new Size(CellSize - 2, CellSize - 2),
                        Location = new Point(x * CellSize, y * CellSize),
                        BackColor = EmptyCellColor,
                        Tag = new Point(x, y),
                        Enabled = false
                    };
                    btnC.FlatStyle = FlatStyle.Flat;
                    btnC.FlatAppearance.BorderSize = 1;
                    btnC.FlatAppearance.BorderColor = GridBorderColor;
                    btnC.UseVisualStyleBackColor = false;
                    btnC.Click += BtnComputerGrid_Click;
                    panelComputer.Controls.Add(btnC);
                    btnComputerGrid[x, y] = btnC;
                }
            }
            AddGridLabels(panelPlayer);
            AddGridLabels(panelComputer);
        }

        private void AddGridLabels(Panel gridPanel)
        {
            Point origin = gridPanel.Location;
            for (int i = 0; i < GridSize; i++)
            {
                var lbl = new Label
                {
                    Text = ((char)('A' + i)).ToString(),
                    Size = new Size(CellSize, CellSize / 2),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                lbl.Location = new Point(origin.X + i * CellSize, origin.Y - lbl.Height);
                this.Controls.Add(lbl);
            }
            for (int i = 0; i < GridSize; i++)
            {
                var lbl = new Label
                {
                    Text = (i + 1).ToString(),
                    Size = new Size(CellSize / 2, CellSize),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                lbl.Location = new Point(origin.X - lbl.Width, origin.Y + i * CellSize);
                this.Controls.Add(lbl);
            }
        }

        private void BtnRotate_Click(object sender, EventArgs e)
        {
            isHorizontal = !isHorizontal;
            btnRotate.Text = isHorizontal ? "Горизонтально" : "Вертикально";
        }

        private void BtnRandomPlace_Click(object sender, EventArgs e)
        {
            playerBoard = new Board(GridSize);
            currentShipIndex = ShipLengths.Length;
            var rndBoard = new Board(GridSize);
            foreach (int len in ShipLengths)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = rnd.Next(GridSize);
                    int y = rnd.Next(GridSize);
                    bool hor = rnd.Next(2) == 0;
                    placed = rndBoard.PlaceShip(x, y, len, hor);
                }
            }
            playerBoard = rndBoard;
            for (int x = 0; x < GridSize; x++)
                for (int y = 0; y < GridSize; y++)
                {
                    bool hasShip = (playerBoard.Grid[x, y] == CellState.Ship);
                    btnPlayerGrid[x, y].BackColor = hasShip ? ShipCellColor : EmptyCellColor;
                    btnPlayerGrid[x, y].UseVisualStyleBackColor = false;
                    btnPlayerGrid[x, y].FlatStyle = FlatStyle.Flat;
                    btnPlayerGrid[x, y].FlatAppearance.BorderColor = GridBorderColor;
                    btnPlayerGrid[x, y].Enabled = false;
                    btnPlayerGrid[x, y].Click -= BtnPlayerGrid_Click;
                }
            UpdateStatus("Кораблі розміщені випадково. Натисніть 'Почати гру'.");
            btnStartGame.Enabled = true;
        }

        private void BtnStartGame_Click(object sender, EventArgs e)
        {
            computerBoard = new Board(GridSize);
            foreach (int len in ShipLengths)
            {
                bool placed = false;
                while (!placed)
                {
                    int x = rnd.Next(GridSize);
                    int y = rnd.Next(GridSize);
                    bool hor = rnd.Next(2) == 0;
                    placed = computerBoard.PlaceShip(x, y, len, hor);
                }
            }
            for (int x = 0; x < GridSize; x++)
                for (int y = 0; y < GridSize; y++)
                {
                    btnComputerGrid[x, y].Enabled = true;
                }
            phase = GamePhase.InProgress;
            UpdateStatus("Ваш хід: оберіть клітинку на полі комп'ютера.");
            btnRotate.Enabled = false;
            btnRandomPlace.Enabled = false;
            btnStartGame.Enabled = false;
        }

        private void BtnPlayerGrid_Click(object sender, EventArgs e)
        {
            if (phase != GamePhase.Placing) return;
            var btn = sender as Button;
            var pt = (Point)btn.Tag;
            int x = pt.X, y = pt.Y;
            int len = ShipLengths[currentShipIndex];
            if (playerBoard.PlaceShip(x, y, len, isHorizontal))
            {
                for (int i = 0; i < len; i++)
                {
                    int cx = x + (isHorizontal ? i : 0);
                    int cy = y + (isHorizontal ? 0 : i);
                    btnPlayerGrid[cx, cy].BackColor = ShipCellColor;
                }
                currentShipIndex++;
                if (currentShipIndex >= ShipLengths.Length)
                {
                    UpdateStatus("Кораблі розміщені. Натисніть 'Почати гру'.");
                    btnStartGame.Enabled = true;
                    for (int i = 0; i < GridSize; i++)
                        for (int j = 0; j < GridSize; j++)
                            btnPlayerGrid[i, j].Click -= BtnPlayerGrid_Click;
                }
                else
                {
                    UpdateStatus($"Розміщення: корабель довжини {ShipLengths[currentShipIndex]}");
                }
            }
            else
            {
                MessageBox.Show("Неправильне розміщення корабля: спробуйте інше місце або орієнтацію.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnComputerGrid_Click(object sender, EventArgs e)
        {
            if (phase != GamePhase.InProgress) return;
            var btn = sender as Button;
            var pt = (Point)btn.Tag;
            int x = pt.X, y = pt.Y;
            var (hit, sunkShip) = computerBoard.ProcessShot(x, y);
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = GridBorderColor;
            if (hit)
            {
                btn.BackColor = HitCellColor;
                btn.Enabled = false;
                if (sunkShip != null)
                    UpdateStatus("Ви потопили корабель комп'ютера!");
                else
                    UpdateStatus("Влучили! Можете зробити ще хід.");
                if (computerBoard.AllShipsSunk())
                {
                    phase = GamePhase.GameOver;
                    MessageBox.Show("Вітаємо! Ви перемогли!", "Кінець гри", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                return;
            }
            else
            {
                btn.BackColor = MissCellColor;
                btn.Enabled = false;
                UpdateStatus("Промах. Хід комп'ютера...");
            }
            ComputerMove();
        }

        private void ComputerMove()
        {
            Point shot;
            while (true)
            {
                if (aiTargets.Count > 0)
                {
                    shot = aiTargets.Dequeue();
                }
                else
                {
                    shot = new Point(rnd.Next(GridSize), rnd.Next(GridSize));
                }
                if (playerBoard.Grid[shot.X, shot.Y] == CellState.Hit || playerBoard.Grid[shot.X, shot.Y] == CellState.Miss)
                    continue;
                break;
            }
            var (hit, sunkShip) = playerBoard.ProcessShot(shot.X, shot.Y);
            var btn = btnPlayerGrid[shot.X, shot.Y];
            btn.UseVisualStyleBackColor = false;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = GridBorderColor;
            if (hit)
            {
                btn.BackColor = HitCellColor;
                UpdateStatus("Комп'ютер влучив у ваш корабель!");
                if (sunkShip == null)
                {
                    var dirs = new Point[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
                    foreach (var d in dirs)
                    {
                        int nx = shot.X + d.X;
                        int ny = shot.Y + d.Y;
                        if (nx >= 0 && nx < GridSize && ny >= 0 && ny < GridSize)
                        {
                            if (playerBoard.Grid[nx, ny] == CellState.Empty || playerBoard.Grid[nx, ny] == CellState.Ship)
                                aiTargets.Enqueue(new Point(nx, ny));
                        }
                    }
                }
                else
                {
                    UpdateStatus("Комп'ютер потопив ваш корабель!");
                    aiTargets.Clear();
                }
                if (playerBoard.AllShipsSunk())
                {
                    phase = GamePhase.GameOver;
                    MessageBox.Show("На жаль, комп'ютер переміг.", "Кінець гри", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ComputerMove();
            }
            else
            {
                btn.BackColor = MissCellColor;
                UpdateStatus("Комп'ютер промахнувся. Ваш хід.");
            }
        }

        private void новаГраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelPlayer.Controls.Clear();
            panelComputer.Controls.Clear();
            var labelsToRemove = this.Controls.OfType<Label>()
                .Where(l => Char.IsLetterOrDigit(l.Text.FirstOrDefault())).ToList();
            foreach (var l in labelsToRemove)
                this.Controls.Remove(l);

            InitializeGame();
            CreateGridButtons();
            btnRotate.Enabled = true;
            btnRandomPlace.Enabled = true;
            btnStartGame.Enabled = false;
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
