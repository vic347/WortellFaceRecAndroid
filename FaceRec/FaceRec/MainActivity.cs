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
using Java.Interop;
using System.Threading.Tasks;
using System.IO;


namespace FaceRec
{
	[Activity (Label = "FaceRec", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, TextureView.ISurfaceTextureListener
	{
		Android.Hardware.Camera _camera;
		TextureView _texture;
		private int cameraId = 0;
		static int REQUEST_IMAGE_CAPTURE = 1;

		int count = 1;

		Button button;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_texture = FindViewById<TextureView> (Resource.Id.textureView1);
			_texture.SurfaceTextureListener = this;

			button = (Button)FindViewById (Resource.Id.button1);
		
			// Get our button from the layout resource,
			// and attach an event to it

		}

		public void OnSurfaceTextureAvailable (
			Android.Graphics.SurfaceTexture surface, int w, int h)
		{	
			_camera = getCameraInstance ();
			_texture.LayoutParameters =
				new RelativeLayout.LayoutParams (w, h);

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

		private void dispatchTakePictureIntent() {
			Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);

			StartActivityForResult(takePictureIntent, REQUEST_IMAGE_CAPTURE);

		}

		public interface IScreenshotManager
		{
			Task<byte[]> CaptureAsync();
		}

		public class ScreenshotManager : IScreenshotManager
		{
			public static Activity Activity { get; set; }

			public async System.Threading.Tasks.Task<byte[]> CaptureAsync()
			{
				if(Activity == null)
				{
					throw new System.Exception("You have to set ScreenshotManager.Activity in your Android project");
				}

				var view = Activity.Window.DecorView;
				view.DrawingCacheEnabled = true;

				Bitmap bitmap = view.GetDrawingCache(true);

				byte[] bitmapData;

				using (var stream = new MemoryStream())
				{
					bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
					bitmapData = stream.ToArray();
				}

				return bitmapData;
			}
		}



		/// <summary>
		/// /////////
		/// </summary>
		///     
		/// 

		//protected virtual void OnActivityResult(int requestCode, int resultCode, Intent data) {
		//	if (requestCode == REQUEST_IMAGE_CAPTURE && resultCode = Result.Ok) {
		//		Bundle extras = data.GetStringExtra("data");
		//		Bitmap imageBitmap = (Bitmap) extras;
		//		//mImageView.setImageBitmap(imageBitmap);
		//	}
		//}

	}
}