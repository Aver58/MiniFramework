using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;

// todo 写个工具，gen.bat 执行后，自动生成这个脚本
public static class StaticConfig {
    private static string ConfigPath = "Assets/ToBundle/Config/LubanConfigs/";

    public static JSONNode LoadByteBuf(string file) {
        string fileFullPath = ConfigPath + file + ".json";
        // 强行同步
        string csvContent = Addressables.LoadAssetAsync<TextAsset>(fileFullPath).WaitForCompletion().text;
        return JSON.Parse(csvContent);
    }

    private static cfg.TbTest tbTest; public static cfg.TbTest Test { get { tbTest ??= new cfg.TbTest(LoadByteBuf("tbtest")); return tbTest; } }
    private static cfg.TbUIViewDefine tbUIViewDefine; public static cfg.TbUIViewDefine UIViewDefine { get { tbUIViewDefine ??= new cfg.TbUIViewDefine(LoadByteBuf("tbuiviewdefine")); return tbUIViewDefine; } }

    private static cfg.TbSprite tbSprite; public static cfg.TbSprite Sprite { get { tbSprite ??= new cfg.TbSprite(LoadByteBuf("sprite")); return tbSprite; } }

    private static cfg.TbSpriteAtlas tbSpriteAtlas; public static cfg.TbSpriteAtlas SpriteAtlas { get { tbSpriteAtlas ??= new cfg.TbSpriteAtlas(LoadByteBuf("spriteatlas")); return tbSpriteAtlas; } }
}