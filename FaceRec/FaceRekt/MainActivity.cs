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
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;

namespace FaceRekt
{
	[Activity (Label = "FaceRekt", MainLauncher = true, Icon = "@mipmap/icon", Theme="@style/MyTheme")]
	public class MainActivity : AppCompatActivity, TextureView.ISurfaceTextureListener
	{
		Android.Hardware.Camera _camera;
		private TextureView _texture;
		private ToggleButton _camera_switch, _picture_switch;
		private ImageView _image;
		private LinearLayout fake_button;
		private DrawerLayout mDrawerLayout;
		private MyActionBarDrawerToggle mDrawerToggle;
		private Toolbar toolbar;
		private LinearLayout mLeftDrawer;
	
		int temp = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			fake_button = FindViewById<LinearLayout> (Resource.Id.button);
//			camera_icon = FindViewById<ImageView> (Resource.Id.image);

			_picture_switch= FindViewById<ToggleButton> (Resource.Id.togglePictureStyle);

			_picture_switch.Click += (o,e)=> {
				if(_picture_switch.Checked){
					temp = 1;
					_texture.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, (float) 1.75);
					fake_button.Visibility = ViewStates.Visible;
				}
				else{
					temp = 0;
					getBitmapObjectLoop();
					_texture.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, (float) 2.0);
					fake_button.Visibility = ViewStates.Invisible;
				}
			};
				

			_texture = FindViewById<TextureView> (Resource.Id.textureView);
			_texture.SurfaceTextureListener = this;

			mDrawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawer);
			mLeftDrawer = FindViewById<LinearLayout> (Resource.Id.left_drawer);

			toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			SetSupportActionBar (toolbar);

			mDrawerToggle = new MyActionBarDrawerToggle (this, mDrawerLayout, 
				Resource.String.openDrawer, Resource.String.closeDrawer);

			mDrawerLayout.SetDrawerListener (mDrawerToggle);
			SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			SupportActionBar.SetHomeButtonEnabled (true);
			SupportActionBar.SetDisplayShowTitleEnabled (true);
			mDrawerToggle.SyncState ();


		}
			

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			mDrawerToggle.OnOptionsItemSelected(item);
			return base.OnOptionsItemSelected(item);
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

		public Android.Hardware.Camera getCameraInstance(){
			Android.Hardware.Camera c = null;

			try {
						c = openFrontFacingCamera();
			}
			catch (System.Exception x){
				
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
			fake_button.Click += delegate {
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
