using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ElectricFieldVis
{
	internal class StaticChargeSource : Entity
	{
		/// <summary>
		/// Parameter representing the strength of Electrostatic field in Coulombs. 
		/// </summary>
		public float StrengthOfEf { get; private set; }

		public GraphicsPath GP { get; private set; }
		/// <summary>
		/// Constructor of the class.
		/// </summary>
		/// 
		/// <param name="charge">charge of the object False=negative, True=positive.</param>
		/// <param name="center">position of center of the object on the coordinates system.</param>
		/// <param name="strengthOfEf">Strength of electrostatic field produced by the object.</param>
		public StaticChargeSource(PointF? center = null, 
			 float strengthOfEf = 1f)
		{
			this.StrengthOfEf = strengthOfEf;
			this.Radius = MathF.Abs(this.StrengthOfEf) > 4 ? 1/2f : MathF.Abs(this.StrengthOfEf) / 8;
			this.Color = this.StrengthOfEf>=0 ? ColorStyle.RedObject : ColorStyle.BlueObject;
			this.Center = center ?? new PointF(0f,0f);
			this.Position = new PointF(this.Center.X - this.Radius, this.Center.Y + this.Radius);
			(this.ColorSet, this.ColorBoundaries) = ColorEnumerable.GetBlend(this.Color);
			GetGraphicsPath();
		}
		/// <summary>
		///	Changes strength of electric field based on time
		/// </summary>
		/// <param name="elapsed">actual time</param>
		public void ChangeChargeSize(float elapsed)
		{
			this.StrengthOfEf = 1 + 0.5f * MathF.Sin((MathF.PI / 2) * elapsed);
			this.Radius = MathF.Abs(this.StrengthOfEf) > 4 ? 1 / 2f : MathF.Abs(this.StrengthOfEf)/8;
			this.Position = new PointF(this.Center.X - this.Radius, this.Center.Y + this.Radius);
			GetGraphicsPath();
			Painter.IntesitiesOfEf = null;
			Painter.Map = null;
		}
		/// <summary>
		/// Changes strength of electric field based on mouse wheel.
		/// </summary>
		/// <param name="x">count of mouse wheel turns</param>
		public void ChangeChargeSize(int x)
		{
			this.StrengthOfEf += x * 0.1f;
			this.Radius = MathF.Abs(this.StrengthOfEf) > 4 ? 1/2f :MathF.Abs(this.StrengthOfEf) < 1? 3/16f : MathF.Abs(this.StrengthOfEf)/8  ;
			if (this.Radius < 3 / 16f)
			{
				this.Radius = 3 / 16f;
			}
			this.Position = new PointF(this.Center.X - this.Radius, this.Center.Y + this.Radius);
			this.Color = this.StrengthOfEf >= 0 ? ColorStyle.RedObject : ColorStyle.BlueObject;
			(this.ColorSet, this.ColorBoundaries) = ColorEnumerable.GetBlend(this.Color);
			GetGraphicsPath();
			Painter.IntesitiesOfEf = null;
			Painter.Map = null;
		}

		/// <summary>
		/// creates graphics path for this object.
		/// </summary>
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
		/// <summary>
		/// Changes location of charge
		/// </summary>
		/// <param name="x">x value</param>
		/// <param name="y">y value</param>
		public void ChangePosition(float x, float y)
		{
			this.Center.X = x;
			this.Center.Y = y;
			this.Position.X = Center.X - this.Radius;
			this.Position.Y = Center.Y + this.Radius;
			GetGraphicsPath();
			Painter.IntesitiesOfEf = null;
			Painter.Map = null;

		}

	}
}
