using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldVis
{
	internal class Probe : Entity
	{
		public float OrbitalRadius { get; set; }
		private const float Speed = MathF.PI /6;
		public Probe(PointF? center = null, float radius = 1.5f/16, ColorStyle color = ColorStyle.SilverObject)
		{
			this.Center = center ?? new PointF(1,0);
			this.Position = new PointF(Center.X - radius, Center.Y + radius);
			this.OrbitalRadius = MathF.Sqrt(MathF.Pow(Center.X, 2) + MathF.Pow(Center.Y, 2));
			this.Radius = radius;
			this.Color = color;
			(this.ColorSet, this.ColorBoundaries) = ColorEnumerable.GetBlend(this.Color);
		}

		public async Task MoveInCircle(float elapsed)
		{
			var newPosition = new PointF(OrbitalRadius * MathF.Cos(-Speed * elapsed), OrbitalRadius * MathF.Sin(-Speed * elapsed));
			this.Center = newPosition;
			this.Position.X = newPosition.X - this.Radius;
			this.Position.Y = newPosition.Y + this.Radius;
			await Task.CompletedTask;

		}
	}
}
