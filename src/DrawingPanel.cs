using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using ElectricFieldVis;
using Newtonsoft.Json;


namespace UPG_SP_2024
{

	/// <summary>
	/// The main panel with the custom visualization
	/// </summary>
	public class DrawingPanel : Panel
	{
		private bool _firstRender = true;
	    private readonly int _mStartTime;
		public string scenario = String.Empty;
	    public string spacing = String.Empty;
	    private float LastDrawTime = 0;
		public float spaceX = 1 / 4f;
	    public float spaceY = 1 / 4f;

		/// <summary>Initializes a new instance of the <see cref="DrawingPanel" /> class.</summary>
		public DrawingPanel()
        {
            DoubleBuffered = true;
			var timer = new System.Windows.Forms.Timer();
			timer.Tick += Timer_Tick;
			timer.Interval = 5;
			
			_mStartTime = Environment.TickCount;
			timer.Start();
			this.MouseDown += MyEventHandler.DrawingPanel_MouseDown;
			this.MouseMove += MyEventHandler.DrawingPanel_MouseMove;
			this.MouseUp += MyEventHandler.DrawingPanel_MouseUp;
			this.MouseWheel += MyEventHandler.DrawingPanel_MouseScroll;

			Painter.Graph = new Graph();
		}


		private void Timer_Tick(object? sender, EventArgs e)
		{
			this.Invalidate();
		}


		/// <summary>TODO: Custom visualization code comes into this method</summary>
		/// <remarks>Raises the <see cref="E:System.Windows.Forms.Control.Paint">Paint</see> event.</remarks>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs">PaintEventArgs</see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if(Painter.Graph.IsDisposed) Painter.Graph = new Graph();
			string[] arguments = Environment.GetCommandLineArgs();

			Regex rgx = new Regex("-g\\d+x\\d+");
			if (arguments.Length > 2 )
			{
				scenario = arguments[1];
				if (rgx.IsMatch(arguments[2])) spacing = arguments[2];
			}
			else if (arguments.Length > 1)
			{
				if (rgx.IsMatch(arguments[1]))
				{
					spacing = arguments[1];
					scenario = "0";
				}
				else scenario = arguments[1];
			}
			else scenario = "0";


			if (spacing != String.Empty )
			{
				spacing = spacing.Replace("-g", "");
				string[] spaces = spacing.Split("x");
				spaceX = Convert.ToInt32(spaces[0]);
				spaceY = Convert.ToInt32(spaces[1]);
				spaceX /= Painter.Scale;
				spaceY /= Painter.Scale;

			}
			var g = e.Graphics;
			g.TranslateTransform(this.Width / 2f, this.Height / 2f);
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			g.SmoothingMode = SmoothingMode.HighQuality;
			Painter.G = g;
			Painter.Bounds = g.ClipBounds;
			if (_firstRender)
			{
				Painter.Load(scenario);
			}
			Painter.SetNeededSizeOfField(Math.Min(this.Width, this.Height));
			Painter.Width = this.Width;
			Painter.Height = this.Height;
			float elapsed = (Environment.TickCount - _mStartTime) / 1000f;
			if (!_firstRender)
			{
				Painter.Draw(spaceX,spaceY);
			}
			elapsed = (Environment.TickCount - _mStartTime) / 1000f;
			Painter.TimeNow = elapsed;
			if (Painter.Scenario.IsScenarioStatic == false)
			{
				Painter.ChangeSize(elapsed);
				Painter.IntesitiesOfEf = null;
				Painter.Map = null;
			}
			//doesnt work


			Painter.Scenario.Probe.MoveInCircle(elapsed);
			
			
			//TODO: Add custom paint code here
			if(LastDrawTime == 0 || elapsed-LastDrawTime>0.25) LastDrawTime = elapsed;
			_firstRender = false;
			// Calling the base class OnPaint
			base.OnPaint(e);
			
        }


		/// <summary>
		/// Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call <span class="keyword">base.onResize</span> to ensure that the event is fired for external listeners.
		/// </summary>
		/// <param name="eventargs">An <see cref="T:System.EventArgs">EventArgs</see> that contains the event data.</param>
		protected override void OnResize(EventArgs eventargs)
        {
            this.Invalidate();  //ensure repaint

			base.OnResize(eventargs);
			Painter.IntesitiesOfEf = null;
			Painter.Map = null;
			Painter.Legend = null;

        }

		
    }
}
