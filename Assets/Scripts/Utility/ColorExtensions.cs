using UnityEngine;

public static class ColorExtensions
{
	public static Color WithHue( this Color color, float hue )
	{
		float _;
		float saturation;
		float value;
		Color.RGBToHSV( color, out _, out saturation, out value );
		return Color.HSVToRGB( hue, saturation, value );
	}
}