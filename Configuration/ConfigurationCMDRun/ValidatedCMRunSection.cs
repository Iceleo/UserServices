//https://www.codeproject.com/Articles/16724/Decoding-the-Mysteries-of-NET-2-0-Configuration

using System;
using System.Configuration;

namespace UserServices.ConfigurationCMDRun
{

    public class ValidatedCMRunSection : ConfigurationSection
    {
        #region Constructor
        static ValidatedCMRunSection()
        {
            _propMyTimeSpan = new ConfigurationProperty(
                "myTimeSpan",
                typeof(TimeSpan),
                TimeSpan.Zero,
                null,
                new TimeSpanValidator(TimeSpan.Zero, TimeSpan.FromHours(24)),
                ConfigurationPropertyOptions.IsRequired
            );

            _propMaxCMD = new ConfigurationProperty("maxCMD", typeof(int), 0, null,
               new IntegerValidator(0, 20), ConfigurationPropertyOptions.IsRequired);

            _propMaxProp = new ConfigurationProperty("maxProp", typeof(long), default, null,
                new LongValidator(0, 1000), ConfigurationPropertyOptions.IsRequired );

            _properties = new ConfigurationPropertyCollection();

            _properties.Add(_propMyTimeSpan);
            _properties.Add(_propMaxCMD);
            _properties.Add(_propMaxProp);
        }

        public ValidatedCMRunSection()
        {
        }
        #endregion

            #region Fields
        private static ConfigurationPropertyCollection _properties;
        private static ConfigurationProperty _propMyTimeSpan;
        private static ConfigurationProperty _propMaxCMD;
        private static ConfigurationProperty _propMaxProp;
        #endregion

        #region Properties
        [ConfigurationProperty("myTimeSpan", DefaultValue = default,
                               IsRequired = true)]
        [TimeSpanValidator(MinValueString = "0:0:0", MaxValueString = "24:0:0")]
        public TimeSpan MyTimeSpan
        {
            get { return (TimeSpan)base[_propMyTimeSpan]; }
        }

        [ConfigurationProperty("maxCMD", DefaultValue = 0, IsRequired = true)]
        [IntegerValidator(MinValue = 0, MaxValue = 20, ExcludeRange = true)]
        public int MaxCMD
        {
            get { return (int)base[_propMaxCMD]; }
        }

        [ConfigurationProperty("maxProp", DefaultValue = 1, IsRequired = true)]
        [LongValidator(MinValue = 0, MaxValue = 1000, ExcludeRange = true)]
//        [LongValidator(MinValue = Int64.MinValue, MaxValue = Int64.MaxValue, ExcludeRange = true)]
        public long MaxProp
        {
            get { return (long)base[_propMaxProp]; }
        }
        #endregion

        /*[ConfigurationProperty("default")]
        public string Default
        {
            get { return (string)base["default"]; }
            set { base["default"] = value; }
        }*/

        // This is a key customization. 
        // It returns the initialized property bag.
        [ConfigurationProperty("cmdrun", IsDefaultCollection = true)]
     //   [ConfigurationCollection(typeof(CommandLineRunConfCollection))]

        protected override ConfigurationPropertyCollection Properties => _properties;

    }
}