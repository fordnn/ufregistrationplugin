using System;
using System.Data;

namespace forDNN.Modules.ufRegistrationPlugin
{
	public class ResponseInfo
	{
		#region Private Members

		private bool _success;
		private string _innerHTML;

		#endregion

		#region Constructors

		public ResponseInfo()
		{
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// True on success AJAX response, False on any fail
		/// </summary>
		public bool success
		{
			get
			{
				return _success;
			}
			set
			{
				_success = value;
			}
		}

		/// <summary>
		/// AJAX response HTML
		/// </summary>
		public string innerHTML
		{
			get
			{
				return _innerHTML;
			}
			set
			{
				_innerHTML = value;
			}
		}

		#endregion
	}
}		
