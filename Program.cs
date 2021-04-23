using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public partial class Program : Form
    {
        private TableLayoutPanel TableLayoutPanel;
        private static string fFileNameInput;
        private static string fMethodInput;

        public Program()
        {
            InitializeComponent();
            EditTableLayout();
        }
        public static void Main(string[] args)
        {
            fFileNameInput = args[0];
            fMethodInput = args[1];

            Console.WriteLine("GUI or Console? ");

            string lResp = Console.ReadLine();

            // instantiate file reader
            FileContent lFR = new FileContent(fFileNameInput);


            // create environment object. NavigationRoute (takes map dim and location of empty cells)
            NavigationRoute lNR = new NavigationRoute(lFR.MapDim, lFR.EmptyCellList, lFR.GoalCoordList);

            // create Robot object. This will take in its init position, goal state and the NavigationRoute
            NavigationPlan lNP = new NavigationPlan(lFR.GoalCoordList);

            // the robot entity will be responsible for executing the search method. 
            RobotEntity lRobot = new RobotEntity(lNP, lNR, lFR.InitPos);

            if (lResp.ToUpper() == "GUI")
            {
                Application.Run(new Program());
            }
            else
            {
                // output: filename method number_of_nodes
                //          path outputs when search is executed by robotEntity. 
                Console.WriteLine($"{fFileNameInput} {fMethodInput} {lFR.MapDim.Width * lFR.MapDim.Height}");

                // execute the search by sending the robotEntity and search string to method below. 
                executeRobotNavForUser(lRobot, fMethodInput);
            }

        }

        private static void executeRobotNavForUser(RobotEntity aRobot, string aSearchStrategy)
        {
            if (aSearchStrategy.ToUpper() == "BFS")
            {
                aRobot.executeBFS();
            }
            if (aSearchStrategy.ToUpper() == "DFS")
            {
                aRobot.executeDFS();
            }
            if (aSearchStrategy.ToUpper() == "GBFS")
            {
                aRobot.executeGBFS();
            }
            if (aSearchStrategy.ToUpper() == "ASTAR")
            {
                aRobot.executeASTAR();
            }
            if (aSearchStrategy.ToUpper() == "DLS")
            {
                aRobot.executeDLS();
            }
            if (aSearchStrategy.ToUpper() == "IDA")
            {
                aRobot.executeIDA();
            }

        }
        private void EditTableLayout()
        {

            var cmdArgs = Environment.GetCommandLineArgs();

            fFileNameInput = cmdArgs[1];
            fMethodInput = cmdArgs[2];

            FileContent lFR = new FileContent(cmdArgs[1]);

            NavigationRoute lNR = new NavigationRoute(lFR.MapDim, lFR.EmptyCellList, lFR.GoalCoordList);

            // create Robot object. This will take in its init position, goal state and the NavigationRoute
            NavigationPlan lNP = new NavigationPlan(lFR.GoalCoordList);

            // the robot entity will be responsible for executing the search method. 
            RobotEntity lRobot = new RobotEntity(lNP, lNR, lFR.InitPos);

            Console.WriteLine($"{fFileNameInput} {fMethodInput} ");
            executeRobotNavForUser(lRobot, fMethodInput);

            TableLayoutPanel.RowCount = lFR.MapDim.Height;
            TableLayoutPanel.ColumnCount = lFR.MapDim.Width;
            TableLayoutPanel.RowStyles.Clear();
            TableLayoutPanel.ColumnStyles.Clear();

            for (int i = 1; i <= TableLayoutPanel.RowCount; i++)
            {
                TableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 1));
            }
            for (int i = 1; i <= TableLayoutPanel.ColumnCount; i++)
            {
                TableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1));
            }

            TableLayoutPanel.CellPaint += new TableLayoutCellPaintEventHandler((sender, e) => TableLayoutPanel_CellPaint(sender, e,
                lNR.EmptyCellList, lFR.InitPos, lFR.GoalCoordList));

            lNP.FinalCoordinateList.Reverse();
            int lCounter = 1;

            foreach (FinalPathState coord in lNP.FinalCoordinateList)
            {
                Label lLabel = new Label
                {
                    Text = $"{lCounter}.  ({coord.X},{coord.Y})",
                    BackColor = Color.RoyalBlue,
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", Font.Size, FontStyle.Bold),

                };

                TableLayoutPanel.CellPaint += new TableLayoutCellPaintEventHandler((sender, ev) => drawPath(sender, ev, coord));
                TableLayoutPanel.Controls.Add(lLabel, coord.X, coord.Y);
                
                lCounter++;
            }

            
            
        }

        void drawPath(object sender, TableLayoutCellPaintEventArgs e, FinalPathState aCoord)
        {
         
            if (e.Column == aCoord.X && e.Row == aCoord.Y)
            {
                 e.Graphics.FillRectangle(Brushes.RoyalBlue, e.CellBounds);
            }
           

        }
        public void InitializeComponent()
        {
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.AutoSize = true;
            this.TableLayoutPanel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.TableLayoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.TableLayoutPanel.ColumnCount = 1;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 1;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TableLayoutPanel.Size = new System.Drawing.Size(1000, 390);
            this.TableLayoutPanel.TabIndex = 0;
            this.TableLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.TableLayoutPanel_Paint);
            // 
            // Program
            // 
            this.ClientSize = new System.Drawing.Size(1000, 390);
            this.Controls.Add(this.TableLayoutPanel);
            this.Name = "Program";
            this.Text = "Robot Navigation";
            this.Load += new System.EventHandler(this.Program_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Program_Load(object sender, EventArgs e)
        {
            
        }

        private void TableLayoutPanel_CellPaint(object sender, TableLayoutCellPaintEventArgs e, List<Cell> emptycells, 
                                                                          InitialStateData initPos, List<GoalStateData> goalStates)
        {
            foreach (Cell cell in emptycells)
            {
                if(e.Column == cell.X && e.Row == cell.Y)
                {
                    e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
                }
            }
            if (e.Column == initPos.X && e.Row == initPos.Y)
            {
                e.Graphics.FillRectangle(Brushes.Red, e.CellBounds);
            }

            foreach(GoalStateData g in goalStates)
            {
                if (e.Column == g.X && e.Row == g.Y)
                {
                    e.Graphics.FillRectangle(Brushes.Green, e.CellBounds);
                }
            }
        }

        private void TableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
