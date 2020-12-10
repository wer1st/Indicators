﻿namespace ATAS.Indicators.Technical
{
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	using ATAS.Indicators.Technical.Properties;

	[DisplayName("DeTrended Oscillator")]
	public class DeTrended : Indicator
	{
		#region Fields

		private readonly ValueDataSeries _renderSeries = new ValueDataSeries(Resources.Visualization);
		private readonly SMA _sma = new SMA();
		private int _lookBack;
		private int _period;

		#endregion

		#region Properties

		[Display(ResourceType = typeof(Resources), Name = "Period", GroupName = "Settings", Order = 100)]
		public int Period
		{
			get => _period;
			set
			{
				if (value <= 0)
					return;

				_period = value;
				RecalculateValues();
			}
		}

		#endregion

		#region ctor

		public DeTrended()
		{
			Panel = IndicatorDataProvider.NewPanel;
			_period = 10;

			DataSeries[0] = _renderSeries;
		}

		#endregion

		#region Protected methods

		protected override void OnCalculate(int bar, decimal value)
		{
			if (bar == 0)
			{
				_sma.Period = _period / 2;
				_lookBack = _sma.Period / 2 + 1;
			}

			_sma.Calculate(bar, value);

			if (bar < _lookBack)
				return;

			_renderSeries[bar] = value - _sma[bar - _lookBack];
		}

		#endregion
	}
}