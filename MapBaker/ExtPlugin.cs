using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Facepunch.Utility;
using MapBaker.Bitmapping;
using MapBaker.Blending;
using MapBaker.Configuration;
using MapBaker.Layering;
using MapBaker.Masking;
using MapBaker.Utility;
using MapBaker.Utility.Converting;
using MapBaker.Utility.Providers;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Game.Rust.Libraries;
using Oxide.Plugins;
using UnityEngine;

public class ExtPlugin : CSPlugin
{
    private const string COMMAND_DRAW = "mb.draw";

    private PluginConfiguration _config;
    
    private string PathToResult { get; set; }

    public ExtPlugin()
    {
        Name = "MapBaker";
        Title = "MapBaker";

        Author = "mebikrouse";

        var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
        Version = new VersionNumber(assemblyVersion.Major, assemblyVersion.Minor, assemblyVersion.Revision);

        HasConfig = true;
    }

    [HookMethod("OnServerInitialized")]
    private void OnServerInitialized()
    {
        _config = Config.ReadObject<PluginConfiguration>();
        PathToResult = Path.Combine(_config.ResultPath.FullPath,
            $"Map-{ConVar.Server.seed}-{ConVar.Server.worldsize}.png");

        var commands = GetLibrary<Command>();
        commands.AddConsoleCommand(COMMAND_DRAW, this, DrawMapConsoleCommand);

        if (File.Exists(PathToResult))
        {
            Interface.CallHook("OnGeneratorMapCreate");
            return;
        }

        var timer = new PluginTimers(this);
        timer.Once(_config.DrawDelay, () =>
        {
            DrawMap();
            Interface.CallHook("OnGeneratorMapCreate");
        });
    }

    private bool DrawMapConsoleCommand(ConsoleSystem.Arg args)
    {
        if (args.Connection != null) return false;

        DrawMap();

        return true;
    }

    private void DrawMap()
    {
        Puts("Drawing map...");

        var width = _config.RenderResolution.Width;
        var height = _config.RenderResolution.Height;

        var coordinateConverter = new CoordinateConverter(width, height);
        var raycastCoordinateConverter = new RaycastCoordinateConverter(coordinateConverter);

        var layers = new List<ILayer>();

        Puts("Initializing background layer...");
        var backgroundTextureProvider =
            new SeamlessProvider(new BitmapProvider(_config.BackgroundTexturePath.FullPath), width, height);
        var backgroundLayer = new BitmapLayer(backgroundTextureProvider, NormalColorBlending.Instance);
        layers.Add(backgroundLayer);

        Puts("Initializing splat layers...");
        foreach (var splatConfiguration in _config.SplatConfigurations)
        {
            Puts($"Splat #{splatConfiguration.Index}...");
            var splatTextureProvider =
                new SeamlessProvider(new BitmapProvider(splatConfiguration.TexturePath.FullPath), width, height);
            var splatMask = new SplatMask(coordinateConverter, splatConfiguration.Index);
            var splatLayer = new MaskedBitmapLayer(splatTextureProvider, splatMask, NormalColorBlending.Instance);
            layers.Add(splatLayer);
        }

        Puts("Initializing materials source...");
        var materialSource =
            new DetailsLayer.MaterialSource(_config.DetailsConfiguration.MaterialConfigurations, width, height);

        Puts("Initializing details layer...");
        var detailsLayer = new DetailsLayer(raycastCoordinateConverter, materialSource,
            _config.DetailsConfiguration.RaycastLayers);
        layers.Add(detailsLayer);

        Puts("Initializing light layer...");
        var lightningLayer = new LightLayer(raycastCoordinateConverter, _config.LightConfiguration,
            _config.LightConfiguration.RaycastLayers);
        layers.Add(lightningLayer);

        Puts("Initializing water layer...");
        var waterTextureProvider =
            new SeamlessProvider(new BitmapProvider(_config.WaterConfiguration.TexturePath.FullPath), height, width);
        var waterMask = new WaterMask(raycastCoordinateConverter, _config.WaterConfiguration.MinOpacity,
            _config.WaterConfiguration.MaxDepth, _config.WaterConfiguration.RaycastLayers);
        var waterLayer = new MaskedBitmapLayer(waterTextureProvider, waterMask, NormalColorBlending.Instance);
        layers.Add(waterLayer);

        Puts("Initializing layer group...");
        var layerGroup = new LayerGroup(layers);

        var result = new Texture2D(width, height);
        var wrapper = new TextureWrapper(result);

        Puts("Drawing...");
        layerGroup.Draw(wrapper);

        Puts("Resizing to result resolution...");
        TextureScale.Scale(result, _config.ResultResolution.Width, _config.ResultResolution.Height);

        Puts("Saving result...");
        Directory.CreateDirectory(_config.ResultPath.FullPath);
        result.SaveAsPng(PathToResult);

        wrapper.Dispose();

        Puts("Done.");
    }

    public void Puts(string format, params object[] args)
    {
        Interface.Oxide.LogInfo("[{0}][{1}] {2}", DateTime.Now, this.Title,
            args.Length != 0 ? string.Format(format, args) : format);
    }

    protected override void LoadDefaultConfig()
    {
        Config.WriteObject(GetDefaultConfig(), true);
    }

    private static PluginConfiguration GetDefaultConfig()
    {
        return new PluginConfiguration();
    }
}