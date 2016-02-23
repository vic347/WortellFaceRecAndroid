using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

// New Xlabs
using XLabs.Ioc; // Using for SimpleContainer
using XLabs.Platform.Services.Geolocation; // Using for Geolocation
using XLabs.Platform.Device; // Using for Display
// End new Xlabs

namespace FaceRec.Droid
{
	[Activity (Label = "FaceRec.Droid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{

		var container = new SimpleContainer(); // Create a SimpleCOntainer
		container.Register<IGeolocator, Geolocator>(); // Register the Geolocator
		container.Register<IDevice> (t => AndroidDevice.CurrentDevice); // Register the Device
		Resolver.SetResolver(container.GetResolver()); // Resolve it

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// New Xlabs
			var container = new SimpleContainer();
			container.Register<IDevice> (t => AndroidDevice.CurrentDevice);
			container.Register<IGeolocator, Geolocator>();
			Resolver.SetResolver(container.GetResolver()); // Resolving the services
			// End new Xlabs

			Xamarin.Forms.Forms.Init(this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);
			LoadApplication(new App()); 

		}
	}
}

