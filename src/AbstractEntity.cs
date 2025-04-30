using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldVis
{
	internal abstract class Entity
	{
		/// <summary>
		/// Center point of object.
		/// </summary>
		public PointF Center;

		/// <summary>
		/// Size of visualized object.
		/// </summary>
		public float Radius;

		/// <summary>
		/// Position of the object(starting point for drawing).
		/// </summary>
		public PointF Position;

		/// <summary>
		/// Gets the color style of the static charge source.
		/// </summary>
		public ColorStyle Color;

		/// <summary>
		/// Set of colors for shading.
		/// </summary>
		public Color[] ColorSet;

		/// <summary>
		/// Boundaries of each color of the set.
		/// </summary>
		public float[] ColorBoundaries;
	}
}
