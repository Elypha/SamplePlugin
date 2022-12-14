using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.PartyFinder;
using SamplePlugin.Windows;
using Dalamud.Logging;


namespace SamplePlugin
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sample Plugin";
        private const string CommandName = "/cc";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");
        [PluginService]
        internal PartyFinderGui PartyFinderGui { get; init; } = null!;
        public string description = "empty line :c";
        private Filter Filter { get; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            WindowSystem.AddWindow(new ConfigWindow(this));
            WindowSystem.AddWindow(new MainWindow(this, goatImage));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            try
            {
                this.Filter = new Filter(this);
            }
            catch (Exception ex)
            {
                PluginLog.LogError($"Error: {ex}");
            }

        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
            this.Filter.Dispose();
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            WindowSystem.GetWindow("My Amazing Window").IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            WindowSystem.GetWindow("A Wonderful Configuration Window").IsOpen = true;
        }
    }
}
