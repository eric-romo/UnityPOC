using UnityEngine;
using System.Collections;

public static class Utilities
{
	public static float mapRange(float a1,float a2,float b1,float b2,float s)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}

