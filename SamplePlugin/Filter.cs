using System;
using Dalamud.Game.Gui.PartyFinder.Types;
using Dalamud.Logging;

namespace SamplePlugin
{
    public class Filter : IDisposable
    {
        private Plugin Plugin { get; }

        public Filter(Plugin plugin)
        {
            this.Plugin = plugin;

            this.Plugin.PartyFinderGui.ReceiveListing += this.ReceiveListing;
        }

        public void Dispose()
        {
            this.Plugin.PartyFinderGui.ReceiveListing -= this.ReceiveListing;
        }

        private void ReceiveListing(PartyFinderListing listing, PartyFinderListingEventArgs args)
        {
            try
            {
                this.Plugin.description = listing.Description.TextValue;
            }
            catch (Exception ex)
            {
                PluginLog.LogError($"Error: {ex}");
            }
        }
    }
}
