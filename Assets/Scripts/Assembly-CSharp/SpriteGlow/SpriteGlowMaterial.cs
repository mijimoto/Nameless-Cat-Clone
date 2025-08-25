using System.Collections.Generic;
using UnityEngine;

namespace SpriteGlow
{
	public class SpriteGlowMaterial : Material
	{
		private static readonly string OUTLINE_SHADER_NAME;

		private static readonly string OUTSIDE_MATERIAL_KEYWORD;

		private static readonly Shader OUTLINE_SHADER;

		private static List<SpriteGlowMaterial> sharedMaterials;

		public Texture SpriteTexture => null;

		public bool DrawOutside => false;

		public bool InstancingEnabled => false;

		public SpriteGlowMaterial(Texture spriteTexture, bool drawOutside = false, bool instancingEnabled = false)
			: base((Shader)null)
		{
		}

		public static Material GetSharedFor(SpriteGlow spriteGlow)
		{
			return null;
		}
	}
}
