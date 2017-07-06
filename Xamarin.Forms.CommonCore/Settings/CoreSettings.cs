﻿using System;
using System.Collections.Generic;
using System.Net;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Newtonsoft.Json;
using System.Reflection;

#if __ANDROID__
using Android.Widget;
#endif
#if __IOS__
using Foundation;
#endif

namespace Xamarin.Forms.CommonCore
{
    public class CoreSettings
    {
		private static ISettings _appSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

        public static ConfigurationModel Config
        {
            get { return AppData.Instance.Settings; }
        }

        /// <summary>
        /// Application Installation ID
        /// </summary>
        /// <value>The installation identifier.</value>
		public static string InstallationId
		{
			get
            {
                var id = _appSettings.GetValueOrDefault("InstallationId", null);

                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString();
                    _appSettings.AddOrUpdateValue("InstallationId", id);
                }

                return id;
            }
			set { _appSettings.AddOrUpdateValue("InstallationId", value); }
		}

        /// <summary>
        /// Gets or sets the current buid.
        /// </summary>
        /// <value>The current buid.</value>
        public static string CurrentBuid { get; set; } = "dev";

		public static AuthenticationToken TokenBearer { get; set; }
		public static NetworkCredential HttpCredentials { get; set; }

		public static bool IsConnected { get; set; } = true;
		public static INavigation AppNav { get; set; }
		public static Size ScreenSize { get; set; }

		public static List<string> NotificationTags { get; set; } = new List<string>();

		public static bool TokenIsValid
		{
			get
			{
				return TokenBearer?.expires > DateTimeOffset.Now;
			}
		}

#if __ANDROID__
        public  static int AppIcon { get; set; }
        public static int SearchView { get; set; }
        public static string DeviceToken { get; set; }
#endif

#if __IOS__
		public static NSData DeviceToken { get; set; }
#endif

		internal class AppData
		{
			public static AppData Instance = new AppData();
			public AppData()
			{
				Load();
			}
			public void Reload()
			{
				Load();
			}

			public ConfigurationModel Settings { get; private set; }


			private void Load()
			{
				string fileName = null;
				switch (CoreSettings.CurrentBuid)
				{
					case "qa":
						fileName = "config.qa.json";
						break;
					case "prod":
						fileName = "config.prod.json";
						break;
					default:
						fileName = "config.dev.json";
						break;
				}

				string json = ResourceLoader.GetEmbeddedResourceString(Assembly.GetAssembly(typeof(ResourceLoader)), fileName);
				var root = JsonConvert.DeserializeObject<ConfigurationModel>(json);
				if (root != null)
				{
					Settings = root;
				}

			}
		}

	}


}
