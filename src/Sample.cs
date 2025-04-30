using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldVis
{
	public class Sample(float time, float value)
	{
		public float Value { get; set; } = value;
		public float Time { get; set; } = time;
		
	}
}
