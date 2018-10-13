using System;
using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;
using Tbs.Configuration;

namespace Tbs.Configuration
{
    public class DepotSettingDefinition
    {
        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display name of the setting.
        /// This can be used to show setting to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// group displayName for this setting.
        /// </summary>
        public string GroupDisplayName { get; set; }

        /// <summary>
        /// Is this setting is Onoff
        /// </summary>
        public bool IsOnOffType { get; set; }

        /// <summary>
        /// Default value of the setting.
        /// </summary>
        public string DefaultValue { get; set; }

        public DepotSettingDefinition(
            string name, 
            string defaultValue, 
            string displayName = null, 
            string groupDisplayName = null, 
            bool isOnOffType = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            DefaultValue = defaultValue;
            DisplayName = displayName;
            GroupDisplayName = groupDisplayName;
            IsOnOffType = isOnOffType;
        }
    }
    
    public static class DepotSettingManager
    {
    }
}
