﻿using System;
using System.Windows.Forms;
using AssetsAdvancedEditor.Assets;
using AssetsTools.NET;

namespace AssetsAdvancedEditor.Winforms
{
    public partial class AssetDataViewer : Form
    {
        private readonly AssetTypeValueField _baseField;
        private readonly uint _format;
        private readonly string _rootDir;

        public AssetDataViewer(AssetsWorkspace workspace, AssetTypeValueField baseField)
        {
            InitializeComponent();
            _format = workspace.MainFile.file.header.format;
            _baseField = baseField;
            _rootDir = workspace.AssetsRootDir;
            PopulateTree();
        }

        private void PopulateTree()
        {
            var type = _baseField.GetFieldType();
            if (type == "MonoBehaviour")
            {
                //var desMonos = MonoLoader.TryDeserializeMono(_baseField, _format, _rootDir);
                //if (desMonos != null) // not tested
                //{
                //    var newTemplateField = _baseField.templateField;
                //    newTemplateField.children = _baseField.templateField.children.Concat(desMonos).ToArray();
                //    newTemplateField.childrenCount = newTemplateField.children.Length;

                //    _baseField.templateField = newTemplateField;
                //}
            }

            rawViewTree.Nodes.Add(type + " " + _baseField.GetName());
            RecursiveTreeLoad(_baseField, rawViewTree.Nodes[0]);
        }

        private static void RecursiveTreeLoad(AssetTypeValueField assetField, TreeNode node)
        {
            if (assetField.childrenCount == 0) return;
            foreach (var children in assetField.children)
            {
                if (children == null) return;
                var value = "";
                if (children.GetValue() != null)
                {
                    var evt = children.GetValue().GetValueType();
                    var quote = "";
                    if (evt == EnumValueTypes.String) quote = "\"";
                    if (1 <= (int)evt && (int)evt <= 12) value = $" = {quote}{children.GetValue().AsString()}{quote}";
                    var isOneItem = children.childrenCount == 1;
                    if (evt is EnumValueTypes.Array or EnumValueTypes.ByteArray)
                        value = $" ({children.childrenCount} {(isOneItem ? "item" : "items")})";
                }

                node.Nodes.Add($"{children.GetFieldType()} {children.GetName() + value}");
                RecursiveTreeLoad(children, node.LastNode);
            }
        }

        private void openAll_Click(object sender, EventArgs e) => rawViewTree?.ExpandAll();

        private void closeAll_Click(object sender, EventArgs e) => rawViewTree?.CollapseAll();

        private void openDown_Click(object sender, EventArgs e) => rawViewTree.SelectedNode?.ExpandAll();

        private void closeDown_Click(object sender, EventArgs e) => rawViewTree.SelectedNode?.Collapse(false);
    }
}