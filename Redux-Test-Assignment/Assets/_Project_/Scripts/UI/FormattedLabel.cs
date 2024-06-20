using System;
using TMPro;
using UnityEngine;

namespace CustomExtensions.UIExtensions
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class FormattedLabel : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _label;
		[SerializeField, TextArea] private string _format;
		[SerializeField] private string[] _formatPreviewValues;

		public Color Color
		{
			get => _label.color;
			set => _label.color = value;
		}
		
		public string Format
		{
			get => _format;
			set => _format = value;
		}

		public void SetValues(params object[] args)
		{
			_label.text = string.Format(_format, args);
		}
		
		public void SetValues(object arg)
		{
			_label.text = string.Format(_format, arg);
		}
		
		public void SetValues(object arg0, object arg1)
		{
			_label.text = string.Format(_format, arg0, arg1);
		}

		private void Reset()
		{
			_label = GetComponent<TextMeshProUGUI>();
			_format = _label.text;
		}

		private void OnValidate()
		{
			try
			{
				if (!Application.isPlaying && _formatPreviewValues != null)
					_label.text = string.Format(_format, _formatPreviewValues);
			}
			catch (FormatException)
			{
			}
		}
	}
}