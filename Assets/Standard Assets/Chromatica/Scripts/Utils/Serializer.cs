using UnityEngine;
using System.Xml;

namespace Chromatica.Utils
{
	public class Serializer
	{
		public static string SerializeBool(string name, bool value)
		{
			return string.Format("<Int name=\"{0}\" value=\"{1}\"/>", name, value.ToString());
		}

		public static string SerializeInt(string name, int value)
		{
			return string.Format("<Int name=\"{0}\" value=\"{1}\"/>", name, value.ToString());
		}

		public static string SerializeFloat(string name, float value)
		{
			return string.Format("<Float name=\"{0}\" value=\"{1}\"/>", name, value.ToString());
		}

		public static string SerializeString(string name, string value)
		{
			return string.Format("<Int name=\"{0}\" value=\"{1}\"/>", name, value);
		}

		public static string SerializeColor(string name, Color value)
		{
			return string.Format("<Color name=\"{0}\" r=\"{1}\" g=\"{2}\" b=\"{3}\" a=\"{4}\"/>", name, value.r.ToString(), value.g.ToString(), value.b.ToString(), value.a.ToString());
		}

		public static string SerializeVector2(string name, Vector2 value)
		{
			return string.Format("<Vector2 name=\"{0}\" x=\"{1}\" y=\"{2}\"/>", name, value.x.ToString(), value.y.ToString());
		}

		public static string SerializeVector3(string name, Vector3 value)
		{
			return string.Format("<Vector3 name=\"{0}\" x=\"{1}\" y=\"{2}\" z=\"{3}\"/>", name, value.x.ToString(), value.y.ToString(), value.z.ToString());
		}

		public static string SerializeVector4(string name, Vector4 value)
		{
			return string.Format("<Vector4 name=\"{0}\" x=\"{1}\" y=\"{2}\" z=\"{3}\" w=\"{4}\"/>", name, value.x.ToString(), value.y.ToString(), value.z.ToString(), value.w.ToString());
		}

		public static string SerializeCurve(string name, AnimationCurve curve)
		{
			string data = string.Format("<Curve name=\"{0}\">\n", name);

			data += string.Format("\t\t\t{0}\n", SerializeInt("preWrapMode", (int)curve.preWrapMode));
			data += string.Format("\t\t\t{0}\n", SerializeInt("postWrapMode", (int)curve.postWrapMode));
			data += string.Format("\t\t\t<KeyFrames count=\"{0}\">\n", curve.keys.Length);

			foreach (Keyframe key in curve.keys)
				data += string.Format("\t\t\t\t<Key value=\"{0}\" time=\"{1}\" inTangent=\"{2}\" outTangent=\"{3}\" tangentMode=\"{4}\"/>\n", key.value, key.time, key.inTangent, key.outTangent, key.tangentMode);

			data += "\t\t\t</KeyFrames>\n";
			data += "\t\t</Curve>";
			return data;
		}

		public static string SerializeGradient(string name, Gradient gradient)
		{
			string data = string.Format("<Gradient name=\"{0}\">\n", name);
			data += string.Format("\t\t\t<ColorKeys count=\"{0}\">\n", gradient.colorKeys.Length);

			foreach (GradientColorKey key in gradient.colorKeys)
				data += string.Format("\t\t\t\t<Key time=\"{0}\" r=\"{1}\" g=\"{2}\" b=\"{3}\"/>\n", key.time, key.color.r, key.color.g, key.color.b);

			data += "\t\t\t</ColorKeys>\n";
			data += string.Format("\t\t\t<AlphaKeys count=\"{0}\">\n", gradient.alphaKeys.Length);

			foreach (GradientAlphaKey key in gradient.alphaKeys)
				data += string.Format("\t\t\t\t<Key time=\"{0}\" alpha=\"{1}\" />\n", key.time, key.alpha);

			data += "\t\t\t</AlphaKeys>\n";
			data += "\t\t</Gradient>";
			return data;
		}

		public static bool DeserializeBool(XmlNode node, bool defaultValue)
		{
			if (node == null)
				return defaultValue;

			bool value = false;
			if (!bool.TryParse(node.Attribute("value"), out value))
				return defaultValue;
			return value;
		}

		public static int DeserializeInt(XmlNode node, int defaultValue)
		{
			if (node == null)
				return defaultValue;

			int value = 0;
			if (!int.TryParse(node.Attribute("value"), out value))
				return defaultValue;
			return value;
		}

		public static float DeserializeFloat(XmlNode node, float defaultValue)
		{
			if (node == null)
				return defaultValue;

			float value = 0;
			if (!float.TryParse(node.Attribute("value"), out value))
				return defaultValue;
			return value;
		}

		public static string DeserializeString(XmlNode node, string defaultValue)
		{
			if (node == null)
				return defaultValue;

			return node.Attribute("value");
		}

		public static Color DeserializeColor(XmlNode node, Color defaultValue)
		{
			if (node == null)
				return defaultValue;

			Color value = new Color();
			if (!float.TryParse(node.Attribute("r"), out value.r))
				value.r = defaultValue.r;
			if (!float.TryParse(node.Attribute("g"), out value.g))
				value.g = defaultValue.g;
			if (!float.TryParse(node.Attribute("b"), out value.b))
				value.b = defaultValue.b;
			if (!float.TryParse(node.Attribute("a"), out value.a))
				value.a = defaultValue.a;
			return value;
		}

		public static Vector2 DeserializeVector2(XmlNode node, Vector2 defaultValue)
		{
			if (node == null)
				return defaultValue;

			Vector2 value = new Vector2();
			if (!float.TryParse(node.Attribute("x"), out value.x))
				value.x = defaultValue.x;
			if (!float.TryParse(node.Attribute("y"), out value.y))
				value.y = defaultValue.y;
			return value;
		}

		public static Vector3 DeserializeVector3(XmlNode node, Vector3 defaultValue)
		{
			if (node == null)
				return defaultValue;

			Vector3 value = new Vector3();
			if (!float.TryParse(node.Attribute("x"), out value.x))
				value.x = defaultValue.x;
			if (!float.TryParse(node.Attribute("y"), out value.y))
				value.y = defaultValue.y;
			if (!float.TryParse(node.Attribute("z"), out value.z))
				value.z = defaultValue.z;
			return value;
		}

		public static Vector4 DeserializeVector4(XmlNode node, Vector4 defaultValue)
		{
			if (node == null)
				return defaultValue;

			Vector4 value = new Vector4();
			if (!float.TryParse(node.Attribute("x"), out value.x))
				value.x = defaultValue.x;
			if (!float.TryParse(node.Attribute("y"), out value.y))
				value.y = defaultValue.y;
			if (!float.TryParse(node.Attribute("z"), out value.z))
				value.z = defaultValue.z;
			if (!float.TryParse(node.Attribute("w"), out value.w))
				value.w = defaultValue.w;
			return value;
		}

		public static AnimationCurve DeserializeCurve(XmlNode node, AnimationCurve defaultValue)
		{
			if (node == null)
				return defaultValue;

			WrapMode preWrapMode = (WrapMode)DeserializeInt(node.GetChild("preWrapMode"), (int)defaultValue.preWrapMode);
			WrapMode postWrapMode = (WrapMode)DeserializeInt(node.GetChild("postWrapMode"), (int)defaultValue.postWrapMode);

			XmlNode keyframes = node.SelectSingleNode("KeyFrames");
			int c = 0;
			int.TryParse(keyframes.Attribute("count"), out c);
			Keyframe[] keys = new Keyframe[c];

			int i = 0;
			foreach (XmlNode key in keyframes)
			{
				float value = 0f;
				float time = 0f;
				float inTangent = 0f;
				float outTangent = 0f;
				int tangentMode = 0;

				float.TryParse(key.Attribute("value"), out value);
				float.TryParse(key.Attribute("time"), out time);
				float.TryParse(key.Attribute("inTangent"), out inTangent);
				float.TryParse(key.Attribute("outTangent"), out outTangent);
				int.TryParse(key.Attribute("tangentMode"), out tangentMode);

				keys[i] = new Keyframe(time, value, inTangent, outTangent);
				keys[i].tangentMode = tangentMode;
				i++;
			}

			AnimationCurve curve = new AnimationCurve(keys);
			curve.preWrapMode = preWrapMode;
			curve.postWrapMode = postWrapMode;
			return curve;
		}

		public static Gradient DeserializeGradient(XmlNode node, Gradient defaultValue)
		{
			if (node == null)
				return defaultValue;

			XmlNode colorkeys = node.SelectSingleNode("ColorKeys");
			XmlNode alphakeys = node.SelectSingleNode("AlphaKeys");

			int c1 = 0, c2 = 0;
			int.TryParse(colorkeys.Attribute("count"), out c1);
			int.TryParse(alphakeys.Attribute("count"), out c2);

			GradientColorKey[] ckeys = new GradientColorKey[c1];
			GradientAlphaKey[] akeys = new GradientAlphaKey[c2];

			int i = 0;
			foreach (XmlNode key in colorkeys)
			{
				float time = 0f;
				Color color = Color.white;

				float.TryParse(key.Attribute("time"), out time);
				float.TryParse(key.Attribute("r"), out color.r);
				float.TryParse(key.Attribute("g"), out color.g);
				float.TryParse(key.Attribute("b"), out color.b);

				ckeys[i] = new GradientColorKey(color, time);
				i++;
			}

			i = 0;
			foreach (XmlNode key in alphakeys)
			{
				float time = 0f;
				float alpha = 0f;

				float.TryParse(key.Attribute("time"), out time);
				float.TryParse(key.Attribute("alpha"), out alpha);

				akeys[i] = new GradientAlphaKey(alpha, time);
				i++;
			}

			Gradient gradient = new Gradient();
			gradient.SetKeys(ckeys, akeys);
			return gradient;
		}
	}
}
