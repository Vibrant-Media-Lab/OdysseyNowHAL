namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class InjusticeFightStickMacProfile : Xbox360DriverMacProfile
	{
		public InjusticeFightStickMacProfile()
		{
			Name = "Injustice Fight Stick";
			Meta = "Injustice Fight Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0125,
				},
			};
		}
	}
	// @endcond
}


