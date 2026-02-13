using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Scripts.Framework.UI {
    public static class ImageExtension {
        public static string SpriteAtlasDirectory = "Assets/ToBundle/Atlas";
        private static readonly Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
        private static readonly Dictionary<string, SpriteAtlas> atlasMap = new Dictionary<string, SpriteAtlas>();

        public static void SetSprite(this Image image, string spriteName, bool setNativeSize = false) {
            if (image == null) {
                throw new NullReferenceException();
            }

            if (string.IsNullOrEmpty(spriteName)) {
                Debug.LogError($"[SetSprite] 传入的 spriteName 为空！");
                return;
            }

            var config = StaticConfig.Sprite.Get(spriteName);
            if (config == null) {
                return;
            }

            if (sprites.TryGetValue(spriteName, out var sprite)) {
                image.sprite = sprite;
            }

            var atlasName = GetSpriteName(config.AtlasId);
            if (atlasMap.TryGetValue(atlasName, out var atlas) && atlas != null) {
                sprites[spriteName] = atlas.GetSprite(spriteName);
                image.sprite = sprites[spriteName];
            }

            var atlasPath = GetSpriteAtlasAssetPath(atlasName);
            ResourceManager.Instance.LoadAssetAsync<SpriteAtlas>(atlasPath, (asset) => {
                if (asset == null) {
                    Debug.LogError($"Failed to load sprite atlas: {atlasPath}");
                    return;
                }
                atlasMap[atlasName] = asset;
                sprites[spriteName] = asset.GetSprite(spriteName);
                image.sprite = sprites[spriteName];
                if (setNativeSize) {
                    image.SetNativeSize();
                }
            });
        }

        private static string GetSpriteName(int atlasId) {
            var atlasConfig = StaticConfig.SpriteAtlas.Get(atlasId);
            return atlasConfig?.Name;
        }

        private static string GetSpriteAtlasAssetPath(string assetName) {
            return $"{SpriteAtlasDirectory}/{assetName}.spriteatlas";
        }
    }
}