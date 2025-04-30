using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.SKCharts;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ElectricFieldVis
{

	public partial class Graph : Form
	{

		public float StartTime;
		public Graph()
		{
			InitializeComponent();
			cartesianChart1.AnimationsSpeed = TimeSpan.Zero;

		}

		public void CreateGraph()
		{
			List<ISeries> series = new List<ISeries>();
			for (int i = 0; i < Painter.Scenario.StaticProbes.Count; i++)
			{
				series.Add(new LineSeries<Sample>
				{
					Values = Painter.Scenario.StaticProbes[i].Data,
					Stroke = new SolidColorPaint(Painter.Scenario.StaticProbes[i].GraphColor) { StrokeThickness = 5 },
					Fill = null,
					GeometryFill = null,
					GeometryStroke = null,
					Mapping = (sample, index) => new(sample.Time, sample.Value),
					YToolTipLabelFormatter = (x) => $" {x.Coordinate.PrimaryValue:0.###E-00} C",
					XToolTipLabelFormatter = (x) => $" {x.Coordinate.SecondaryValue:0.###} sec"
				});
			}

			cartesianChart1.Series = series;
			cartesianChart1.XAxes = new[] { new Axis { Labeler = value => $"{value}" } };
			cartesianChart1.YAxes = new[] { new Axis { Labeler = value => $"{value:0 E-00} C" } };

		}

		private void cartesianChart1_Load(object sender, EventArgs e)
		{

		}
	}
}
