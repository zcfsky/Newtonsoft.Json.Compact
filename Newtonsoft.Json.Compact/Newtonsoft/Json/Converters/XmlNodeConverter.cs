using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000021 RID: 33
	public class XmlNodeConverter : JsonConverter
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000062DC File Offset: 0x000044DC
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000062E4 File Offset: 0x000044E4
		public string DeserializeRootElementName { get; set; }

		// Token: 0x0600017E RID: 382 RVA: 0x000062F0 File Offset: 0x000044F0
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			XmlNode xmlNode = value as XmlNode;
			if (xmlNode == null)
			{
				throw new ArgumentException("Value must be an XmlNode", "value");
			}
			writer.WriteStartObject();
			this.SerializeNode(writer, xmlNode, true);
			writer.WriteEndObject();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000632C File Offset: 0x0000452C
		private string GetPropertyName(XmlNode node)
		{
			switch ((int)node.NodeType)
			{
			case 1:
				return node.Name;
			case 2:
				return "@" + node.Name;
			case 3:
				return "#text";
			case 4:
				return "#cdata-section";
			case 7:
				return "?" + node.Name;
			case 8:
				return "#comment";
			case 13:
				return "#whitespace";
			case 14:
				return "#significant-whitespace";
			case 17:
				return "?xml";
			}
			throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000063F8 File Offset: 0x000045F8
		private void SerializeGroupedNodes(JsonWriter writer, XmlNode node)
		{
			Dictionary<string, List<XmlNode>> dictionary = new Dictionary<string, List<XmlNode>>();
			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				XmlNode xmlNode = node.ChildNodes[i];
				string propertyName = this.GetPropertyName(xmlNode);
				List<XmlNode> list;
				if (!dictionary.TryGetValue(propertyName, out list))
				{
					list = new List<XmlNode>();
					dictionary.Add(propertyName, list);
				}
				list.Add(xmlNode);
			}
			foreach (KeyValuePair<string, List<XmlNode>> keyValuePair in dictionary)
			{
				List<XmlNode> value = keyValuePair.Value;
				bool flag;
				if (value.Count == 1)
				{
					XmlNode xmlNode2 = value[0];
					XmlAttribute xmlAttribute = (xmlNode2.Attributes != null) ? xmlNode2.Attributes["Array", "http://james.newtonking.com/projects/json"] : null;
					flag = (xmlAttribute != null && XmlConvert.ToBoolean(xmlAttribute.Value));
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					this.SerializeNode(writer, value[0], true);
				}
				else
				{
					string key = keyValuePair.Key;
					writer.WritePropertyName(keyValuePair.Key);
					writer.WriteStartArray();
					for (int j = 0; j < value.Count; j++)
					{
						this.SerializeNode(writer, value[j], false);
					}
					writer.WriteEndArray();
				}
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000657C File Offset: 0x0000477C
		private void SerializeNode(JsonWriter writer, XmlNode node, bool writePropertyName)
		{
			switch ((int)node.NodeType)
			{
			case 1:
				if (writePropertyName)
				{
					writer.WritePropertyName(node.Name);
				}
				if (Enumerable.Count<XmlAttribute>(this.ValueAttributes(node.Attributes)) == 0 && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == (XmlNodeType)3)
				{
					writer.WriteValue(node.ChildNodes[0].Value);
					return;
				}
				if (node.ChildNodes.Count == 0 && CollectionUtils.IsNullOrEmpty(node.Attributes))
				{
					writer.WriteNull();
					return;
				}
				if (Enumerable.Count<XmlElement>(Enumerable.Where<XmlElement>(Enumerable.OfType<XmlElement>(node.ChildNodes), (XmlElement x) => x.Name.StartsWith("-"))) > 1)
				{
					XmlElement xmlElement = Enumerable.First<XmlElement>(Enumerable.Where<XmlElement>(Enumerable.OfType<XmlElement>(node.ChildNodes), (XmlElement x) => x.Name.StartsWith("-")));
					string name = xmlElement.Name.Substring(1);
					writer.WriteStartConstructor(name);
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						this.SerializeNode(writer, node.ChildNodes[i], false);
					}
					writer.WriteEndConstructor();
					return;
				}
				writer.WriteStartObject();
				for (int j = 0; j < node.Attributes.Count; j++)
				{
					this.SerializeNode(writer, node.Attributes[j], true);
				}
				this.SerializeGroupedNodes(writer, node);
				writer.WriteEndObject();
				return;
			case 2:
			case 3:
			case 4:
			case 7:
			case 13:
			case 14:
				if (node.Prefix == "xmlns" && node.Value == "http://james.newtonking.com/projects/json")
				{
					return;
				}
				if (node.NamespaceURI == "http://james.newtonking.com/projects/json")
				{
					return;
				}
				if (writePropertyName)
				{
					writer.WritePropertyName(this.GetPropertyName(node));
				}
				writer.WriteValue(node.Value);
				return;
			case 8:
				if (writePropertyName)
				{
					writer.WriteComment(node.Value);
					return;
				}
				return;
			case 9:
			case 11:
				this.SerializeGroupedNodes(writer, node);
				return;
			case 17:
			{
				XmlDeclaration xmlDeclaration = (XmlDeclaration)node;
				writer.WritePropertyName(this.GetPropertyName(node));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDeclaration.Version))
				{
					writer.WritePropertyName("@version");
					writer.WriteValue(xmlDeclaration.Version);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
				{
					writer.WritePropertyName("@encoding");
					writer.WriteValue(xmlDeclaration.Encoding);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
				{
					writer.WritePropertyName("@standalone");
					writer.WriteValue(xmlDeclaration.Standalone);
				}
				writer.WriteEndObject();
				return;
			}
			}
			throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000686C File Offset: 0x00004A6C
		public override object ReadJson(JsonReader reader, Type objectType, JsonSerializer serializer)
		{
			if (objectType != typeof(XmlDocument))
			{
				throw new JsonSerializationException("XmlNodeConverter only supports deserializing XmlDocuments");
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlNamespaceManager manager = new XmlNamespaceManager(xmlDocument.NameTable);
			XmlNode xmlNode;
			if (!string.IsNullOrEmpty(this.DeserializeRootElementName))
			{
				xmlNode = xmlDocument.CreateElement(this.DeserializeRootElementName);
				xmlDocument.AppendChild(xmlNode);
			}
			else
			{
				xmlNode = xmlDocument;
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");
			}
			reader.Read();
			this.DeserializeNode(reader, xmlDocument, manager, xmlNode);
			return xmlDocument;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x000068F0 File Offset: 0x00004AF0
		private void DeserializeValue(JsonReader reader, XmlDocument document, XmlNamespaceManager manager, string propertyName, XmlNode currentNode)
		{
			if (propertyName != null)
			{
				if (propertyName == "#text")
				{
					currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
					return;
				}
				if (propertyName == "#cdata-section")
				{
					currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
					return;
				}
				if (propertyName == "#whitespace")
				{
					currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
					return;
				}
				if (propertyName == "#significant-whitespace")
				{
					currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
					return;
				}
			}
			if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
			{
				if (propertyName == "?xml")
				{
					string text = null;
					string text2 = null;
					string text3 = null;
					while (reader.Read() && reader.TokenType != JsonToken.EndObject)
					{
						string text4;
						if ((text4 = reader.Value.ToString()) != null)
						{
							if (text4 == "@version")
							{
								reader.Read();
								text = reader.Value.ToString();
								continue;
							}
							if (text4 == "@encoding")
							{
								reader.Read();
								text2 = reader.Value.ToString();
								continue;
							}
							if (text4 == "@standalone")
							{
								reader.Read();
								text3 = reader.Value.ToString();
								continue;
							}
						}
						throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
					}
					XmlDeclaration xmlDeclaration = document.CreateXmlDeclaration(text, text2, text3);
					currentNode.AppendChild(xmlDeclaration);
					return;
				}
				XmlProcessingInstruction xmlProcessingInstruction = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
				currentNode.AppendChild(xmlProcessingInstruction);
				return;
			}
			else
			{
				bool flag = false;
				bool flag2 = false;
				string prefix = this.GetPrefix(propertyName);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (reader.TokenType == JsonToken.StartArray)
				{
					XmlElement xmlElement = this.CreateElement(propertyName, document, prefix, manager);
					currentNode.AppendChild(xmlElement);
					while (reader.Read() && reader.TokenType != JsonToken.EndArray)
					{
						this.DeserializeValue(reader, document, manager, propertyName, xmlElement);
					}
					return;
				}
				if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null && reader.TokenType != JsonToken.Boolean && reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Date && reader.TokenType != JsonToken.StartConstructor)
				{
					while (!flag && !flag2 && reader.Read())
					{
						JsonToken tokenType = reader.TokenType;
						if (tokenType != JsonToken.PropertyName)
						{
							if (tokenType != JsonToken.EndObject)
							{
								throw new JsonSerializationException("Unexpected JsonToken: " + reader.TokenType);
							}
							flag2 = true;
						}
						else
						{
							string text5 = reader.Value.ToString();
							if (text5[0] == '@')
							{
								text5 = text5.Substring(1);
								reader.Read();
								string text6 = reader.Value.ToString();
								dictionary.Add(text5, text6);
								string text7;
								if (this.IsNamespaceAttribute(text5, out text7))
								{
									manager.AddNamespace(text7, text6);
								}
							}
							else
							{
								flag = true;
							}
						}
					}
				}
				XmlElement xmlElement2 = this.CreateElement(propertyName, document, prefix, manager);
				currentNode.AppendChild(xmlElement2);
				foreach (KeyValuePair<string, string> keyValuePair in dictionary)
				{
					string prefix2 = this.GetPrefix(keyValuePair.Key);
					XmlAttribute xmlAttribute = (!string.IsNullOrEmpty(prefix2)) ? document.CreateAttribute(keyValuePair.Key, manager.LookupNamespace(prefix2)) : document.CreateAttribute(keyValuePair.Key);
					xmlAttribute.Value = keyValuePair.Value;
					xmlElement2.SetAttributeNode(xmlAttribute);
				}
				if (reader.TokenType == JsonToken.String)
				{
					xmlElement2.AppendChild(document.CreateTextNode(reader.Value.ToString()));
					return;
				}
				if (reader.TokenType == JsonToken.Integer)
				{
					xmlElement2.AppendChild(document.CreateTextNode(XmlConvert.ToString((long)reader.Value)));
					return;
				}
				if (reader.TokenType == JsonToken.Float)
				{
					xmlElement2.AppendChild(document.CreateTextNode(XmlConvert.ToString((double)reader.Value)));
					return;
				}
				if (reader.TokenType == JsonToken.Boolean)
				{
					xmlElement2.AppendChild(document.CreateTextNode(XmlConvert.ToString((bool)reader.Value)));
					return;
				}
				if (reader.TokenType == JsonToken.Date)
				{
					DateTime dateTime = (DateTime)reader.Value;
					xmlElement2.AppendChild(document.CreateTextNode(XmlConvert.ToString(dateTime, DateTimeUtils.ToSerializationMode(dateTime.Kind))));
					return;
				}
				if (reader.TokenType == JsonToken.Null)
				{
					return;
				}
				if (!flag2)
				{
					manager.PushScope();
					this.DeserializeNode(reader, document, manager, xmlElement2);
					manager.PopScope();
				}
				return;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006DC8 File Offset: 0x00004FC8
		private XmlElement CreateElement(string elementName, XmlDocument document, string elementPrefix, XmlNamespaceManager manager)
		{
			if (string.IsNullOrEmpty(elementPrefix))
			{
				return document.CreateElement(elementName);
			}
			return document.CreateElement(elementName, manager.LookupNamespace(elementPrefix));
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006DEC File Offset: 0x00004FEC
		private void DeserializeNode(JsonReader reader, XmlDocument document, XmlNamespaceManager manager, XmlNode currentNode)
		{
			JsonToken tokenType;
			for (;;)
			{
				tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.StartConstructor:
				{
					string text = reader.Value.ToString();
					while (reader.Read())
					{
						if (reader.TokenType == JsonToken.EndConstructor)
						{
							break;
						}
						this.DeserializeValue(reader, document, manager, "-" + text, currentNode);
					}
					goto IL_EB;
				}
				case JsonToken.PropertyName:
				{
                    if ((int)currentNode.NodeType == 9 && document.DocumentElement != null)
					{
						goto Block_3;
					}
					string propertyName = reader.Value.ToString();
					reader.Read();
					if (reader.TokenType == JsonToken.StartArray)
					{
						while (reader.Read())
						{
							if (reader.TokenType == JsonToken.EndArray)
							{
								break;
							}
							this.DeserializeValue(reader, document, manager, propertyName, currentNode);
						}
						goto IL_EB;
					}
					this.DeserializeValue(reader, document, manager, propertyName, currentNode);
					goto IL_EB;
				}
				}
				break;
				IL_EB:
				if (reader.TokenType != JsonToken.PropertyName && !reader.Read())
				{
					return;
				}
			}
			switch (tokenType)
			{
			case JsonToken.EndObject:
			case JsonToken.EndArray:
				return;
			default:
				throw new JsonSerializationException("Unexpected JsonToken when deserializing node: " + reader.TokenType);
			}
			Block_3:
			throw new JsonSerializationException("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006EFC File Offset: 0x000050FC
		private bool IsNamespaceAttribute(string attributeName, out string prefix)
		{
			if (attributeName.StartsWith("xmlns", (StringComparison)4))
			{
				if (attributeName.Length == 5)
				{
					prefix = string.Empty;
					return true;
				}
				if (attributeName[5] == ':')
				{
					prefix = attributeName.Substring(6, attributeName.Length - 6);
					return true;
				}
			}
			prefix = null;
			return false;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006F4C File Offset: 0x0000514C
		private string GetPrefix(string qualifiedName)
		{
			int num = qualifiedName.IndexOf(':');
			if (num == -1 || num == 0 || qualifiedName.Length - 1 == num)
			{
				return string.Empty;
			}
			return qualifiedName.Substring(0, num);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006F94 File Offset: 0x00005194
		private IEnumerable<XmlAttribute> ValueAttributes(XmlAttributeCollection c)
		{
			return Enumerable.Where<XmlAttribute>(Enumerable.OfType<XmlAttribute>(c), (XmlAttribute a) => a.NamespaceURI != "http://james.newtonking.com/projects/json");
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00006FD0 File Offset: 0x000051D0
		private IEnumerable<XmlNode> ValueNodes(XmlNodeList c)
		{
			return Enumerable.Where<XmlNode>(Enumerable.OfType<XmlNode>(c), (XmlNode n) => n.NamespaceURI != "http://james.newtonking.com/projects/json");
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006FFA File Offset: 0x000051FA
		public override bool CanConvert(Type valueType)
		{
			return typeof(XmlNode).IsAssignableFrom(valueType);
		}

		// Token: 0x04000087 RID: 135
		private const string TextName = "#text";

		// Token: 0x04000088 RID: 136
		private const string CommentName = "#comment";

		// Token: 0x04000089 RID: 137
		private const string CDataName = "#cdata-section";

		// Token: 0x0400008A RID: 138
		private const string WhitespaceName = "#whitespace";

		// Token: 0x0400008B RID: 139
		private const string SignificantWhitespaceName = "#significant-whitespace";

		// Token: 0x0400008C RID: 140
		private const string DeclarationName = "?xml";

		// Token: 0x0400008D RID: 141
		private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";
	}
}
