using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectricFieldVis
{
	public static class MyEventHandler
	{
		private static bool _chargeClick;
		private static PointF _clickPoint;
		private static int _dragItemIndex = -1;
		private static PointF _diffPoint;
		private static bool _probeMove = false;
		public const int WHEEL_DELTA = 120;
		private static bool _delete = false;
		public static void DrawingPanel_MouseUp(object? sender, MouseEventArgs e)
		{
			_clickPoint = Painter.TransformClickCoords(e.X, e.Y);
			if (!_chargeClick && !_probeMove && !_delete)
			{
				Random rnd = new Random();
				SKColor color = new SKColor((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));
				Painter.Scenario.StaticProbes.Add(new StaticProbe(_clickPoint,color));
				Painter.Graph.StartTime = Painter.TimeNow;
				Painter.Graph.Show();
			}

			_probeMove = false;
			_chargeClick = false;
			_dragItemIndex = -1;
		}

		public static void DrawingPanel_MouseDown(object? sender, MouseEventArgs e)
		{
			_clickPoint = Painter.TransformClickCoords(e.X, e.Y);
			for (int i = 0; i < Painter.Scenario.StaticProbes.Count; i++)
			{

				RectangleF bounds = Painter.Scenario.StaticProbes[i].GP.GetBounds();
				if (bounds.Contains(_clickPoint))
				{
					if (_delete)
					{
						Painter.Scenario.StaticProbes.Remove(Painter.Scenario.StaticProbes[i]);
						if (Painter.Scenario.StaticProbes.Count == 0)
						{
							Painter.Graph.Close();
						}
						return;
					}
					_probeMove = true;
					_diffPoint = new PointF(_clickPoint.X - Painter.Scenario.StaticProbes[i].Center.X,
						_clickPoint.Y - Painter.Scenario.StaticProbes[i].Center.Y);
					_dragItemIndex = i;
					return;
				}
			}

			for (int i = 0; i < Painter.Scenario.StaticChargeSourceList.Count; i++)
			{
				//could not use GraphicsPath.IsVisible, because it was buggy. I do not know why. 
				RectangleF bounds = Painter.Scenario.StaticChargeSourceList[i].GP.GetBounds();
				if (bounds.Contains(_clickPoint))
				{
					if (_delete)
					{
						Painter.Scenario.StaticChargeSourceList.Remove(Painter.Scenario.StaticChargeSourceList[i]);
						Painter.Map = null;
						Painter.IntesitiesOfEf = null;
						return;
					}
					_dragItemIndex = i;
					_chargeClick = true;
					_diffPoint = new PointF(_clickPoint.X - Painter.Scenario.StaticChargeSourceList[_dragItemIndex].Center.X,
						_clickPoint.Y - Painter.Scenario.StaticChargeSourceList[_dragItemIndex].Center.Y);
					return;
				}
			}
		}

		public static void DrawingPanel_MouseMove(object? sender, MouseEventArgs e)
		{
			if (_chargeClick && _dragItemIndex != -1&& _probeMove == false)
			{
				PointF releasePoint = Painter.TransformClickCoords(e.X, e.Y);
				releasePoint.X -= _diffPoint.X;
				releasePoint.Y -= _diffPoint.Y;
				Painter.Scenario.StaticChargeSourceList[_dragItemIndex].ChangePosition(releasePoint.X, releasePoint.Y);
			}
			else if (_probeMove)
			{
				PointF releasePoint = Painter.TransformClickCoords(e.X, e.Y);
				releasePoint.X -= _diffPoint.X;
				releasePoint.Y -= _diffPoint.Y;
				Painter.Scenario.StaticProbes[_dragItemIndex].ChangePosition(new PointF(releasePoint.X, releasePoint.Y));
			}
		}

		public static void DrawingPanel_MouseScroll(object? sender, MouseEventArgs e)
		{
			PointF scrollPoint = Painter.TransformClickCoords(e.X, e.Y);
			for (int i = 0; i < Painter.Scenario.StaticChargeSourceList.Count; i++)
			{
				//could not use GraphicsPath.IsVisible, because it was buggy. I do not know why. 
				RectangleF bounds = Painter.Scenario.StaticChargeSourceList[i].GP.GetBounds();
				if (bounds.Contains(scrollPoint))
				{
					Painter.Scenario.StaticChargeSourceList[i].ChangeChargeSize(e.Delta / WHEEL_DELTA);

				}
			}
		}

		public static void Controls_HandleEvent(object sender, EventArgs e)
		{
			ToolStripMenuItem ts = (ToolStripMenuItem)sender;
			string name = ts.ToString();
			ts.Checked = !ts.Checked;
			if (name == "Show probes")
			{
				Painter.ShowProbes = !Painter.ShowProbes;
				
			}
			else if (name == "Show grid")
			{
				Painter.ShowGrid = !Painter.ShowGrid;

			}
			else if (name == "Show arrows")
			{
				Painter.ShowArrows = !Painter.ShowArrows;

			}
			else if (name == "Show electric field map")
			{
				Painter.ShowEFMap = !Painter.ShowEFMap;

			}
			else if (name == "Show static probes")
			{
				Painter.ShowStaticProbes = !Painter.ShowStaticProbes;

			}
			//else if (name == "Show graph") Painter.ShowGraph = !Painter.ShowGraph; 
			else if (name == "Show charges")
			{
				Painter.ShowCharges = !Painter.ShowCharges;

			}
			else if (name == "Show legend")
			{
				Painter.ShowLegend = !Painter.ShowLegend;

			}
		}

		public static void Load_HandleEvent(object sender, EventArgs e)
		{
			ToolStripMenuItem ts = (ToolStripMenuItem)sender;
			string name = ts.ToString();
			if (name == "Scenario 0") Painter.Load("0");
			else if (name == "Scenario 1") Painter.Load("1");
			else if (name == "Scenario 2") Painter.Load("2");
			else if (name == "Scenario 3") Painter.Load("3");
			else if (name == "Scenario 4") Painter.Load("4");
			else if (name == "Load Custom Scenario")
			{
				string file = String.Empty;
				var t = new Thread((ThreadStart)(() =>
				{
					OpenFileDialog ofd = new OpenFileDialog();
					ofd.InitialDirectory = "c:\\";
					ofd.Filter = "json files (*.json)|*.json";
					ofd.RestoreDirectory = true;
					if (ofd.ShowDialog() == DialogResult.OK)
					{
						file = ofd.FileName;
					}
				}));
				t.SetApartmentState(ApartmentState.STA);
				t.Start();
				t.Join();
				if (file ==String.Empty) return;
				Painter.Load(file);
			}

			Painter.Map = null;
			Painter.IntesitiesOfEf = null;

		}
		public static void Save_HandleEvent(object sender, EventArgs e)
		{

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(Painter.Scenario);
			var t = new Thread((ThreadStart)(() =>
			{
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.InitialDirectory = "c:\\";
				sfd.Filter = "json files (*.json)|*.json";
				sfd.RestoreDirectory = true;
				if (sfd.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllText(sfd.FileName, json);
				}
			}));
			t.SetApartmentState(ApartmentState.STA);
			t.Start();
			t.Join();
			

		}

		public static void EnableDeleteTool(object sender, EventArgs e)
		{
			ToolStripMenuItem ts = (ToolStripMenuItem)sender;
			ts.Checked = !ts.Checked;
			_delete = !_delete;
		}

		public static void ShowGraph(object? sender, EventArgs e)
		{
			FormCollection fm = Application.OpenForms;

			foreach (Form form in fm)
			{
				if (form.Name == "Graph") return;
				
			}
			Painter.Graph.Show();
		}
	}
}
