using Engine;
using SettingFileReader;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Advanced.Properties;

namespace UI.Advanced
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();
            picXS = new Dictionary<Engine.Point, PictureBox>();
        

        }

        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                moveFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "moves.txt");
                txtMoveFile.Text = moveFile;

                boardFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "board.txt");
                txtBoardFile.Text = boardFile;

            }
        }
        private void ReDraw()
        {
            CheckForIllegalCrossThreadCalls = false;
            foreach (var pt in picXS.Keys)
            {
                if (boardSetting.Mines.Any(m => m == pt))
                {
                    picXS[pt].Image = Resources.MINE;
                }
                else if (boardSetting.ExitPoint == pt)
                {
                    picXS[pt].Image = Resources.Exit;
                }
                else if (boardSetting.StartPoint == pt)
                {
                    switch (boardSetting.Direction)
                    {
                        case Engine.Direction.North:
                            picXS[pt].Image = Resources.TNorth;

                            break;
                        case Engine.Direction.West:
                            picXS[pt].Image = Resources.TWest;
                            break;
                        case Engine.Direction.East:
                            picXS[pt].Image = Resources.Teast;

                            break;
                        case Engine.Direction.South:
                            picXS[pt].Image = Resources.TSouth;

                            break;
                    }
                }
                else
                {
                    picXS[pt].Image = null;
                    picXS[pt].Update();
                }
            }
        }
        IGame game;
        private void CreateBoard(BoardSetting boardSetting)
        {
            for (int i = 0; i < boardSetting.BoardWithd; i++)
            {
                for (int j = 0; j < boardSetting.BoardHeight; j++)
                {
                    Engine.Point pt = new Engine.Point { X = i, Y = j };
                    var ctl = CreateCell(pt);
                    if (boardSetting.Mines.Any(m => m == pt))
                    {
                        ctl.Image = Resources.MINE;
                    }
                    else if (boardSetting.ExitPoint == pt)
                    {
                        ctl.Image = Resources.Exit;
                    }
                    else if (boardSetting.StartPoint == pt)
                    {
                        switch (boardSetting.Direction)
                        {
                            case Engine.Direction.North:
                                ctl.Image = Resources.TNorth;

                                break;
                            case Engine.Direction.West:
                                ctl.Image = Resources.TWest;
                                break;
                            case Engine.Direction.East:
                                ctl.Image = Resources.Teast;

                                break;
                            case Engine.Direction.South:
                                ctl.Image = Resources.TSouth;

                                break;
                        }
                    }
                    panel2.Controls.Add(ctl);
                    ctl.BringToFront();
                }
            }
        }
        private PictureBox CreateCell(Engine.Point pt)
        {
            PictureBox Cell = new PictureBox();
            Cell.Tag = pt;
            Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Cell.Location = new System.Drawing.Point(0, 0);
            Cell.Name = "Cell_" + pt.X.ToString() + "_" + pt.Y.ToString();
            Cell.Size = new System.Drawing.Size(52, 52);
            Cell.TabIndex = 0;
            Cell.TabStop = false;
            Cell.SizeMode = PictureBoxSizeMode.StretchImage;
            Cell.Top = pt.Y * 52;
            Cell.Left = pt.X * 52;
            Cell.BackColor = Color.Transparent;

            picXS.Add(pt, Cell);
            return Cell;
        }

        string moveFile;
        string boardFile;
        private void Button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "*.txt|Moves.txt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Title = "Move File";
            openFileDialog1.FileName = "moves.txt";
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                moveFile = openFileDialog1.FileName;
                txtMoveFile.Text = moveFile;
            }


        }
        private PictureBox GetCellByPoint(Engine.Point pt)
        {
            return picXS[pt];
        }
        Dictionary<Engine.Point, PictureBox> picXS;
        Engine.Point current;
        private void G_TurtleStatusChanged(object sender, TurtleStatusChangedEventArg e)
        {
            CheckForIllegalCrossThreadCalls = false;
            lastState = e.Status;
            var oldPicx = GetCellByPoint(current);
            if (oldPicx != null)
            {
                oldPicx.Image = null;
                oldPicx.Update();
            }
            var picX = GetCellByPoint(e.Position);
            if (picX != null)
            {
                switch (e.Status)
                {
                    case TurtleState.Killed:
                        picX.Image = Resources.Dead;
                        AppendText(Color.Red, "Killed" + Environment.NewLine);
                        break;
                    case TurtleState.Escaped:
                        picX.Image = Resources.Escaped;
                        AppendText(Color.Green, "Escaped" + Environment.NewLine);
                        break;
                    case TurtleState.IsInDanger:

                        switch (e.Direction)
                        {
                            case Engine.Direction.North:
                                picX.Image = Resources.TNorth;

                                break;
                            case Engine.Direction.West:
                                picX.Image = Resources.TWest;
                                break;
                            case Engine.Direction.East:
                                picX.Image = Resources.Teast;

                                break;
                            case Engine.Direction.South:
                                picX.Image = Resources.TSouth;
                                break;
                        }
                        break;

                }
            }
            current = e.Position;
            picX.Update();
            this.Invalidate();
            this.Update();
            this.Refresh();
            Thread.Sleep(500);
        }
        MovesSetting movesSetting;
        BoardSetting boardSetting;
        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                movesSetting = new MovesSetting(moveFile);
                movesSetting.Load();



                boardSetting = new BoardSetting(boardFile);
                boardSetting.Load();
                current = boardSetting.StartPoint;
                CreateBoard(boardSetting);

                this.Width = boardSetting.BoardWithd * 52 + 220;
                this.Height = boardSetting.BoardHeight * 52 + 40 + panel1.Height;
                this.Update();
                game = Engine.Game
                     .Create(boardSetting.BoardWithd, boardSetting.BoardHeight)
                     .LayMine(boardSetting.Mines)
                     .SetExitPoint(boardSetting.ExitPoint)
                     .AddTurtle(boardSetting.StartPoint, boardSetting.Direction, this.G_TurtleStatusChanged);

                ThreadPool.QueueUserWorkItem(s => Play());
            }
            catch (InvalidSettingException ex)
            {
                StringBuilder strb = new StringBuilder();
                strb.AppendLine(ex.Message);
                foreach (var item in ex.Errors)
                {
                    strb.AppendLine(item.ToString());

                }
                ShowError(strb.ToString());

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
        TurtleState lastState;
        private void Play()
        {
            CheckForIllegalCrossThreadCalls = false;
            foreach (var mv in movesSetting.Moves)
            {
                AppendText(Color.Blue, mv + Environment.NewLine.ToUpper());
                ReDraw();
                game.Reset();
                bool Error = false;
                foreach (char chr in mv)
                {
                    if (chr == 'r')
                    {
                        try
                        {
                            game.Rotate();

                        }
                        catch (Exception ex)
                        {
                            ShowError(ex.Message);
                            break;
                        }
                    }
                    else if (chr == 'm')
                    {
                        try
                        {
                            game.Move();
                        }
                        catch (Exception ex)
                        {
                            ShowError(ex.Message);
                            break;
                        }
                    }
                }
                if (lastState == TurtleState.IsInDanger)
                {
                    AppendText(Color.Orange, "In Danger" + Environment.NewLine);

                }


            }
        }

        private void ShowError(string msg)
        {
            
            StringBuilder strb = new StringBuilder();
            strb.AppendLine("***************************");
            strb.AppendLine(msg);
            strb.AppendLine("***************************");
            AppendText(Color.Red, strb.ToString());
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "*.txt|board.txt";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Title = "Board File";
            openFileDialog1.FileName = "Board.txt";
            var result = openFileDialog1.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                boardFile = openFileDialog1.FileName;
                txtBoardFile.Text = boardFile;
            }


        }


        void AppendText(Color color, string text)
        {
            int start = this.richTextBox1.TextLength;
            this.richTextBox1.AppendText(text);
            int end = this.richTextBox1.TextLength;

            this.richTextBox1.Select(start, end - start);
            {
                this.richTextBox1.SelectionColor = color;
            }
            this.richTextBox1.SelectionLength = 0;
        }


    }
}
