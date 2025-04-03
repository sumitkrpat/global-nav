using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace GlobalNavService.Utils
{
    public class StyleGuideSection : ConfigurationSection
    {
        [ConfigurationProperty("versions")]
        public StyleGuideVersionCollection Versions
        {
            get { return (StyleGuideVersionCollection)base["versions"]; }
        }

        [ConfigurationProperty("defaultVersion", IsRequired = true)]
        public string DefaultVersion
        {
            get { return (string)base["defaultVersion"]; }
            set { base["defaultVersion"] = value; }
        }
    }

    [ConfigurationCollection(typeof(StyleGuideElement))]
    public class StyleGuideVersionCollection : ConfigurationElementCollection, IEnumerable<StyleGuideElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StyleGuideElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StyleGuideElement)element).Version;
        }

        public StyleGuideElement this[int index]
        {
            get
            {
                return BaseGet(index) as StyleGuideElement;
            }
        }
        
        IEnumerator<StyleGuideElement> IEnumerable<StyleGuideElement>.GetEnumerator()
        {
            return (from i in Enumerable.Range(0, this.Count)
                    select this[i])
                    .GetEnumerator();
        }
    }

    public class StyleGuideElement : ConfigurationElement
    {
        [ConfigurationProperty("version", IsRequired = true)]
        public string Version
        {
            get { return (string)base["version"]; }
            set { base["version"] = value; }
        }
        
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
            set { base["url"] = value; }
        }
    }
}