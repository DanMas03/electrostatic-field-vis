using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldVis
{
	public class GraphicsPathJsonConverter : JsonConverter<System.Drawing.Drawing2D.GraphicsPath>
	{
		public override void WriteJson(JsonWriter writer, System.Drawing.Drawing2D.GraphicsPath value, JsonSerializer serializer)
		{
			// Example: Serialize the path points and types
			var pathData = value.PathData;
			serializer.Serialize(writer, new { Points = pathData.Points, Types = pathData.Types });
		}

		public override System.Drawing.Drawing2D.GraphicsPath ReadJson(JsonReader reader, Type objectType, System.Drawing.Drawing2D.GraphicsPath existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var data = serializer.Deserialize<PathData>(reader);
			var path = new System.Drawing.Drawing2D.GraphicsPath();
			path.AddPath(new System.Drawing.Drawing2D.GraphicsPath(data.PathPoints, data.PathTypes), false);
			return path;
		}

		private class PathData
		{
			public System.Drawing.PointF[] PathPoints { get; set; }
			public byte[] PathTypes { get; set; }
		}
	}
}
