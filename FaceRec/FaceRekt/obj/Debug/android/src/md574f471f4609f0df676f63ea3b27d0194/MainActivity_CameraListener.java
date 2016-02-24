package md574f471f4609f0df676f63ea3b27d0194;


public class MainActivity_CameraListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.hardware.Camera.PreviewCallback
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onPreviewFrame:([BLandroid/hardware/Camera;)V:GetOnPreviewFrame_arrayBLandroid_hardware_Camera_Handler:Android.Hardware.Camera/IPreviewCallbackInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("FaceRekt.MainActivity+CameraListener, FaceRekt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MainActivity_CameraListener.class, __md_methods);
	}


	public MainActivity_CameraListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MainActivity_CameraListener.class)
			mono.android.TypeManager.Activate ("FaceRekt.MainActivity+CameraListener, FaceRekt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPreviewFrame (byte[] p0, android.hardware.Camera p1)
	{
		n_onPreviewFrame (p0, p1);
	}

	private native void n_onPreviewFrame (byte[] p0, android.hardware.Camera p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
