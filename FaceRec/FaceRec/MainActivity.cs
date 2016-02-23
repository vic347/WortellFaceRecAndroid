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
using Java.IO;


namespace FaceRec
{
	[Activity (Label = "FaceRec", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, Android.Hardware.Camera.IPictureCallback ,Android.Hardware.Camera.IPreviewCallback, Android.Hardware.Camera.IShutterCallback, ISurfaceHolderCallback
	{
		Android.Hardware.Camera _camera;
		SurfaceView _surfaceView;
		private int cameraId = 0;
		static int REQUEST_IMAGE_CAPTURE = 1;

		int count = 1;

		Button button;

		Android.Hardware.Camera camera;
		System.String PICTURE_FILENAME = "picture.jpg";

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_surfaceView = FindViewById<SurfaceView> (Resource.Id.textureView1);

			SurfaceView surface = (SurfaceView)FindViewById (Resource.Id.textureView1);
			var holder = surface.Holder;
			holder.AddCallback (this);
			holder.SetType (Android.Views.SurfaceType.PushBuffers);

			FindViewById(Resource.Id.button1).Click += delegate {
				Android.Hardware.Camera.Parameters p = camera.GetParameters();
				p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
				camera.SetParameters(p);
				camera.TakePicture(this,this,this);

			};
		
			// Get our button from the layout resource,
			// and attach an event to it

		}

		void Android.Hardware.Camera.IPreviewCallback.OnPreviewFrame(byte[] b, Android.Hardware.Camera c)
		{

		}

		void Android.Hardware.Camera.IShutterCallback.OnShutter()
		{

		}

		public void SurfaceCreated(ISurfaceHolder holder)
		{
			try{
				camera = Android.Hardware.Camera.Open();
				Android.Hardware.Camera.Parameters p = camera.GetParameters();
				p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
				camera.SetParameters(p);
				camera.SetPreviewCallback(this);
				camera.Lock();
				camera.SetPreviewDisplay(holder);
				camera.StartPreview();
			}
			catch(System.IO.IOException e){
			}
		}

		public void SurfaceDestroyed(ISurfaceHolder holder){

			camera.Unlock ();
			camera.StopPreview ();
			camera.SetPreviewCallback (null);
			camera.Release ();
			camera = null;
		}

		public void SurfaceChanged(ISurfaceHolder holder,Android.Graphics.Format f,int i, int j)
		{
			camera.SetDisplayOrientation(90);
		}

		void Android.Hardware.Camera.IPictureCallback.OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
		{
			FileOutputStream outStream = null;
			Java.IO.File dataDir = Android.OS.Environment.ExternalStorageDirectory;
			if (data != null) {
				try{
					outStream = new FileOutputStream(dataDir + "/" + PICTURE_FILENAME);
					outStream.Write(data);
					outStream.Close();
				}catch(System.IO.FileNotFoundException e){
					System.Console.Out.WriteLine (e.Message);
				}catch(System.IO.IOException ie){
					System.Console.Out.WriteLine (ie.Message);
				}
			}
		}

		//public void OnSurfaceTextureAvailable (
			//Android.Graphics.SurfaceTexture surface, int w, int h)
		//{	
			//_camera = getCameraInstance ();
			//_surfaceView.LayoutParameters =
				//new RelativeLayout.LayoutParams (w, h);

			//try {
				//_camera.SetDisplayOrientation(45);
				//_camera.SetPreviewTexture (surface);
				//_camera.StartPreview ();


			//}  catch (Java.IO.IOException ex) {
				//System.Console.WriteLine (ex.Message);
			//}
		//}

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