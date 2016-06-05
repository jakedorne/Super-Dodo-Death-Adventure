using UnityEngine;

namespace Chromatica.Utils
{
	public class InternalColorSpace
	{
		public static bool IsLinear { get { return QualitySettings.activeColorSpace == ColorSpace.Linear; } }
		public static RenderTextureReadWrite RTrw { get { return IsLinear ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.Default; } }
		public static bool TEXrw { get { return IsLinear; } }
	}
}