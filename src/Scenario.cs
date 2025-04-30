using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricFieldVis
{
	internal class Scenario
	{
		/// <summary>
		///     List of all <see cref="StaticChargeSource" />.
		/// </summary>
		/// <value>
		///     List of instances of <see cref="StaticChargeSource" />
		/// </value>
		public List<StaticChargeSource> StaticChargeSourceList = [];
		/// <summary>
		///     Probe present in the image.
		/// </summary>
		public Probe Probe { get; set; } = new();
		/// <summary>
		///		Static probe that is shown after click on panel.
		/// </summary>
		public List<StaticProbe> StaticProbes { get; set; } = [];
		/// <summary>
		///		Tells whether the current scenario is static.
		/// </summary>
		public bool IsScenarioStatic { get; set; } = true;

		public Scenario(List<StaticChargeSource> scsList,Probe probe, bool isStatic = true, StaticProbe? sProbe = null)
		{
			this.StaticChargeSourceList = scsList;
			this.Probe = probe;
			if(sProbe!=null)this.StaticProbes.Add(sProbe);
			this.IsScenarioStatic = isStatic;
		}
	}
}
