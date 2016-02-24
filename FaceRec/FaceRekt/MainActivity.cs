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
		Switch _switch;
		ImageView _image;
		Button button;

		int temp = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			button = FindViewById<Button> (Resource.Id.button);


			_switch = FindViewById<Switch> (Resource.Id.floop);

			_switch.CheckedChange += delegate(object sender, CompoundButton.CheckedChangeEventArgs e) {
				if(e.IsChecked == true){
					temp = 1;
					button.Visibility = ViewStates.Visible;
				}
				else{
					temp = 0;
					getBitmapObjectLoop();
					button.Visibility = ViewStates.Invisible;
				}
			};

			_texture = FindViewById<TextureView> (Resource.Id.textureView);
			_texture.SurfaceTextureListener = this;


			// Get our button from the layout resource,
			// and attach an event to il

		}

		public async void getBitmapObjectLoop(){
			while (temp != 1) {
				await Task.Factory.StartNew (() => {
					RunOnUiThread(()=>{
						getBitmapObject();
					});
				});
				await Task.Delay(1000);
			}
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


		public void OnSurfaceTextureUpdated (Android.Graphics.SurfaceTexture surface){

			_image = FindViewById<ImageView> (Resource.Id.imageView2);
			button.Click += delegate {
				getBitmapObject();
			
			};
		}
		private void getBitmapObject(){
			_image = FindViewById<ImageView> (Resource.Id.imageView2);

				Bitmap bmp = Bitmap.CreateBitmap(100, 100, Bitmap.Config.Argb8888);
				Bitmap bit = _texture.GetBitmap (bmp);
				_image.SetImageBitmap(bit);
			bit.Dispose();


		}
			//062115787
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
