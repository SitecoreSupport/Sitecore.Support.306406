namespace Sitecore.Support.XA.Foundation.Multisite.Pipelines.ResolveTokens
{
  using Microsoft.Extensions.DependencyInjection;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Data.Managers;
  using Sitecore.DependencyInjection;
  using Sitecore.StringExtensions;
  using Sitecore.XA.Foundation.Multisite;
  using Sitecore.XA.Foundation.TokenResolution.Pipelines.ResolveTokens;
  using System.Collections.Generic;
  using System.Linq;

  public class ResolveMultisiteTokens : Sitecore.XA.Foundation.Multisite.Pipelines.ResolveTokens.ResolveMultisiteTokens
  {
    public ResolveMultisiteTokens() : base() { }

    public ResolveMultisiteTokens(IMultisiteContext multisiteContext, ISharedSitesContext shredSIteContext) : base(multisiteContext, shredSIteContext) { }
    public override void Process(ResolveTokensArgs args)
    {
      string query = args.Query;
      Item contextItem = args.ContextItem;
      bool escapeSpaces = args.EscapeSpaces;
      query = ReplaceTokenWithItemPath(query, "$tenant", () => MultisiteContext.GetTenantItem(contextItem), escapeSpaces);
      query = ReplaceTokenWithItemPath(query, "$siteMedia", () => MultisiteContext.GetSiteMediaItem(contextItem), escapeSpaces);
      query = ReplaceTokenWithItemPath(query, "$site", () => MultisiteContext.GetSiteItem(contextItem), escapeSpaces);
      query = ExpandTokenWithDynamicItemPaths(query, "$sharedSites", () => SharedSitesContext.GetSharedSitesWithoutCurrent(contextItem), escapeSpaces);
      query = ReplaceTokenWithItemPath(query, "$home", () => ServiceLocator.ServiceProvider.GetService<ISiteInfoResolver>().GetStartPath(contextItem), escapeSpaces);
      query = (args.Query = ReplaceTokenWithValue(query, "$templates", () => GetTenantTemplatesQuery(contextItem)));
    }
  }
}