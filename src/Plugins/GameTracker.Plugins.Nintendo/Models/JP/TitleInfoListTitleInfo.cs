namespace GameTracker.Plugins.Nintendo.Models.JP
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TitleInfoListTitleInfo
    {

        private string initialCodeField;

        private string titleNameField;

        private string makerNameField;

        private object makerKanaField;

        private string priceField;

        private string salesDateField;

        private string softTypeField;

        private ushort platformIDField;

        private byte dlIconFlgField;

        private string linkURLField;

        private byte screenshotImgFlgField;

        private string screenshotImgURLField;

        /// <remarks/>
        public string InitialCode
        {
            get
            {
                return this.initialCodeField;
            }
            set
            {
                this.initialCodeField = value;
            }
        }

        /// <remarks/>
        public string TitleName
        {
            get
            {
                return this.titleNameField;
            }
            set
            {
                this.titleNameField = value;
            }
        }

        /// <remarks/>
        public string MakerName
        {
            get
            {
                return this.makerNameField;
            }
            set
            {
                this.makerNameField = value;
            }
        }

        /// <remarks/>
        public object MakerKana
        {
            get
            {
                return this.makerKanaField;
            }
            set
            {
                this.makerKanaField = value;
            }
        }

        /// <remarks/>
        public string Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public string SalesDate
        {
            get
            {
                return this.salesDateField;
            }
            set
            {
                this.salesDateField = value;
            }
        }

        /// <remarks/>
        public string SoftType
        {
            get
            {
                return this.softTypeField;
            }
            set
            {
                this.softTypeField = value;
            }
        }

        /// <remarks/>
        public ushort PlatformID
        {
            get
            {
                return this.platformIDField;
            }
            set
            {
                this.platformIDField = value;
            }
        }

        /// <remarks/>
        public byte DlIconFlg
        {
            get
            {
                return this.dlIconFlgField;
            }
            set
            {
                this.dlIconFlgField = value;
            }
        }

        /// <remarks/>
        public string LinkURL
        {
            get
            {
                return this.linkURLField;
            }
            set
            {
                this.linkURLField = value;
            }
        }

        /// <remarks/>
        public byte ScreenshotImgFlg
        {
            get
            {
                return this.screenshotImgFlgField;
            }
            set
            {
                this.screenshotImgFlgField = value;
            }
        }

        /// <remarks/>
        public string ScreenshotImgURL
        {
            get
            {
                return this.screenshotImgURLField;
            }
            set
            {
                this.screenshotImgURLField = value;
            }
        }
    }
}