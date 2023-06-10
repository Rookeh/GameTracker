namespace GameTracker.Plugins.Nintendo.Models.JP
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class TitleInfoList
    {

        private TitleInfoListTitleInfo[] titleInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TitleInfo")]
        public TitleInfoListTitleInfo[] TitleInfo
        {
            get
            {
                return this.titleInfoField;
            }
            set
            {
                this.titleInfoField = value;
            }
        }
    }
}