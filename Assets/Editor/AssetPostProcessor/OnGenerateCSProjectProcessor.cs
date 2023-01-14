using System;
using UnityEditor;
using UnityEngine;
using System.Xml;
using System.IO;

public class OnGenerateCSProjectProcessor : AssetPostprocessor {
    /// <summary>
    /// 打开Unity项目工程时调用
    /// </summary>
    /// <param name="path"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string OnGeneratedCSProject(string path, string content) {
        if (path.EndsWith("Game.csproj")) {
            content = content.Replace("<Compile Include=\"Assets\\Scripts\\Empty.cs\" />", string.Empty);
            content = content.Replace("<None Include=\"Assets\\Scripts\\Game.asmdef\" />", string.Empty);
        }

        if (path.EndsWith("Game.csproj")) {
            return GenerateCustomProject(path, content, @"Code\**\*.cs");
        }

        return content;
    }

    private static string GenerateCustomProject(string path, string content, string codesPath) {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(content);

        var newDoc = doc.Clone() as XmlDocument;

        var rootNode = newDoc.GetElementsByTagName("Project")[0];

        var itemGroup = newDoc.CreateElement("ItemGroup", newDoc.DocumentElement.NamespaceURI);
        var compile = newDoc.CreateElement("Compile", newDoc.DocumentElement.NamespaceURI);

        compile.SetAttribute("Include", codesPath);
        itemGroup.AppendChild(compile);
        
        rootNode.AppendChild(itemGroup);

        using (StringWriter sw = new StringWriter()) {
            using (XmlTextWriter tx = new XmlTextWriter(sw)) {
                tx.Formatting = Formatting.Indented;
                newDoc.WriteTo(tx);
                tx.Flush();
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}