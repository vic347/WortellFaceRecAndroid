using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Hardware;
using System;
using Android.Util;
using Java.Lang;
using Android.Content;
using Android.Provider;
using Android.Content.PM;
using Android.Graphics;
using System.IO;
using System.Threading.Tasks;



namespace FaceRekt
{
	[Activity (Label = "FaceRekt", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, TextureView.ISurfaceTextureListener
	{
		Android.Hardware.Camera _camera;
		TextureView _texture;
		ImageView _image;
		private int cameraId = 0;
		private int TAKE_PICTURE = 1;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_texture = FindViewById<TextureView> (Resource.Id.textureView);
			_texture.SurfaceTextureListener = this;

			//button.Click += saveFullImage;


			// Get our button from the layout resource,
			// and attach an event to it

		}

		public async Task Delay()
		{
			Button button = FindViewById<Button> (Resource.Id.button);

			_image = FindViewById<ImageView> (Resource.Id.imageView2);

			//button.Click += delegate {

			Bitmap bmp = Bitmap.CreateBitmap(100, 100, Bitmap.Config.Argb8888);
			_texture.GetBitmap (bmp);
			_image.SetImageBitmap (bmp);
			bmp.Dispose();

			await Task.Delay(5000);
		}

		public void OnSurfaceTextureAvailable (
			Android.Graphics.SurfaceTexture surface, int w, int h)
		{	
			_camera = getCameraInstance ();
			_texture.LayoutParameters =
				new LinearLayout.LayoutParams (w, h);

			try {
				_camera.SetDisplayOrientation(90);
				_camera.SetPreviewTexture (surface);
				_camera.StartPreview ();


			}  catch (Java.IO.IOException ex) {
				Console.WriteLine (ex.Message);
			}
		}

		public static Android.Hardware.Camera getCameraInstance(){
			Android.Hardware.Camera c = null;
			try {
				c = openFrontFacingCamera(); 
			}
			catch (System.Exception e){
			}
			return c; 
		}

		public bool OnSurfaceTextureDestroyed (
			Android.Graphics.SurfaceTexture surface)
		{
			_camera.StopPreview ();
			_camera.Release ();

			return true;
		}

		public void OnSurfaceTextureSizeChanged (Android.Graphics.SurfaceTexture surface, int width, int height)
		{
			// camera takes care of this
		}


		public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface)
		{
			
		}
	
		private static Android.Hardware.Camera openFrontFacingCamera() 
		{
			int cameraCount = 0;
			Android.Hardware.Camera cam = null;
			Android.Hardware.Camera.CameraInfo cameraInfo = new Android.Hardware.Camera.CameraInfo();
			cameraCount = Android.Hardware.Camera.NumberOfCameras;
			for ( int camIdx = 0; camIdx < cameraCount; camIdx++ ) {
				Android.Hardware.Camera.GetCameraInfo( camIdx, cameraInfo );
				if ( cameraInfo.Facing == CameraFacing.Front ) {
					try {
						cam = Android.Hardware.Camera.Open( camIdx );
					} catch (RuntimeException e) {

					}
				}
			}

			return cam;
		}
			
	}	


}