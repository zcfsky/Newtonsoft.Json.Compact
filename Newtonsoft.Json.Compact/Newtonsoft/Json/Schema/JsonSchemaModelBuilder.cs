using System;
using System.Collections.Generic;
using System.Linq;

namespace Newtonsoft.Json.Schema
{
	// Token: 0x0200003A RID: 58
	internal class JsonSchemaModelBuilder
	{
		// Token: 0x060003DF RID: 991 RVA: 0x0000DC7C File Offset: 0x0000BE7C
		public JsonSchemaModel Build(JsonSchema schema)
		{
			this._nodes = new JsonSchemaNodeCollection();
			this._node = this.AddSchema(null, schema);
			this._nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			return this.BuildNodeModel(this._node);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
		{
			string id;
			if (existingNode != null)
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				id = JsonSchemaNode.GetId(Enumerable.Union<JsonSchema>(existingNode.Schemas, new JsonSchema[]
				{
					schema
				}));
			}
			else
			{
				id = JsonSchemaNode.GetId(new JsonSchema[]
				{
					schema
				});
			}
			if (this._nodes.Contains(id))
			{
				return this._nodes[id];
			}
			JsonSchemaNode jsonSchemaNode = (existingNode != null) ? existingNode.Combine(schema) : new JsonSchemaNode(schema);
			this._nodes.Add(jsonSchemaNode);
			if (schema.Properties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> keyValuePair in schema.Properties)
				{
					this.AddProperty(jsonSchemaNode, keyValuePair.Key, keyValuePair.Value);
				}
			}
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.Count; i++)
				{
					this.AddItem(jsonSchemaNode, i, schema.Items[i]);
				}
			}
			if (schema.AdditionalProperties != null)
			{
				this.AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				jsonSchemaNode = this.AddSchema(jsonSchemaNode, schema.Extends);
			}
			return jsonSchemaNode;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000DE08 File Offset: 0x0000C008
		public void AddProperty(JsonSchemaNode parentNode, string propertyName, JsonSchema schema)
		{
			JsonSchemaNode existingNode;
			parentNode.Properties.TryGetValue(propertyName, out existingNode);
			parentNode.Properties[propertyName] = this.AddSchema(existingNode, schema);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000DE38 File Offset: 0x0000C038
		public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
		{
			JsonSchemaNode existingNode = (parentNode.Items.Count > index) ? parentNode.Items[index] : null;
			JsonSchemaNode jsonSchemaNode = this.AddSchema(existingNode, schema);
			if (parentNode.Items.Count <= index)
			{
				parentNode.Items.Add(jsonSchemaNode);
				return;
			}
			parentNode.Items[index] = jsonSchemaNode;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000DE94 File Offset: 0x0000C094
		public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalProperties = this.AddSchema(parentNode.AdditionalProperties, schema);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
		{
			JsonSchemaModel jsonSchemaModel;
			if (this._nodeModels.TryGetValue(node, out jsonSchemaModel))
			{
				return jsonSchemaModel;
			}
			jsonSchemaModel = JsonSchemaModel.Create(node.Schemas);
			this._nodeModels[node] = jsonSchemaModel;
			foreach (KeyValuePair<string, JsonSchemaNode> keyValuePair in node.Properties)
			{
				if (jsonSchemaModel.Properties == null)
				{
					jsonSchemaModel.Properties = new Dictionary<string, JsonSchemaModel>();
				}
				jsonSchemaModel.Properties[keyValuePair.Key] = this.BuildNodeModel(keyValuePair.Value);
			}
			for (int i = 0; i < node.Items.Count; i++)
			{
				if (jsonSchemaModel.Items == null)
				{
					jsonSchemaModel.Items = new List<JsonSchemaModel>();
				}
				jsonSchemaModel.Items.Add(this.BuildNodeModel(node.Items[i]));
			}
			if (node.AdditionalProperties != null)
			{
				jsonSchemaModel.AdditionalProperties = this.BuildNodeModel(node.AdditionalProperties);
			}
			return jsonSchemaModel;
		}

		// Token: 0x040000EA RID: 234
		private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();

		// Token: 0x040000EB RID: 235
		private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();

		// Token: 0x040000EC RID: 236
		private JsonSchemaNode _node;
	}
}
