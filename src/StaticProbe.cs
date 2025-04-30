using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace ElectricFieldVis
{
	internal class StaticProbe : Entity
	{
		public List<Sample> Data = new();
		public GraphicsPath GP {get; set; }
		public SKColor GraphColor { get; set; }
		public StaticProbe(PointF center,SKColor grColor, float radius = 1, ColorStyle color = ColorStyle.SilverObject)
		{
			this.Center = center;
			this.Radius = (radius * 1.5f) / 16;
			this.Position = new PointF(this.Center.X - this.Radius, this.Center.Y + this.Radius);
			this.Color = color;
			this.GraphColor=grColor;
			(this.ColorSet, this.ColorBoundaries) = ColorEnumerable.GetBlend(this.Color);
			GetGraphicsPath();
		}

		public void ChangePosition(PointF newCenter)
		{
			this.Center = newCenter;
			this.Position = new PointF(this.Center.X - this.Radius, this.Center.Y + this.Radius);
			GetGraphicsPath();
		}
		public void GetGraphicsPath()
		{
			//draws the StaticChargeSource.
			GraphicsPath path = new GraphicsPath();
			RectangleF rect = new RectangleF(this.Position.X,
				this.Position.Y,
				this.Radius * 2,
				-this.Radius * 2);
			path.AddEllipse(rect);
			path.CloseFigure();
			this.GP = path;
		}
	}
}
