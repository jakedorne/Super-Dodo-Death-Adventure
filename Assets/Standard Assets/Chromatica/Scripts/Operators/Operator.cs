using UnityEngine;
using System;
using System.Xml;
using Chromatica.Utils;

namespace Chromatica.Operators
{
	[Serializable]
	public class Operator : ScriptableObject
	{
		public string Title = "Null Operator";
		public bool IsUnfolded = true;
		public bool IsEnabled = true;

		public virtual void Init() { }
		public virtual void ResetToDefault() { }
		public virtual Operator Clone() { return null; }
		public virtual string ToXML() { return ""; }
		public virtual void FromXML(XmlNode xmlNode) { }
	}
}
