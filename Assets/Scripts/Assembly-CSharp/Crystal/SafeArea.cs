using UnityEngine;

namespace Crystal
{
	public class SafeArea : MonoBehaviour
	{
		public enum SimDevice
		{
			None = 0,
			iPhoneX = 1,
			iPhoneXsMax = 2,
			Pixel3XL_LSL = 3,
			Pixel3XL_LSR = 4
		}

		public static SimDevice Sim;

		private Rect[] NSA_iPhoneX;

		private Rect[] NSA_iPhoneXsMax;

		private Rect[] NSA_Pixel3XL_LSL;

		private Rect[] NSA_Pixel3XL_LSR;

		private RectTransform Panel;

		private Rect LastSafeArea;

		[SerializeField]
		private bool ConformX;

		[SerializeField]
		private bool ConformY;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void Refresh()
		{
		}

		private Rect GetSafeArea()
		{
			return default(Rect);
		}

		private void ApplySafeArea(Rect r)
		{
		}
	}
}
