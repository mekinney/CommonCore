﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xamarin.Forms.CommonCore
{
    public partial class CoreBusiness : IDisposable
    {
        #region ReadOnly AppData Settings
        [JsonIgnore]
        public string AESEncryptionKey { get { return CoreSettings.Config.AESEncryptionKey; } }
        [JsonIgnore]
        public Dictionary<string, string> WebApis { get { return CoreSettings.Config?.WebApi; } }
        [JsonIgnore]
        public Dictionary<string, string> CustomSettings { get { return CoreSettings.Config?.CustomSettings; } }
        #endregion

        #region Injection Services

        /// <summary>
        /// AuthenticatorService for Google, Facebook and Microsoft.
        /// </summary>
        /// <value>The authenticator service.</value>
        [JsonIgnore]
        protected IAuthenticatorService AuthenticatorService
        {
            get
            {
                return (IAuthenticatorService)CoreDependencyService.GetService<IAuthenticatorService, AuthenticatorService>(true);
            }
        }
        /// <summary>
        /// Backgrounding event timer that fires an event specified in the future on a repeating basis.
        /// </summary>
        /// <value>The background timer.</value>
        [JsonIgnore]
        protected IBackgroundTimer BackgroundTimer
        {
            get
            {
                return (IBackgroundTimer)CoreDependencyService.GetService<IBackgroundTimer, BackgroundTimer>(true);
            }
        }

        /// <summary>
        /// Service that provides network calls over http.
        /// </summary>
        /// <value>The http service.</value>
        [JsonIgnore]
        protected IHttpService HttpService
        {
            get
            {
                return (IHttpService)CoreDependencyService.GetService<IHttpService, HttpService>(true);
            }
        }

        /// <summary>
        /// Embedded file store that allow objects to be json serialized.
        /// </summary>
        /// <value>The file store.</value>
        [JsonIgnore]
        protected IFileStore FileStore
        {
            get
            {
                return (IFileStore)CoreDependencyService.GetService<IFileStore, FileStore>(true);
            }
        }

        /// <summary>
        /// Service that uses the OS account store to retrieve dictionary data
        /// </summary>
        /// <value>The account service.</value>
        [JsonIgnore]
        protected IAccountService AccountService
        {
            get
            {
                return (IAccountService)CoreDependencyService.GetService<IAccountService, AccountService>(true);
            }
        }

        /// <summary>
        /// AES encryption and Hash service.
        /// </summary>
        /// <value>The encryption service.</value>
        [JsonIgnore]
        protected IEncryptionService EncryptionService
        {
            get
            {
                return (IEncryptionService)CoreDependencyService.GetService<IEncryptionService, EncryptionService>(true);
            }
        }

		#endregion

		public virtual void Dispose()
		{

		}
    }
}
