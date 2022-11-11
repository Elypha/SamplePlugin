using System;
using Dalamud.Game.Gui.PartyFinder.Types;

namespace SamplePlugin
{
    public class Filter : IDisposable
    {
        private Plugin Plugin { get; }

        internal Filter(Plugin plugin)
        {
            this.Plugin = plugin;

            this.Plugin.PartyFinderGui.ReceiveListing += this.handle_listing;
        }

        public void Dispose()
        {
            this.Plugin.PartyFinderGui.ReceiveListing -= this.handle_listing;
        }

        private void handle_listing(PartyFinderListing listing, PartyFinderListingEventArgs args)
        {
            this.Plugin.description = listing.Description.TextValue;
        }
    }
}