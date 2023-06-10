using System.Text.Json.Serialization;

namespace GameTracker.Plugins.Nintendo.Models.EU
{
    public class EUDocument
    {
        [JsonPropertyName("fs_id")]
        public string FsId { get; set; }

        [JsonPropertyName("change_date")]
        public DateTime ChangeDate { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("dates_released_dts")]
        public DateTime[] DatesReleased { get; set; }

        [JsonPropertyName("club_nintendo")]
        public bool ClubNintendo { get; set; }

        [JsonPropertyName("pretty_date_s")]
        public string PrettyDate { get; set; }

        [JsonPropertyName("play_mode_tv_mode_b")]
        public bool PlayModeTVMode { get; set; }

        [JsonPropertyName("play_mode_handheld_mode_b")]
        public bool PlayModeHandheldMode { get; set; }

        [JsonPropertyName("product_code_txt")]
        public string[] ProductCode { get; set; }

        [JsonPropertyName("image_url_sq_s")]
        public string ImageUrlSq { get; set; }

        [JsonPropertyName("deprioritise_b")]
        public bool Deprioritise { get; set; }

        [JsonPropertyName("demo_availability")]
        public bool DemoAvailability { get; set; }

        [JsonPropertyName("pg_s")]
        public string Pg { get; set; }

        [JsonPropertyName("compatible_controller")]
        public string[] CompatibleController { get; set; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("originally_for_t")]
        public string OriginallyFor { get; set; }

        [JsonPropertyName("cloud_saves_b")]
        public bool CloudSaves { get; set; }

        [JsonPropertyName("priority")]
        public DateTime Priority { get; set; }

        [JsonPropertyName("digital_version_b")]
        public bool DigitalVersion { get; set; }

        [JsonPropertyName("title_extras_txt")]
        public string[] TitleExtras { get; set; }

        [JsonPropertyName("image_url_h2x1_s")]
        public string ImageUrlH2X1 { get; set; }

        [JsonPropertyName("system_type")]
        public string[] SystemType { get; set; }

        [JsonPropertyName("age_rating_sorting_i")]
        public int AgeRatingSorting { get; set; }

        [JsonPropertyName("game_categories_txt")]
        public string[] GameCategories { get; set; }

        [JsonPropertyName("play_mode_tabletop_mode_b")]
        public bool PlayModeTableTopMode { get; set; }

        [JsonPropertyName("publisher")]
        public string Publisher { get; set; }

        [JsonPropertyName("product_code_ss")]
        public string[] ProductCodes { get; set; }

        [JsonPropertyName("excerpt")]
        public string Excerpt { get; set; }

        [JsonPropertyName("nsuid_txt")]
        public string[] NSUID { get; set; }

        [JsonPropertyName("date_from")]
        public DateTime DateFrom { get; set; }

        [JsonPropertyName("language_availability")]
        public string[] LanguageAvailability { get; set; }

        [JsonPropertyName("price_has_discount_b")]
        public bool PriceHasDiscount { get; set; }

        [JsonPropertyName("product_catalog_description_s")]
        public string ProductCatalogDescription { get; set; }

        [JsonPropertyName("price_discount_percentage_f")]
        public float PriceDiscountPercentage { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("sorting_title")]
        public string SortingTitle { get; set; }

        [JsonPropertyName("copyright_s")]
        public string Copyright { get; set; }

        [JsonPropertyName("wishlist_email_square_image_url_s")]
        public string WishlistEmailSquareImageUrl { get; set; }

        [JsonPropertyName("players_to")]
        public int PlayersTo { get; set; }

        [JsonPropertyName("wishlist_email_banner640w_image_url_s")]
        public string WishlistEmailBanner640WImageUrl { get; set; }

        [JsonPropertyName("playable_on_txt")]
        public string[] PlayableOn { get; set; }

        [JsonPropertyName("hits_i")]
        public int Hits { get; set; }

        [JsonPropertyName("pretty_game_categories_txt")]
        public string[] PrettyGameCategories { get; set; }

        [JsonPropertyName("switch_game_voucher_b")]
        public bool SwitchGameVoucher { get; set; }

        [JsonPropertyName("GameCategory")]
        public string[] GameCategory { get; set; }

        [JsonPropertyName("system_names_txt")]
        public string[] SystemNames { get; set; }

        [JsonPropertyName("pretty_agerating_s")]
        public string PrettyAgeRating { get; set; }

        [JsonPropertyName("price_regular_f")]
        public float PriceRegular { get; set; }

        [JsonPropertyName("eshop_removed_b")]
        public bool EshopRemoved { get; set; }

        [JsonPropertyName("age_rating_type")]
        public string AgeRatingType { get; set; }

        [JsonPropertyName("price_sorting_f")]
        public float PriceSorting { get; set; }

        [JsonPropertyName("price_lowest_f")]
        public float PriceLowest { get; set; }

        [JsonPropertyName("age_rating_value")]
        public string AgeRating { get; set; }

        [JsonPropertyName("physical_version_b")]
        public bool PhysicalVersion { get; set; }

        [JsonPropertyName("wishlist_email_banner460w_image_url_s")]
        public string WishlistEmailBanner460WImageUrl { get; set; }

        [JsonPropertyName("_version_")]
        public long Version { get; set; }

        [JsonPropertyName("paid_subscription_required_b")]
        public bool PaidSubscriptionRequired { get; set; }

        [JsonPropertyName("voice_chat_b")]
        public bool VoiceChat { get; set; }

        [JsonPropertyName("players_from")]
        public int PlayersFrom { get; set; }
    }

}
