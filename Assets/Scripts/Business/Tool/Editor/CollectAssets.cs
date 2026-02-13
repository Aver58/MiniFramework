using System.Collections.Generic;
using System.Linq;
using NPOI.HSSF.UserModel;
using Scripts.Framework.UI;
using UnityEditor;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Bussiness.Tool.Editor {
    public class CollectAssets {
        // 收集 Assets\ToBundle\Atlas 目录下的所有 .spriteatlas 文件到项目外文件夹 MiniFramework\LuBanTools\Datas\#SpriteAtlas.xlsx
        [MenuItem("Tools/CollectAssets/CollectSpriteAtlas")]
        public static void CollectSpriteAtlas() {
            var spriteAtlasDirectory = ImageExtension.SpriteAtlasDirectory;
            // 项目外文件夹 MiniFramework\LuBanTools\Datas\#SpriteAtlas.xlsx
            // 使用项目根目录（Assets 的父目录）作为基准，生成绝对路径
            var projectRoot = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
            string originFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(projectRoot, "LuBanTools", "Datas", "#SpriteAtlas.xlsx"));
            if (!System.IO.Directory.Exists(spriteAtlasDirectory)) {
                Debug.LogError($"Directory not found: {spriteAtlasDirectory}");
                return;
            }

            var atlasFiles = System.IO.Directory.GetFiles(spriteAtlasDirectory, "*.spriteatlas", System.IO.SearchOption.AllDirectories);
            if (atlasFiles.Length == 0) {
                Debug.LogWarning("No sprite atlas files found.");
                return;
            }

            var constLineLength = 4;
            // 确保输出目录存在
            var originDir = System.IO.Path.GetDirectoryName(originFilePath);
            if (!string.IsNullOrEmpty(originDir) && !System.IO.Directory.Exists(originDir)) {
                System.IO.Directory.CreateDirectory(originDir);
            }

            IWorkbook excel;
            ISheet sheet;
            // 打开或创建 xlsx
            // if (!System.IO.File.Exists(originFilePath)) {
            //     excel = new XSSFWorkbook();
            //     sheet = excel.CreateSheet("Sheet1");
            //     for (int r = 0; r < constLineLength; r++) {
            //         sheet.CreateRow(r);
            //     }
            // } else {
            //     using (var fs = new System.IO.FileStream(originFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read)) {
            //         if (originFilePath.Equals(".xls")) {
            //             excel = new HSSFWorkbook(fs); //xls
            //         } else {
            //             excel = new XSSFWorkbook(fs); //xlsx
            //         }
            //     }
            //     sheet = excel.NumberOfSheets > 0 ? excel.GetSheetAt(0) : excel.CreateSheet("Sheet1");
            // }
            //
            // if (sheet == null) {
            //     Debug.LogError($"[翻译]没有找到 {originFilePath} 的 第一个分页为空！");
            //     return;
            // }
            //
            // // 校验最少行数
            // int existingRows = sheet.LastRowNum + 1;
            // if (existingRows < constLineLength) {
            //     Debug.LogError($"XLSX file `{originFilePath}` does not have enough rows. Expected at least {constLineLength} rows.");
            //     return;
            // }
            //
            // // 清除之前的内容（从 constLineLength 行开始）
            // for (int r = sheet.LastRowNum; r >= constLineLength; r--) {
            //     var row = sheet.GetRow(r);
            //     if (row != null) sheet.RemoveRow(row);
            // }
            //
            // // 写入新条目，第一列为索引，第二列为图集名
            // for (int i = 0; i < atlasFiles.Length; i++) {
            //     var filePath = atlasFiles[i];
            //     var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(filePath);
            //     int newRowIndex = constLineLength + i;
            //     var row = sheet.GetRow(newRowIndex) ?? sheet.CreateRow(newRowIndex);
            //     row.CreateCell(0).SetCellValue(i);
            //     row.CreateCell(1).SetCellValue(fileNameWithoutExtension);
            // }
            //
            // // 保存 xlsx
            // using (var fsOut = new System.IO.FileStream(originFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
            //     excel.Write(fsOut);
            // }
            //
            // UnityEngine.Debug.Log($"SpriteAtlas collect completed. Output: `{originFilePath}`");
        }


        // 收集所有图集文件的Sprite文件到 MiniFramework\LuBanTools\Datas\#Sprite.xlsx
        [MenuItem("Tools/CollectAssets/CollectSprite")]
        public static void CollectSprite() {
            // string atlasDirectory = "Assets/ToBundle/Atlas";
            // string csvPath = "Assets/ToBundle/Config/Sprite.csv";
            // if (!System.IO.Directory.Exists(atlasDirectory)) {
            //     UnityEngine.Debug.LogError($"Directory not found: {atlasDirectory}");
            //     return;
            // }
            //
            // var atlasFiles = System.IO.Directory.GetFiles(atlasDirectory, "*.spriteatlas", System.IO.SearchOption.AllDirectories);
            // if (atlasFiles.Length == 0) {
            //     UnityEngine.Debug.LogWarning("No sprite atlas files found.");
            //     return;
            // }
            //
            // var constLineLength = 4;
            // var csvLines = System.IO.File.ReadAllLines(csvPath).ToList();
            // if (csvLines.Count < constLineLength) {
            //     UnityEngine.Debug.LogError($"CSV file {csvPath} does not have enough lines. Expected at least {constLineLength} lines.");
            //     return;
            // }
            //
            // // 清除之前的内容
            // csvLines.RemoveRange(constLineLength, csvLines.Count - constLineLength);
            //
            // HashSet<string> nameMap = new HashSet<string>();
            // for (int i = 0; i < atlasFiles.Length; i++) {
            //     var filePath = atlasFiles[i];
            //     // filePath 转 AssetDatabase 路径
            //     filePath = filePath.Replace(Application.dataPath, "Assets");
            //     var spriteAtlas = AssetDatabase.LoadAssetAtPath<UnityEngine.U2D.SpriteAtlas>(filePath);
            //     if (spriteAtlas == null) {
            //         UnityEngine.Debug.LogWarning($"SpriteAtlas not found at path: {filePath}");
            //         continue;
            //     }
            //
            //     var sprites = new Sprite[spriteAtlas.spriteCount];
            //     spriteAtlas.GetSprites(sprites);
            //     foreach (var sprite in sprites) {
            //         var name = sprite.name;
            //         name = name.Replace("(Clone)", "");
            //         if (nameMap.Contains(name)) {
            //             UnityEngine.Debug.LogError($"Duplicate sprite name found: {name}");
            //             continue;
            //         }
            //         var newLine = $"{name},{i}";
            //         csvLines.Add(newLine);
            //         nameMap.Add(name);
            //     }
            // }
            // System.IO.File.WriteAllLines(csvPath, csvLines);
            // UnityEngine.Debug.Log($"Sprite collect completed. Output: {csvPath}");
        }
    }
}