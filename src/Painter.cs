using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using UPG_SP_2024;
using Font = System.Drawing.Font;

namespace ElectricFieldVis;

/// <summary>
///     class that handles all painting to the panel.
/// </summary>
internal static class Painter
{
	

	/// <summary>
	///     Gets or sets the <see cref="Graphics" /> object used for painting.
	/// </summary>
	/// <value>
	///     The <see cref="Graphics" /> object used for rendering the visual elements.
	/// </value>
	public static Graphics G { get; set; }

	public static Scenario Scenario { get; set; }
	public static float Scale { get; set; }
	/// <summary>
	///		Width of Panel.
	/// </summary>
	public static int Width { get; set; }
	/// <summary>
	/// 
	/// </summary>
	public static int Height { get; set; }
	/// <summary>
	///		Table of Intensities.
	/// </summary>
	public static float[][] IntesitiesOfEf { get; set; }

	/// <summary>
	///		Minimal value in Intensity table.
	/// </summary>
	public static float Min { get; set; } = 0;

	/// <summary>
	///		Maximal value in intensity table.
	/// </summary>
	public static float Max { get; set; } = 0;
	/// <summary>
	///		Look up table for colors.
	/// </summary>
	public static Color[]? Lut { get; set; }
	/// <summary>
	///		Background image.
	/// </summary>
	public static Bitmap? Map { get; set; }
	public static Bitmap? Legend { get; set; }
	/// <summary>
	/// Bounds of an image.
	/// (its here, because for some reason the method GetRangeOfCoords does not work in method TransformClickCoords)
	/// </summary>
	public static RectangleF Bounds { get; set; }
	/// <summary>
	/// Instance of a window with graph.
	/// </summary>
	public static Graph Graph { get; set; }
	/// <summary>
	/// Actual time.
	/// </summary>
	public static float TimeNow;
	/// <summary>
	/// Decides if probes should be drawn.
	/// </summary>
	public static bool ShowProbes { get; set; } = true;
	/// <summary>
	/// Decides if grid should be drawn.
	/// </summary>
	public static bool ShowGrid{get;set;} = true;
	/// <summary>
	/// Decides if arrows should be drawn.
	/// </summary>
	public static bool ShowArrows{get;set;} = true;
	/// <summary>
	/// Decides if Map of electrostatic field should be drawn.
	/// </summary>
	public static bool ShowEFMap{get;set;} = true;
	/// <summary>
	/// Decides if static probes should be drawn.
	/// </summary>
	public static bool ShowStaticProbes{get;set;} = true;
	/// <summary>
	/// Decides if charges should be drawn.
	/// </summary>
	public static bool ShowCharges{get;set;} = true;
	/// <summary>
	/// Decides if legend should be drawn.
	/// </summary>
	public static bool ShowLegend { get; set; } = true;



	/// <summary>
	///     Draws all static charge sources present in <see cref="StaticChargeSourceList" />
	/// </summary>
	public static void DrawStaticChargeSources()
	{
		PathGradientBrush brush;

		RectangleF rect;
		for (var i = 0; i < Scenario.StaticChargeSourceList.Count; i++)
		{
			var staticChargePath = Scenario.StaticChargeSourceList[i].GP;

			//create brush for the drawing of the static charge source.
			brush = new PathGradientBrush(staticChargePath);
			brush.CenterPoint = new PointF(Scenario.StaticChargeSourceList[i].Center.X,
				Scenario.StaticChargeSourceList[i].Center.Y);
			brush.FocusScales = new PointF(0, 0);
			brush.InterpolationColors = new ColorBlend(4)
			{
				Colors = Scenario.StaticChargeSourceList[i].ColorSet,
				Positions = Scenario.StaticChargeSourceList[i].ColorBoundaries
			};

			G.FillRegion(brush, new Region(staticChargePath));
		}

		G.ScaleTransform(1, -1);
		string s;
		const float multiplier = 3F / 5F;
		float a;
		PointF p1;
		for (var i = 0; i < Scenario.StaticChargeSourceList.Count; i++)
		{
			//draw +/- depending on polarity of the charge.
			var font = new Font("Consolas", 0.15f * Scenario.StaticChargeSourceList[i].Radius * 2);
			s = Scenario.StaticChargeSourceList[i].StrengthOfEf>0 ? "+" : "-";

			for (var j = 0; j < 6; j++)
			{
				a = j * 2 * MathF.PI / 6;
				p1 = new PointF(
					Scenario.StaticChargeSourceList[i].Center.X + multiplier * Scenario.StaticChargeSourceList[i].Radius * MathF.Cos(a),
					-Scenario.StaticChargeSourceList[i].Center.Y -
					multiplier * Scenario.StaticChargeSourceList[i].Radius * MathF.Sin(a)); //TODO

				G.DrawString(s, font, Brushes.Black, p1.X - G.MeasureString(s, font).Width / 2,
					p1.Y - G.MeasureString(s, font).Height / 2);
			}

			s = $"{Scenario.StaticChargeSourceList[i].StrengthOfEf:0.#} C";
			G.DrawString(s, font, Brushes.Black,
				Scenario.StaticChargeSourceList[i].Center.X - G.MeasureString(s, font).Width / 2,
				-Scenario.StaticChargeSourceList[i].Center.Y - G.MeasureString(s, font).Height / 2);
		}

		G.ScaleTransform(1, -1);

		
	}

	/// <summary>
	///     Draws probe that measures electrostatic field.
	/// </summary>
	public static void DrawProbe()
	{
		



		var probePath = new GraphicsPath();
		var rect = new RectangleF(Scenario.Probe.Position.X, Scenario.Probe.Position.Y, Scenario.Probe.Radius * 2,
			-Scenario.Probe.Radius * 2);
		probePath.AddEllipse(rect);

		var brush = new PathGradientBrush(probePath);
		brush.CenterPoint = new PointF(Scenario.Probe.Center.X, Scenario.Probe.Center.Y);
		brush.FocusScales = new PointF(0, 0);
		brush.InterpolationColors = new ColorBlend(4)
		{
			Colors = Scenario.Probe.ColorSet,
			Positions = Scenario.Probe.ColorBoundaries
		};
		G.FillRegion(brush, new Region(probePath));
		DrawVector();


	}

	/// <summary>
	/// draws static probe.
	/// </summary>
	public static void DrawStaticProbe()
	{

		foreach (var sProbe in Scenario.StaticProbes)
		{
			var brush = new PathGradientBrush(sProbe.GP);
			brush.CenterPoint = new PointF(sProbe.Center.X, sProbe.Center.Y);
			brush.FocusScales = new PointF(0, 0);
			brush.InterpolationColors = new ColorBlend(4)
			{
				Colors = sProbe.ColorSet,
				Positions = sProbe.ColorBoundaries
			};
			G.FillRegion(brush, new Region(sProbe.GP));
			DrawStaticProbeVector(sProbe);
			Graph.CreateGraph();
		}
		
	}
	/// <summary>
	/// Draws vector of static probe.
	/// </summary>
	public static void DrawStaticProbeVector(StaticProbe sProbe)
	{
		
		var location = sProbe.Center;
		var epsilon0 = 8.854f * MathF.Pow(10, -12);
		float strengthX = 0;
		float strengthY = 0;
		var constant = 1 / (4 * MathF.PI * epsilon0);
		float normalise;

		for (var i = 0; i < Scenario.StaticChargeSourceList.Count; i++)
		{
			normalise = MathF.Sqrt(MathF.Pow(Scenario.StaticChargeSourceList[i].Center.X - location.X, 2) +
			                       MathF.Pow(Scenario.StaticChargeSourceList[i].Center.Y - location.Y, 2));
			strengthX += constant * Scenario.StaticChargeSourceList[i].StrengthOfEf *
			             (Scenario.StaticChargeSourceList[i].Center.X - location.X) /
			             MathF.Pow(normalise, 3);
			strengthY += constant * Scenario.StaticChargeSourceList[i].StrengthOfEf *
			             (Scenario.StaticChargeSourceList[i].Center.Y - location.Y) /
			             MathF.Pow(normalise, 3);
		}
		PointF direction;
		float tipLength;
		float vector;
		string s;
		var font = new Font("Consolas", 0.3f * sProbe.Radius * 2);
		if (sProbe != null)
		{
			location = sProbe.Center;
			normalise = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2)) * 3f / 2;

			//normalised direction of the vector
			direction = new PointF(location.X - strengthX / normalise, location.Y - strengthY / normalise);

			//real size of the field
			vector = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2));

			s = $"‖E‖ = {vector:0.###E-00}";
			G.ScaleTransform(1, -1);
			G.DrawString(s, font, Brushes.Black, sProbe.Center.X - G.MeasureString(s, font).Width / 2,
				-sProbe.Center.Y + sProbe.Radius);
			G.ScaleTransform(1, -1);
			tipLength = 1f / 8;
			DrawArrow(location, direction, tipLength, Color.Black);
		}
		var intensity = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2));
		if(sProbe.Data.Count>500) sProbe.Data.RemoveAt(0);
		sProbe.Data.Add(new Sample(TimeNow,intensity));
		
	}
	/// <summary>
	///     Draws vector of electrostatic field of probe or of specific point.
	/// </summary>
	/// <param name="p">optional parameter. When not set, location of probe is used.</param>
	public static async Task DrawVector(PointF? p = null)
	{

		

		var location = p == null ? Scenario.Probe.Center : new PointF(p.Value.X, p.Value.Y);
		var epsilon0 = 8.854f * MathF.Pow(10, -12);
		float strengthX = 0;
		float strengthY = 0;
		var constant = 1 / (4 * MathF.PI * epsilon0);
		float normalise;

		for (var i = 0; i < Scenario.StaticChargeSourceList.Count; i++)
		{
			normalise = MathF.Sqrt(MathF.Pow(Scenario.StaticChargeSourceList[i].Center.X - location.X, 2) +
			                       MathF.Pow(Scenario.StaticChargeSourceList[i].Center.Y - location.Y, 2));
			strengthX += constant * Scenario.StaticChargeSourceList[i].StrengthOfEf *
			             (Scenario.StaticChargeSourceList[i].Center.X - location.X) /
			             MathF.Pow(normalise, 3);
			strengthY += constant * Scenario.StaticChargeSourceList[i].StrengthOfEf *
			             (Scenario.StaticChargeSourceList[i].Center.Y - location.Y) /
			             MathF.Pow(normalise, 3);
		}

		PointF direction;
		float tipLength;
		float vector;
		string s;
		var font = new Font("Consolas", 0.3f * Scenario.Probe.Radius * 2);
		if (p == null)
		{
			normalise = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2)) * 3f / 2;

			//normalised direction of the vector
			direction = new PointF(location.X - strengthX / normalise, location.Y - strengthY / normalise);

			//real size of the field
			vector = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2));

			s = $"‖E‖ = {vector:0.###E-00}";
			G.ScaleTransform(1, -1);
			G.DrawString(s, font, Brushes.Black, Scenario.Probe.Center.X - G.MeasureString(s, font).Width / 2,
				-Scenario.Probe.Center.Y + Scenario.Probe.Radius);
			G.ScaleTransform(1, -1);
			tipLength = 1f / 8;
			DrawArrow(location, direction, tipLength,Color.Black);
			
		}
		else
		{
			normalise = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2)) * 5;
			//normalised direction of the vector
			direction = new PointF(location.X - strengthX / normalise, location.Y - strengthY / normalise);
			//real size of the field


			//calculate this it is not correct
			var lenX = direction.X - location.X;
			var lenY = direction.Y - location.Y;
			direction.X -= lenX / 2;
			direction.Y -= lenY / 2;
			location.X -= lenX / 2;
			location.Y -= lenY / 2;
			tipLength = 5f / 64;
			DrawArrow(location, direction, tipLength,Color.DimGray);
		}

		
		await Task.CompletedTask;
	}

	/// <summary>
	/// draws array of vectors.
	/// </summary>
	/// <param name="disX">distance between arrows on x-axis</param>
	/// <param name="disY">distance between arrows on y-axis</param>
	public static void DrawVectorArray(float disX,float disY) 
	{
		
		//do it from(0,0) and call Draw vector 2 times
		var range = GetRangeOfCoords();
		var rangeX = range.Width / 2;
		var rangeY = range.Height / 2;

		for (var y = disY/2; y <= rangeY; y += disY)
		for (var x = disX/2; x <= rangeX; x += disX)
		{
			
			DrawVector(new PointF(x, y));
			DrawVector(new PointF(-x, -y));
			DrawVector(new PointF(x, -y));
			DrawVector(new PointF(-x, y));
		}
		
	}
	/// <summary>
	/// Draws arrow between 2 points
	/// </summary>
	/// <param name="p1">start point</param>
	/// <param name="p2">end point</param>
	/// <param name="tipLength">length of tip</param>
	/// <param name="color">color of arrow</param>
	public static void DrawArrow(PointF p1, PointF p2, float tipLength,Color color)
	{
		
		double uX = p2.X - p1.X;
		double uY = p2.Y - p1.Y;
		var uLen1 = 1 / Math.Sqrt(uX * uX + uY * uY);
		uX *= uLen1;
		uY *= uLen1;
		//u ma delku 1

		var pen = new Pen(color, 0.0125f);
		pen.LineJoin = LineJoin.Round;
		pen.StartCap = LineCap.Round;
		pen.EndCap = LineCap.Round;

		G.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);

		var cX = (float)(p2.X - uX * tipLength);
		var cY = (float)(p2.Y - uY * tipLength);

		var d = 0.375f * tipLength;

		var dX = (float)(cX - uY * d);
		var dY = (float)(cY + uX * d);
		var eX = (float)(cX + uY * d);
		var eY = (float)(cY - uX * d);
		PointF[] points =
		[
			new(dX, dY),
			p2,
			new(eX, eY)
		];
		G.DrawLines(pen, points);
		
	}
	/// <summary>
	/// sets needed size of field so everything is visible.
	/// </summary>
	/// <param name="size"></param>
	public static void SetNeededSizeOfField(int size)
	{
		
		float rangeX = 0, rangeY = 0;
		for (var i = 0; i < Scenario.StaticChargeSourceList.Count; i++)
		{
			if (Math.Abs(Scenario.StaticChargeSourceList[i].Center.X) > rangeX)
				rangeX = MathF.Abs(Scenario.StaticChargeSourceList[i].Center.X);
			if (Math.Abs(Scenario.StaticChargeSourceList[i].Center.Y) > rangeY)
				rangeY = MathF.Abs(Scenario.StaticChargeSourceList[i].Center.Y);
		}

		var scale = MathF.Max(rangeX, rangeY) == 0 ? 3 :
			MathF.Max(rangeX, rangeY) + 3 > 6 ? 6 : MathF.Max(rangeX, rangeY) + 3;
		Scale = size / scale;
		G.ScaleTransform(size / scale, -size / scale);
		
	}
	/// <summary>
	/// Gets range of Coordinate system
	/// </summary>
	/// <returns></returns>
	public static RectangleF GetRangeOfCoords()
	{
		return G.ClipBounds;
	}
	/// <summary>
	/// Calculates strength of field at <see cref="p"/>
	/// </summary>
	/// <param name="p">point</param>
	/// <returns></returns>
	private static float CalculateStrengthOfEf(PointF p)
	{
		
		var epsilon0 = 8.854f * MathF.Pow(10, -12);
		float strengthX = 0;
		float strengthY = 0;
		var constant = 1 / (4 * MathF.PI * epsilon0);
		float normalise;

		for (var i = 0; i < Scenario.StaticChargeSourceList.Count; i++)
		{
			normalise = MathF.Sqrt(MathF.Pow(Scenario.StaticChargeSourceList[i].Center.X - p.X, 2) +
			                       MathF.Pow(Scenario.StaticChargeSourceList[i].Center.Y - p.Y, 2));
			strengthX += constant * Scenario.StaticChargeSourceList[i].StrengthOfEf *
			             ((Scenario.StaticChargeSourceList[i].Center.X - p.X) /
			              MathF.Pow(normalise, 3));
			strengthY += constant * Scenario.StaticChargeSourceList[i].StrengthOfEf *
			             ((Scenario.StaticChargeSourceList[i].Center.Y - p.Y) /
			              MathF.Pow(normalise, 3));
		}

		var vector = MathF.Sqrt(MathF.Pow(strengthX, 2) + MathF.Pow(strengthY, 2));
		
		return vector;
	}

	/// <summary>
	/// Draws grid.
	/// </summary>
	public static void DrawGrid()
	{
		
		var range = GetRangeOfCoords();
		var rangeX = range.Width / 2;
		var rangeY = range.Height / 2;
		var font = new Font("Consolas", 0.05f, FontStyle.Bold);
		Pen pen;
		string s;
		for (float i = 0; i < rangeX; i += 1f)
		{
			pen = i == 0 ? new Pen(Color.Black, 0.01f) : new Pen(Color.Gray, 0.01f);


			G.DrawLine(pen, new PointF(i, rangeY), new PointF(i, -rangeY));
			G.DrawLine(pen, new PointF(-i, rangeY), new PointF(-i, -rangeY));
			G.DrawLine(pen, new PointF(rangeX, i), new PointF(-rangeX, i));
			G.DrawLine(pen, new PointF(rangeX, -i), new PointF(-rangeX, -i));


			if (i % 1 == 0 && i != 0)
			{
				G.ScaleTransform(1, -1);

				s = Convert.ToString(i, CultureInfo.InvariantCulture);


				G.DrawString(s, font, Brushes.Black, 0, -i);
				G.DrawString(s, font, Brushes.Black, i, 0);


				s = "-" + s;


				G.DrawString(s, font, Brushes.Black, -i, 0);
				G.DrawString(s, font, Brushes.Black, 0, i);


				G.ScaleTransform(1, -1);
			}
		}
		
	}
	/// <summary>
	/// Loads Scenarios from .json
	/// </summary>
	/// <param name="argument">index of scenario or path to .json file</param>
	public static void Load(string argument)
	{
		string location = AppContext.BaseDirectory;
		var settings = new JsonSerializerSettings();
		settings.Converters.Add(new GraphicsPathJsonConverter());
		if (argument.Length == 1) location = $"Scenario_{argument}.json";
		else location = argument;
		string value = File.ReadAllText(location);
		Scenario sc;
		try
		{
			sc = Newtonsoft.Json.JsonConvert.DeserializeObject<Scenario>(value, settings);
		}
		catch (Exception e)
		{
			sc = Newtonsoft.Json.JsonConvert.DeserializeObject<Scenario>("0", settings);
			throw;
		}
		

		Scenario = sc;
		
	}
	/// <summary>
	/// Changes size of all charges. depends on actual time.
	/// </summary>
	/// <param name="elapsed">current time</param>
	public static void ChangeSize(float elapsed)
	{
		
		var i = 1;
		for (var j = 0; j < Scenario.StaticChargeSourceList.Count; j++)
		{
			Scenario.StaticChargeSourceList[j].ChangeChargeSize(i * elapsed);
			i *= -1;
		}
		
	}
	/// <summary>
	/// Draws whole image
	/// </summary>
	/// <param name="x">distance between arrows on x-axis</param>
	/// <param name="y">distance between arrows on y-axis</param>
	public static void Draw(float x,float y)
	{


		if (IntesitiesOfEf is null) CreateIntensityTable(); 
		if (Lut is null) MakeLut();
		if(ShowEFMap)DrawElectricFieldMap(); 
		if(ShowGrid)DrawGrid();
		if(ShowArrows)DrawVectorArray(x,y);
		if (ShowLegend) DrawLegend();
		if(ShowCharges)DrawStaticChargeSources();
		if(ShowProbes)DrawProbe();
		if(ShowStaticProbes)DrawStaticProbe();


	}

	/// <summary>
	///     gets color from lut and converts it to ARGB
	/// </summary>
	/// <param name="fieldIntensity">intensity of electric field</param>
	/// <returns>color in ARGB</returns>
	public static int GetColor(float fieldIntensity)
	{
		var normalizedValue = (Math.Log(fieldIntensity) - Math.Log(Min)) / (Math.Log(Max) - Math.Log(Min));
		var index = (int)(normalizedValue * (Lut.Length - 1));
		if (index < 0) return Lut[0].ToArgb();
		if (index >= Lut.Length) return Lut.Last().ToArgb();
		return Lut[index].ToArgb();
	}
	/// <summary>
	/// Draws map of intensity of electrostatic field
	/// </summary>
	public static void DrawElectricFieldMap()
	{
		if (Map is not null)
		{
			var Rect = GetRangeOfCoords();
			G.DrawImage(Map, Rect.X, Rect.Y, Rect.Width, Rect.Height);
			return;
		}
		var bitmap = new Bitmap(Width, Height);

		var rect = new Rectangle(0, 0, Width, Height);
		var bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
		var ptr = bitmapData.Scan0;

		var pixels = new int[Width * Height];


		Parallel.For(0, Width, x =>
		{
			for (var y = 0; y < Height; y++)
			{


				pixels[y * Width + x] = GetColor(IntesitiesOfEf[x][y]);
			}
		});

		Marshal.Copy(pixels, 0, ptr, pixels.Length);

		bitmap.UnlockBits(bitmapData);


		var drawRect = GetRangeOfCoords();
		G.DrawImage(bitmap, drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height);
		Map = bitmap;
		
	}
	/// <summary>
	/// creates table of intensities.
	/// </summary>
	public static void CreateIntensityTable()
	{
		float min = float.MaxValue, max = float.MinValue;
		RectangleF coords = GetRangeOfCoords();
		float MaxX = coords.Width / 2, MaxY = coords.Height / 2;
		float stepX = coords.Width / Width, stepY = coords.Height / Height;
		IntesitiesOfEf = new float[Width][];
		object minMaxLock = new object(); 
		

		Parallel.For(0, (int)Width, () => (float.MaxValue, float.MinValue), (x, state, localMinMax) =>
		{


			IntesitiesOfEf[(int)x] = new float[Height];

			for (float y = 0, scaledY = coords.Y; y < Height; y++, scaledY += stepY)
			{
				 
				IntesitiesOfEf[(int)x][(int)y] = CalculateStrengthOfEf(new PointF(coords.X + x * stepX, scaledY));


				if (IntesitiesOfEf[(int)x][(int)y] < localMinMax.Item1)
					localMinMax.Item1 = IntesitiesOfEf[(int)x][(int)y];
				if (IntesitiesOfEf[(int)x][(int)y] > localMinMax.Item2)
					localMinMax.Item2 = IntesitiesOfEf[(int)x][(int)y];
			}

			return localMinMax;
		}, finalLocalMinMax =>
		{
			// Synchronizace globálního minima a maxima
			lock (minMaxLock)
			{
				if (finalLocalMinMax.Item1 < min) min = finalLocalMinMax.Item1;
				if (finalLocalMinMax.Item2 > max) max = finalLocalMinMax.Item2;
			}
		});


		if (Min == 0 || Max == 0)
		{
			Min = min;
			Max = max;
		}
		

	}
	/// <summary>
	/// transform coordinates of click to current transformation
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static PointF TransformClickCoords(float x, float y)
	{
		x -= Width / 2f;
		y -= Height / 2f;
		x /= Scale;
		y /= Scale;
		y = -y;
		var pt = new PointF(x, y);

		
		var difference = Bounds.Width / 2 - Bounds.Height / 2;
		return pt;
	}
	/// <summary>
	/// creates image of legend, that is then drawn in the upper right corner.
	/// </summary>
	public static void DrawLegend()
	{
		var drawRect = GetRangeOfCoords();
		if (Legend is not null)
		{
			G.DrawImage(Legend, drawRect.X, -drawRect.Y, drawRect.Width / 4, -drawRect.Height / 8);
			return;
		}

		string max = $"{Max:0.00 E-00}";
		string min = $"{Min:0.00 E-00}";
		Bitmap bmp = new Bitmap(Width / 4, Height / 8);
		var g = Graphics.FromImage(bmp);
		var rect = g.ClipBounds;
		g.Clear(Color.FromArgb(0xF0,0xB0,0xB0,0xB0));
		PointF topleft = new PointF((1 / 8f * bmp.Width), (3 / 8f * bmp.Height));
		var size = new SizeF(bmp.Width * 6/8f, bmp.Height * 4 / 8f);
		var GradRect = new RectangleF(topleft, size);
		float[] positions = new float[Painter.Lut.Length];

		for (int i = 0; i < Lut.Length; i++)
		{
			positions[i] = (float)i / (Lut.Length - 1);
		}
		var blend = new ColorBlend
		{
			Colors = Lut,
			Positions = positions
		};

		var brush = new LinearGradientBrush(GradRect, Color.Empty, Color.Empty, LinearGradientMode.Horizontal)
		{
			InterpolationColors = blend
		};
		var font = new Font("Consolas", 7.5f );
		g.FillRectangle(brush,GradRect);
		g.DrawString(min,font,Brushes.Black,GradRect.Left-g.MeasureString(min,font).Width/2,GradRect.Top-g.MeasureString(min,font).Height);
		g.DrawString(max, font, Brushes.Black, GradRect.Right - g.MeasureString(max, font).Width / 2, GradRect.Top - g.MeasureString(max, font).Height);
		Legend = bmp;
		G.DrawImage(bmp,drawRect.X,-drawRect.Y,drawRect.Width/4,-drawRect.Height/8);
	}
	/// <summary>
	/// Creates lut based of min and max value of intensities
	/// </summary>
	public static void MakeLut()
	{
		const int N = 256; 
		var clr = new Color[N];


		Color[] keyColors =
		{
			Color.FromArgb(50, 100, 200),
			Color.FromArgb(0,200,200),
			Color.FromArgb(0,255,0),
			Color.FromArgb(255, 255, 0),
			Color.FromArgb(255, 165, 0),
			Color.FromArgb(255, 120, 0),
			Color.FromArgb(255,70,0),
			Color.FromArgb(255, 0, 0),
			Color.FromArgb(180, 0, 0),
			Color.FromArgb(100,0,0)

		};

		var segmentCount = keyColors.Length - 1;

		for (var i = 0; i < N; i++)
		{
			var t = (float)i / (N - 1); 
			var value = Min * (float)Math.Pow(Max / Min, t);

			if (value <= Min)
			{
				clr[i] = keyColors[0]; 
			}
			else if (value >= Max)
			{
				clr[i] =keyColors.Last(); 
			}
			else
			{
				
				var normalizedValue = (Math.Log(value) - Math.Log(Min)) / (Math.Log(Max) - Math.Log(Min));
				var segment = (int)(normalizedValue * segmentCount);
				var localT = normalizedValue * segmentCount - segment;

				
				var startColor = keyColors[segment];
				var endColor = keyColors[segment + 1];

				var r = (int)(startColor.R + localT * (endColor.R - startColor.R));
				var g = (int)(startColor.G + localT * (endColor.G - startColor.G));
				var b = (int)(startColor.B + localT * (endColor.B - startColor.B));

				clr[i] = Color.FromArgb(r, g, b);
			}
		}

		Lut = clr;
	}

}