﻿namespace StockSharp.Algo.Indicators;

/// <summary>
/// Gator oscillator.
/// </summary>
/// <remarks>
/// https://doc.stocksharp.com/topics/api/indicators/list_of_indicators/gator_oscillator.html
/// </remarks>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.GatorKey,
	Description = LocalizedStrings.GatorOscillatorKey)]
[Doc("topics/api/indicators/list_of_indicators/gator_oscillator.html")]
[IndicatorOut(typeof(GatorOscillatorValue))]
public class GatorOscillator : BaseComplexIndicator<GatorOscillatorValue>
{
	private readonly Alligator _alligator;

	/// <summary>
	/// Initializes a new instance of the <see cref="GatorOscillator"/>.
	/// </summary>
	public GatorOscillator()
	{
		_alligator = new();
		AddResetTracking(_alligator);

		AddInner(Histogram1 = new(_alligator.Jaw, _alligator.Lips, false));
		AddInner(Histogram2 = new(_alligator.Lips, _alligator.Teeth, true));
	}

	/// <inheritdoc />
	public override IndicatorMeasures Measure => IndicatorMeasures.MinusOnePlusOne;

	/// <summary>
	/// Top histogram.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.UpKey,
		Description = LocalizedStrings.TopHistogramKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public GatorHistogram Histogram1 { get; }

	/// <summary>
	/// Lower histogram.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[Display(
		ResourceType = typeof(LocalizedStrings),
		Name = LocalizedStrings.DownKey,
		Description = LocalizedStrings.LowHistogramKey,
		GroupName = LocalizedStrings.GeneralKey)]
	public GatorHistogram Histogram2 { get; }

	/// <inheritdoc />
	protected override bool CalcIsFormed() => _alligator.IsFormed;

	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		_alligator.Process(input);

		return base.OnProcess(input);
	}

	/// <inheritdoc />
	public override string ToString() => $"{base.ToString()}, H1={Histogram1}, H2={Histogram2}";

	/// <inheritdoc />
	protected override GatorOscillatorValue CreateValue(DateTimeOffset time)
		=> new(this, time);
}

/// <summary>
/// <see cref="GatorOscillator"/> indicator value.
/// </summary>
public class GatorOscillatorValue : ComplexIndicatorValue<GatorOscillator>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GatorOscillatorValue"/>.
	/// </summary>
	/// <param name="indicator"><see cref="GatorOscillator"/></param>
	/// <param name="time"><see cref="IIndicatorValue.Time"/></param>
	public GatorOscillatorValue(GatorOscillator indicator, DateTimeOffset time)
		: base(indicator, time)
	{
	}

	/// <summary>
	/// Gets the <see cref="GatorOscillator.Histogram1"/> value.
	/// </summary>
	public decimal Histogram1 => GetInnerDecimal(TypedIndicator.Histogram1);

	/// <summary>
	/// Gets the <see cref="GatorOscillator.Histogram2"/> value.
	/// </summary>
	public decimal Histogram2 => GetInnerDecimal(TypedIndicator.Histogram2);
}
