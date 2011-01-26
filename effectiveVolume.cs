#region Using declarations
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Data;
using NinjaTrader.Gui.Chart;
#endregion

// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    /// <summary>
    /// Enter the description of your new custom indicator here
    /// </summary>
    [Description("Enter the description of your new custom indicator here")]
    public class effectiveVolume : Indicator
    {
        #region Variables
		private DataSeries myDataSeries; // Declare a DataSeries variable
        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        protected override void Initialize()
        {
			Add(new Plot(Color.FromKnownColor(KnownColor.Orange), PlotStyle.Line, "EV"));
			
            Overlay				= false;
			CalculateOnBarClose = false;
			myDataSeries = new DataSeries(this);
			
        }

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override void OnBarUpdate()
        {
			// official trading hours in the US is from 9.30am to 4.00pm
			// 390 minutes == 390 bars, outside pre-market and after-hours
			
			double high = High[0] > Close[1] ? High[0] : Close[1];
			double low = Low[0] < Close[1] ? Low[0] : Close[1];
			double PI = 0.01;
			
			double closeDiff = Close[1] - Close[0] + PI;
			double spread = high - low + PI;
			
			double effectiveVol = closeDiff / spread * Volume[0];
			
			myDataSeries.Set(effectiveVol + myDataSeries[1]);
			EV.Set(myDataSeries[0]);
			
        }

        #region Properties
[Browsable(false)] // this line prevents the data series from being displayed in the indicator properties dialog, do not remove
[XmlIgnore()] // this line ensures that the indicator can be saved/recovered as part of a chart template, do not remove
        public DataSeries EV
        {
            get { return Values[0]; }
        }
        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.
// This namespace holds all indicators and is required. Do not change it.
namespace NinjaTrader.Indicator
{
    public partial class Indicator : IndicatorBase
    {
        private effectiveVolume[] cacheeffectiveVolume = null;

        private static effectiveVolume checkeffectiveVolume = new effectiveVolume();

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public effectiveVolume effectiveVolume()
        {
            return effectiveVolume(Input);
        }

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public effectiveVolume effectiveVolume(Data.IDataSeries input)
        {
            if (cacheeffectiveVolume != null)
                for (int idx = 0; idx < cacheeffectiveVolume.Length; idx++)
                    if (cacheeffectiveVolume[idx].EqualsInput(input))
                        return cacheeffectiveVolume[idx];

            lock (checkeffectiveVolume)
            {
                if (cacheeffectiveVolume != null)
                    for (int idx = 0; idx < cacheeffectiveVolume.Length; idx++)
                        if (cacheeffectiveVolume[idx].EqualsInput(input))
                            return cacheeffectiveVolume[idx];

                effectiveVolume indicator = new effectiveVolume();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                Indicators.Add(indicator);
                indicator.SetUp();

                effectiveVolume[] tmp = new effectiveVolume[cacheeffectiveVolume == null ? 1 : cacheeffectiveVolume.Length + 1];
                if (cacheeffectiveVolume != null)
                    cacheeffectiveVolume.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheeffectiveVolume = tmp;
                return indicator;
            }
        }
    }
}

// This namespace holds all market analyzer column definitions and is required. Do not change it.
namespace NinjaTrader.MarketAnalyzer
{
    public partial class Column : ColumnBase
    {
        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.effectiveVolume effectiveVolume()
        {
            return _indicator.effectiveVolume(Input);
        }

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public Indicator.effectiveVolume effectiveVolume(Data.IDataSeries input)
        {
            return _indicator.effectiveVolume(input);
        }
    }
}

// This namespace holds all strategies and is required. Do not change it.
namespace NinjaTrader.Strategy
{
    public partial class Strategy : StrategyBase
    {
        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        [Gui.Design.WizardCondition("Indicator")]
        public Indicator.effectiveVolume effectiveVolume()
        {
            return _indicator.effectiveVolume(Input);
        }

        /// <summary>
        /// Enter the description of your new custom indicator here
        /// </summary>
        /// <returns></returns>
        public Indicator.effectiveVolume effectiveVolume(Data.IDataSeries input)
        {
            if (InInitialize && input == null)
                throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'Initialize()' method");

            return _indicator.effectiveVolume(input);
        }
    }
}
#endregion

