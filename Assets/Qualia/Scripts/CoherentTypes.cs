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
	public string Id;
	public string Error;
	
}

//GENERIC TYPES

[CoherentType(PropertyBindingFlags.All)]
public struct SOptions
{
	public string String0;
}