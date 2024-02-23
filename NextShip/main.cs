using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Microsoft.Extensions.DependencyInjection;
using NextShip.Api.Attributes;
using NextShip.Api.Extension;
using NextShip.Api.Interfaces;
using NextShip.Api.RPCs;
using NextShip.Api.Services;
using NextShip.Cosmetics;
using NextShip.DIY.Plugins;
using NextShip.Languages;
using NextShip.Manager;
using NextShip.Services;
using BepInExLogger = BepInEx.Logging.Logger;

[assembly: AssemblyFileVersion(Main.VersionString)]
[assembly: AssemblyInformationalVersion(Main.VersionString)]

namespace NextShip;

[BepInPlugin(Id, ModName, VersionString)]
[BepInProcess("Among Us.exe")]
public sealed class NextShip : BasePlugin
{
    // 模组名称
    public const string ModName = "NextShip";

    // 模组id
    public const string Id = "cn.MengChu.NextShip";

    // 模组版本
    public const string VersionString = "1.0.0";

    public const string BepInExVersion = "682";

    // Among Us游玩版本
    public static readonly AmongUsVersion SupportVersion = new(2023, 11, 28);

    public static readonly ShipVersion Version = new ShipVersion().Parse(VersionString);

    internal static readonly ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;

    public static List<INextAdd> Adds;

    private ManualLogSource TISLog;

    public static NextService _Service { get; private set; }

    public static Main Instance { get; private set; }

    public static Assembly RootAssembly { get; private set; }

    private Harmony Harmony { get; } = new(Id);

    private static event Action<ServiceBuilder> OnBuilder;

    private NextManager _nextManager;


    public override void Load()
    {
        Instance = this;
        RootAssembly = Assembly.GetExecutingAssembly();
        TISLog = BepInExLogger.CreateLogSource(ModName.RemoveBlank());
        Harmony.PatchAll();
        _nextManager = AddComponent<NextManager>().DontDestroyOnLoad();

        Api.Logs.Log.Get(TISLog);
        Init();

        CreateService();
        LoadDependent();
        Task.Run(() =>
        {
            var roleManager = _Service.Get<NextRoleManager>();
            FastAddRoleExt.Registration(roleManager, RootAssembly);
        });

        SteamExtension.UseSteamIdFile();
        ReactorExtension.UseReactorHandshake();
        ModStampExtension.UseModStamp();
        FastRPCExtension.UseFastRPC();
        FastRpcReaderPatch.AddFormAssembly(RootAssembly);

        ConsoleManager.SetConsoleTitle("Among Us " + ModName + " Game");
        RegisterManager.Registration();
        
        LanguagePack.Init();
        CustomCosmeticsManager.LoadHat();
    }

    private static void Init()
    {
        Info("IsDev:{}", "Const");
        Info($"CountryName:| {RegionInfo.CurrentRegion.DisplayName}", "Const");
        Info("isCn:", "Const");
        Info("IsChinese:", "Const");
        Info($"Support Among Us Version {SupportVersion}", "Info");
        Info("Hash: ", "Info");
        Info($"欢迎游玩{ModName} | Welcome to{ModName}", "Info");

        Adds = INextAdd.GetAdds(RootAssembly);
        foreach (var varType in Adds)
            varType.ConfigBind(Instance.Config);
    }

    private static void InitPlugins()
    {
        var manager = PluginManager.Get();
        manager.InitPlugins();
        OnBuilder += manager.OnServiceBuild;
    }

    private void CreateService()
    {
        var builder = new ServiceBuilder();
        builder.CreateService();
        builder._collection.AddLogging();
        builder._collection.AddSingleton<IRoleManager, NextRoleManager>();
        builder._collection.AddSingleton<IEventManager, EventManager>();
        builder._collection.AddSingleton<IPatchManager, NextPatchManager>();
        builder._collection.AddSingleton<IPlayerManager, NextPlayerManager>();
        builder._collection.AddSingleton<INextOptionManager, NextOptionManager>();
        builder._collection.AddSingleton<INextButtonManager, NextButtonsManager>();
        builder._collection.AddSingleton<INextScreenManager, NextScreenManager>();
        builder._collection.AddSingleton<IBotManager, NextBotManager>();
        builder._collection.AddSingleton<ILangManager, NextLangManager>();
        builder._collection.AddSingleton<IKeyBindManager, NextKeyBindManager>();
        builder._collection.AddSingleton(_nextManager);
        builder._collection.AddHttpClient();
        builder.AddTransient<GithubAnalyzer>();
        builder.Add<DownloadService>();
        builder.Add<MetadataService>();
        builder.Add<DataService>();
        builder.Add<DependentService>();
        builder.Add<InstanceManager>();

        OnBuilder?.Invoke(builder);

        foreach (var varType in Adds)
            varType.ServiceAdd(builder);

        _Service = NextService.Build(builder);
        ServiceAddAttribute.Registration(_Service._Provider, RootAssembly);
    }

    private static void LoadDependent()
    {
        var service = _Service.Get<DependentService>();
        service.SetPath(new DirectoryInfo(NextPaths.TIS_Lib));

        foreach (var varType in Adds)
            varType.DependentAdd(service);

        service.BuildDependent();
        service.LoadDependent();
    }
}