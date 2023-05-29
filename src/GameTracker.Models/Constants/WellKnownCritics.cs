namespace GameTracker.Models.Constants
{
    public  class WellKnownCritics
    {
        public static Critic Metacritic => new Critic
        {
            Name = "Metacritic",
            Description = "Metacritic is a website that aggregates reviews of films, television shows, music albums, video games, and formerly books. For each product, the scores from each review are averaged.",
            Links = new[]
            {
                new SocialLink
                {
                    LinkPlatform = Enums.LinkType.Web,
                    LinkTarget = "https://www.metacritic.com/"
                },
                new SocialLink
                {
                    LinkPlatform = Enums.LinkType.Twitter,
                    LinkTarget = "@metacritic"
                }
            }
        };
    }
}