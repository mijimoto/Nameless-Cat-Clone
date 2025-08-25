using UnityEngine;

namespace SpriteGlow
{
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	public class SpriteGlow : MonoBehaviour
	{
		[SerializeField]
		private Color _glowColor;

		[SerializeField]
		private float _glowBrightness;

		[SerializeField]
		private int _outlineWidth;

		[SerializeField]
		private float _alphaThreshold;

		[SerializeField]
		private bool _drawOutside;

		[SerializeField]
		private bool _enableInstancing;

		private SpriteRenderer spriteRenderer;

		private MaterialPropertyBlock materialProperties;

		private int isOutlineEnabledId;

		private int outlineColorId;

		private int outlineSizeId;

		private int alphaThresholdId;

		public SpriteRenderer Renderer => null;

		public Color GlowColor
		{
			get
			{
				return default(Color);
			}
			set
			{
			}
		}

		public float GlowBrightness
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public int OutlineWidth
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public float AlphaThreshold
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public bool DrawOutside
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool EnableInstancing
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void OnValidate()
		{
		}

		private void OnDidApplyAnimationProperties()
		{
		}

		private void SetMaterialProperties()
		{
		}
	}
}
