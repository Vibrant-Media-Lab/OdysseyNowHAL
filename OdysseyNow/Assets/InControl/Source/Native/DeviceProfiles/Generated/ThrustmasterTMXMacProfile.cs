namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class ThrustmasterTMXMacProfile : Xbox360DriverMacProfile
	{
		public ThrustmasterTMXMacProfile()
		{
			Name = "Thrustmaster TMX";
			Meta = "Thrustmaster TMX on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x044f,
					ProductID = 0xb67e,
				},
			};
		}
	}
	// @endcond
}


