using System.Drawing;
using System.Windows.Forms;
using ElectricFieldVis;

namespace UPG_SP_2024
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			drawingPanel = new DrawingPanel();
			menuStrip1 = new MenuStrip();
			editorToolsToolStripMenuItem = new ToolStripMenuItem();
			deleteObjectToolStripMenuItem = new ToolStripMenuItem();
			addChargeToolStripMenuItem = new ToolStripMenuItem();
			addProbeToolStripMenuItem = new ToolStripMenuItem();
			addStaticProbeToolStripMenuItem = new ToolStripMenuItem();
			controlsToolStripMenuItem = new ToolStripMenuItem();
			showGridToolStripMenuItem = new ToolStripMenuItem();
			showArrowsToolStripMenuItem = new ToolStripMenuItem();
			showElectricFieldMapToolStripMenuItem = new ToolStripMenuItem();
			showProbesToolStripMenuItem = new ToolStripMenuItem();
			showStaticProbesToolStripMenuItem = new ToolStripMenuItem();
			showGraphToolStripMenuItem = new ToolStripMenuItem();
			showChargesToolStripMenuItem = new ToolStripMenuItem();
			showLegendToolStripMenuItem = new ToolStripMenuItem();
			loadToolStripMenuItem = new ToolStripMenuItem();
			scenario0ToolStripMenuItem = new ToolStripMenuItem();
			scenario1ToolStripMenuItem = new ToolStripMenuItem();
			scenario2ToolStripMenuItem = new ToolStripMenuItem();
			scenario3ToolStripMenuItem = new ToolStripMenuItem();
			scenario4ToolStripMenuItem = new ToolStripMenuItem();
			loadCustomScenarioToolStripMenuItem = new ToolStripMenuItem();
			saveToolStripMenuItem = new ToolStripMenuItem();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// drawingPanel
			// 
			drawingPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			drawingPanel.BackColor = SystemColors.Control;
			drawingPanel.Dock = DockStyle.Fill;
			drawingPanel.Location = new Point(0, 28);
			drawingPanel.Name = "drawingPanel";
			drawingPanel.Size = new Size(982, 725);
			drawingPanel.TabIndex = 0;
			drawingPanel.Paint += drawingPanel_Paint;
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new Size(20, 20);
			menuStrip1.Items.AddRange(new ToolStripItem[] { editorToolsToolStripMenuItem, controlsToolStripMenuItem, loadToolStripMenuItem, saveToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(982, 28);
			menuStrip1.TabIndex = 3;
			menuStrip1.Text = "menuStrip1";
			// 
			// editorToolsToolStripMenuItem
			// 
			editorToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { deleteObjectToolStripMenuItem, addChargeToolStripMenuItem, addProbeToolStripMenuItem, addStaticProbeToolStripMenuItem });
			editorToolsToolStripMenuItem.Name = "editorToolsToolStripMenuItem";
			editorToolsToolStripMenuItem.Size = new Size(102, 24);
			editorToolsToolStripMenuItem.Text = "Editor Tools";
			// 
			// deleteObjectToolStripMenuItem
			// 
			deleteObjectToolStripMenuItem.Name = "deleteObjectToolStripMenuItem";
			deleteObjectToolStripMenuItem.Size = new Size(204, 26);
			deleteObjectToolStripMenuItem.Text = "Delete Object";
			deleteObjectToolStripMenuItem.Click += MyEventHandler.EnableDeleteTool;
			// 
			// addChargeToolStripMenuItem
			// 
			addChargeToolStripMenuItem.Name = "addChargeToolStripMenuItem";
			addChargeToolStripMenuItem.Size = new Size(204, 26);
			addChargeToolStripMenuItem.Text = "Add Charge";
			
			// 
			// addProbeToolStripMenuItem
			// 
			addProbeToolStripMenuItem.Name = "addProbeToolStripMenuItem";
			addProbeToolStripMenuItem.Size = new Size(204, 26);
			addProbeToolStripMenuItem.Text = "Add Probe";
			
			// 
			// addStaticProbeToolStripMenuItem
			// 
			addStaticProbeToolStripMenuItem.Name = "addStaticProbeToolStripMenuItem";
			addStaticProbeToolStripMenuItem.Size = new Size(204, 26);
			addStaticProbeToolStripMenuItem.Text = "Add Static Probe";
			
			// 
			// controlsToolStripMenuItem
			// 
			controlsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showGridToolStripMenuItem, showArrowsToolStripMenuItem, showElectricFieldMapToolStripMenuItem, showProbesToolStripMenuItem, showStaticProbesToolStripMenuItem, showChargesToolStripMenuItem, showLegendToolStripMenuItem, showGraphToolStripMenuItem });
			controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
			controlsToolStripMenuItem.Size = new Size(78, 24);
			controlsToolStripMenuItem.Text = "Controls";
			// 
			// showGridToolStripMenuItem
			// 
			showGridToolStripMenuItem.Checked = true;
			showGridToolStripMenuItem.CheckState = CheckState.Checked;
			showGridToolStripMenuItem.Name = "showGridToolStripMenuItem";
			showGridToolStripMenuItem.Size = new Size(248, 26);
			showGridToolStripMenuItem.Text = "Show grid";
			showGridToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// showArrowsToolStripMenuItem
			// 
			showArrowsToolStripMenuItem.Checked = true;
			showArrowsToolStripMenuItem.CheckState = CheckState.Checked;
			showArrowsToolStripMenuItem.Name = "showArrowsToolStripMenuItem";
			showArrowsToolStripMenuItem.Size = new Size(248, 26);
			showArrowsToolStripMenuItem.Text = "Show arrows";
			showArrowsToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// showElectricFieldMapToolStripMenuItem
			// 
			showElectricFieldMapToolStripMenuItem.Checked = true;
			showElectricFieldMapToolStripMenuItem.CheckState = CheckState.Checked;
			showElectricFieldMapToolStripMenuItem.Name = "showElectricFieldMapToolStripMenuItem";
			showElectricFieldMapToolStripMenuItem.Size = new Size(248, 26);
			showElectricFieldMapToolStripMenuItem.Text = "Show electric field map";
			showElectricFieldMapToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// showProbesToolStripMenuItem
			// 
			showProbesToolStripMenuItem.Checked = true;
			showProbesToolStripMenuItem.CheckState = CheckState.Checked;
			showProbesToolStripMenuItem.Name = "showProbesToolStripMenuItem";
			showProbesToolStripMenuItem.Size = new Size(248, 26);
			showProbesToolStripMenuItem.Text = "Show probes";
			showProbesToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// showStaticProbesToolStripMenuItem
			// 
			showStaticProbesToolStripMenuItem.Checked = true;
			showStaticProbesToolStripMenuItem.CheckState = CheckState.Checked;
			showStaticProbesToolStripMenuItem.Name = "showStaticProbesToolStripMenuItem";
			showStaticProbesToolStripMenuItem.Size = new Size(248, 26);
			showStaticProbesToolStripMenuItem.Text = "Show static probes";
			showStaticProbesToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// showGraphToolStripMenuItem
			// 
			showGraphToolStripMenuItem.Name = "showGraphToolStripMenuItem";
			showGraphToolStripMenuItem.Size = new Size(248, 26);
			showGraphToolStripMenuItem.Text = "Show graph";
			showGraphToolStripMenuItem.Click += MyEventHandler.ShowGraph;
			// 
			// showChargesToolStripMenuItem
			// 
			showChargesToolStripMenuItem.Checked = true;
			showChargesToolStripMenuItem.CheckState = CheckState.Checked;
			showChargesToolStripMenuItem.Name = "showChargesToolStripMenuItem";
			showChargesToolStripMenuItem.Size = new Size(248, 26);
			showChargesToolStripMenuItem.Text = "Show charges";
			showChargesToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// showLegendToolStripMenuItem
			// 
			showLegendToolStripMenuItem.Checked = true;
			showLegendToolStripMenuItem.CheckState = CheckState.Checked;
			showLegendToolStripMenuItem.Name = "showLegendToolStripMenuItem";
			showLegendToolStripMenuItem.Size = new Size(248, 26);
			showLegendToolStripMenuItem.Text = "Show legend";
			showLegendToolStripMenuItem.Click += MyEventHandler.Controls_HandleEvent;
			// 
			// loadToolStripMenuItem
			// 
			loadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { scenario0ToolStripMenuItem, scenario1ToolStripMenuItem, scenario2ToolStripMenuItem, scenario3ToolStripMenuItem, scenario4ToolStripMenuItem, loadCustomScenarioToolStripMenuItem });
			loadToolStripMenuItem.Name = "loadToolStripMenuItem";
			loadToolStripMenuItem.Size = new Size(56, 24);
			loadToolStripMenuItem.Text = "Load";
			// 
			// scenario0ToolStripMenuItem
			// 
			scenario0ToolStripMenuItem.Name = "scenario0ToolStripMenuItem";
			scenario0ToolStripMenuItem.Size = new Size(240, 26);
			scenario0ToolStripMenuItem.Text = "Scenario 0";
			scenario0ToolStripMenuItem.Click += MyEventHandler.Load_HandleEvent;
			// 
			// scenario1ToolStripMenuItem
			// 
			scenario1ToolStripMenuItem.Name = "scenario1ToolStripMenuItem";
			scenario1ToolStripMenuItem.Size = new Size(240, 26);
			scenario1ToolStripMenuItem.Text = "Scenario 1";
			scenario1ToolStripMenuItem.Click += MyEventHandler.Load_HandleEvent;
			// 
			// scenario2ToolStripMenuItem
			// 
			scenario2ToolStripMenuItem.Name = "scenario2ToolStripMenuItem";
			scenario2ToolStripMenuItem.Size = new Size(240, 26);
			scenario2ToolStripMenuItem.Text = "Scenario 2";
			scenario2ToolStripMenuItem.Click += MyEventHandler.Load_HandleEvent;
			// 
			// scenario3ToolStripMenuItem
			// 
			scenario3ToolStripMenuItem.Name = "scenario3ToolStripMenuItem";
			scenario3ToolStripMenuItem.Size = new Size(240, 26);
			scenario3ToolStripMenuItem.Text = "Scenario 3";
			scenario3ToolStripMenuItem.Click += MyEventHandler.Load_HandleEvent;
			// 
			// scenario4ToolStripMenuItem
			// 
			scenario4ToolStripMenuItem.Name = "scenario4ToolStripMenuItem";
			scenario4ToolStripMenuItem.Size = new Size(240, 26);
			scenario4ToolStripMenuItem.Text = "Scenario 4";
			scenario4ToolStripMenuItem.Click += MyEventHandler.Load_HandleEvent;
			// 
			// loadCustomScenarioToolStripMenuItem
			// 
			loadCustomScenarioToolStripMenuItem.Name = "loadCustomScenarioToolStripMenuItem";
			loadCustomScenarioToolStripMenuItem.Size = new Size(240, 26);
			loadCustomScenarioToolStripMenuItem.Text = "Load Custom Scenario";
			loadCustomScenarioToolStripMenuItem.Click += MyEventHandler.Load_HandleEvent;
			// 
			// saveToolStripMenuItem
			// 
			saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			saveToolStripMenuItem.Size = new Size(54, 24);
			saveToolStripMenuItem.Text = "Save";
			saveToolStripMenuItem.Click += MyEventHandler.Save_HandleEvent;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(982, 753);
			Controls.Add(drawingPanel);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
			Margin = new Padding(3, 4, 3, 4);
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "A23B0293P - Semestrální práce KIV/UPG 2024/2025";
			Load += MainForm_Load;
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}


		#endregion

		private DrawingPanel drawingPanel;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem editorToolsToolStripMenuItem;
		private ToolStripMenuItem deleteObjectToolStripMenuItem;
		private ToolStripMenuItem addChargeToolStripMenuItem;
		private ToolStripMenuItem addProbeToolStripMenuItem;
		private ToolStripMenuItem addStaticProbeToolStripMenuItem;
		private ToolStripMenuItem controlsToolStripMenuItem;
		private ToolStripMenuItem showGridToolStripMenuItem;
		private ToolStripMenuItem showArrowsToolStripMenuItem;
		private ToolStripMenuItem showElectricFieldMapToolStripMenuItem;
		private ToolStripMenuItem showProbesToolStripMenuItem;
		private ToolStripMenuItem showStaticProbesToolStripMenuItem;
		private ToolStripMenuItem showGraphToolStripMenuItem;
		private Button button1;
		private ToolStripMenuItem loadToolStripMenuItem;
		private ToolStripMenuItem scenario0ToolStripMenuItem;
		private ToolStripMenuItem scenario1ToolStripMenuItem;
		private ToolStripMenuItem scenario2ToolStripMenuItem;
		private ToolStripMenuItem scenario3ToolStripMenuItem;
		private ToolStripMenuItem scenario4ToolStripMenuItem;
		private ToolStripMenuItem loadCustomScenarioToolStripMenuItem;
		private ToolStripMenuItem showChargesToolStripMenuItem;
		private ToolStripMenuItem showLegendToolStripMenuItem;
		private ToolStripMenuItem saveToolStripMenuItem;
	}
}
