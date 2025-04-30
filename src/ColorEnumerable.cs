using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldVis
{
	public enum ColorStyle
	{
		RedObject,
		SilverObject,
		BlueObject,
		GoldenObject
	}

	public static class ColorEnumerable
	{
		public static Dictionary<ColorStyle, (Color[], float[])> ColorBlendSets = new Dictionary<ColorStyle, (Color[], float[])>()
		{
			{
				ColorStyle.RedObject,
				(
					new Color[] { Color.DarkRed, Color.Firebrick, Color.IndianRed, Color.PeachPuff },
					new float[] { 0f, 0.3f, 0.6f, 1f }
				)
			},
			{
				ColorStyle.SilverObject,
				(
					new Color[] { Color.DarkSlateGray, Color.Silver, Color.LightGray, Color.WhiteSmoke },
					new float[] { 0f, 0.3f, 0.6f, 1f }
				)
			},
			{
				ColorStyle.BlueObject,
				(
					new Color[] { Color.DarkBlue, Color.RoyalBlue, Color.SkyBlue, Color.LightCyan },
					new float[] { 0f, 0.4f, 0.7f, 1f }
				)
			},
			{
				ColorStyle.GoldenObject,
				(
					new Color[] { Color.DarkGoldenrod, Color.Goldenrod, Color.Gold, Color.LightGoldenrodYellow },
					new float[] { 0f, 0.3f, 0.6f, 1f }
				)
			}
		};

		public static (Color[], float[]) GetBlend(ColorStyle style)
		{
			return ColorBlendSets[style];
		}
	}
}
