using Coherent.UI.Binding;
// all properties / fields for Options will be visible to Coherent UI
[CoherentType(PropertyBindingFlags.All)]
public struct LaunchAppOptions
{
	public string Name;
}

[CoherentType(PropertyBindingFlags.All)]
public struct SwitchEnvironmentOptions
{
	public string Name;
}

[CoherentType(PropertyBindingFlags.All)]
public struct DisplayModelOptions
{
	public string Url;
}

[CoherentType(PropertyBindingFlags.All)]
public struct LoadModelOptions
{
	public string Url;
	public bool HasMtl;
}

[CoherentType(PropertyBindingFlags.All)]
public struct ScrollOptions
{
	public int ScrollTop;
}

[CoherentType(PropertyBindingFlags.All)]
public struct LoadModelReturn
{
	public string AssetId;
	public string Error;
	
}

[CoherentType(PropertyBindingFlags.All)]
public struct AddModelReturn
{
	public string ModelId;
	public string Error;
	
}

[CoherentType(PropertyBindingFlags.All)]
public struct ModelTransformOptions
{
	public float Duration;
	public string ModelId;
	public string TransformType;
	public float X;
	public float Y;
	public float Z;
	
}


//GENERIC TYPES

[CoherentType(PropertyBindingFlags.All)]
public struct SOptions
{
	public string String0;
}

[CoherentType(PropertyBindingFlags.All)]
public struct Vector3Options
{
	public float x;
	public float y;
	public float z;
}