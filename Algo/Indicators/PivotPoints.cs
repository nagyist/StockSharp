﻿namespace StockSharp.Algo.Indicators;

using StockSharp.Algo.Candles;

/// <summary>
/// Pivot Points indicator.
/// </summary>
[Display(
	ResourceType = typeof(LocalizedStrings),
	Name = LocalizedStrings.PPKey,
	Description = LocalizedStrings.PivotPointsKey)]
[IndicatorIn(typeof(CandleIndicatorValue))]
[Doc("topics/api/indicators/list_of_indicators/pivot_points.html")]
[IndicatorOut(typeof(PivotPointsValue))]
public class PivotPoints : BaseComplexIndicator<PivotPointsValue>
{
	/// <summary>
	/// Pivot point.
	/// </summary>
	[Browsable(false)]
	public PivotPointPart PivotPoint { get; } = new() { Name = "PivotPoint" };

	/// <summary>
	/// Resistance level R1.
	/// </summary>
	[Browsable(false)]
	public PivotPointPart R1 { get; } = new() { Name = "R1" };

	/// <summary>
	/// Resistance level R2.
	/// </summary>
	[Browsable(false)]
	public PivotPointPart R2 { get; } = new() { Name = "R2" };

	/// <summary>
	/// Support level S1.
	/// </summary>
	[Browsable(false)]
	public PivotPointPart S1 { get; } = new() { Name = "S1" };

	/// <summary>
	/// Support level S2.
	/// </summary>
	[Browsable(false)]
	public PivotPointPart S2 { get; } = new() { Name = "S2" };

	/// <summary>
	/// Initializes a new instance of the <see cref="PivotPoints"/>.
	/// </summary>
	public PivotPoints()
	{
		AddInner(PivotPoint);
		AddInner(R1);
		AddInner(R2);
		AddInner(S1);
		AddInner(S2);
	}

	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		var candle = input.ToCandle();
		var cl = candle.GetLength();

		var result = new PivotPointsValue(this, input.Time);

		var pivotPoint = (candle.HighPrice + candle.LowPrice + candle.ClosePrice) / 3;

		result.Add(PivotPoint, PivotPoint.Process(pivotPoint, input.Time, input.IsFinal));
		result.Add(R1, R1.Process(2 * pivotPoint - candle.LowPrice, input.Time, input.IsFinal));
		result.Add(R2, R2.Process(pivotPoint + cl, input.Time, input.IsFinal));
		result.Add(S1, S1.Process(2 * pivotPoint - candle.HighPrice, input.Time, input.IsFinal));
		result.Add(S2, S2.Process(pivotPoint - cl, input.Time, input.IsFinal));

		return result;
	}

	/// <inheritdoc />
	protected override PivotPointsValue CreateValue(DateTimeOffset time)
		=> new(this, time);
}

/// <summary>
/// Represents a part of the Pivot Points indicator.
/// </summary>
[IndicatorHidden]
public class PivotPointPart : BaseIndicator
{
	/// <inheritdoc />
	protected override IIndicatorValue OnProcess(IIndicatorValue input)
	{
		if (input.IsFinal)
			IsFormed = true;
		
		return input;
	}
}

/// <summary>
/// <see cref="PivotPoints"/> indicator value.
/// </summary>
public class PivotPointsValue : ComplexIndicatorValue<PivotPoints>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="PivotPointsValue"/>.
	/// </summary>
	/// <param name="indicator"><see cref="PivotPoints"/></param>
	/// <param name="time"><see cref="IIndicatorValue.Time"/></param>
	public PivotPointsValue(PivotPoints indicator, DateTimeOffset time)
		: base(indicator, time)
	{
	}

	/// <summary>
	/// Gets the Pivot Point value.
	/// </summary>
	public decimal PivotPoint => GetInnerDecimal(TypedIndicator.PivotPoint);

	/// <summary>
	/// Gets the R1 value.
	/// </summary>
	public decimal R1 => GetInnerDecimal(TypedIndicator.R1);

	/// <summary>
	/// Gets the R2 value.
	/// </summary>
	public decimal R2 => GetInnerDecimal(TypedIndicator.R2);

	/// <summary>
	/// Gets the S1 value.
	/// </summary>
	public decimal S1 => GetInnerDecimal(TypedIndicator.S1);

	/// <summary>
	/// Gets the S2 value.
	/// </summary>
	public decimal S2 => GetInnerDecimal(TypedIndicator.S2);
}
