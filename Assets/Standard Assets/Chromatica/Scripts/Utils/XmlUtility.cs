using System.Xml;

namespace Chromatica.Utils
{
	public static class XmlUtility
	{
		public static string Attribute(this XmlNode node, string name)
		{
			XmlAttribute attr = (XmlAttribute)node.Attributes.GetNamedItem(name);
			return attr == null ? "" : attr.Value;
		}

		public static XmlNode GetChild(this XmlNode node, string name)
		{
			XmlNode child = null;

			foreach (XmlNode subnode in node)
			{
				if (Attribute(subnode, "name") == name)
				{
					child = subnode;
					break;
				}
			}

			return child;
		}
	}
}
