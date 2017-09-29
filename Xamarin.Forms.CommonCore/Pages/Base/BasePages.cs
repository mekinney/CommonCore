﻿using System;
using System.Reflection;

namespace Xamarin.Forms.CommonCore
{
    public class BasePages : ContentPage
    {
        private long appearingUTC;

        public bool AnalyticsEnabled
		{
            get { return CoreSettings.AppData.Settings.AnalyticsEnabled; }
        }
        public Size ScreenSize
        {
            get { return CoreSettings.ScreenSize; }
        }

		public ILogService Log
		{
			get
			{
                
				return (ILogService)InjectionManager.GetService<ILogService, LogService>(true);
			}
		}

        protected override bool OnBackButtonPressed()
        {
            var bindingContext = BindingContext as ObservableViewModel;
            var result = bindingContext?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
            return result;
        }

        public void OnSoftBackButtonPressed()
        {
            var bindingContext = BindingContext as ObservableViewModel;
            bindingContext?.OnSoftBackButtonPressed();
        }

        public static readonly BindableProperty NeedOverrideSoftBackButtonProperty =
                BindableProperty.Create("NeedOverrideSoftBackButton", typeof(bool), typeof(BasePages), false);

        /// <summary>
        /// Enables the ability of the Pages view model to receive soft back button press events
        /// </summary>
        /// <value><c>true</c> if need override soft back button; otherwise, <c>false</c>.</value>
        public bool NeedOverrideSoftBackButton
        {
            get { return (bool)GetValue(NeedOverrideSoftBackButtonProperty); }
            set { SetValue(NeedOverrideSoftBackButtonProperty, value); }
        }

        public void NavigateTo<T>() where T : ContentPage, new()
        {
            CoreSettings.AppNav.PushAsync(new T()).ConfigureAwait(false);
        }

        public void NavigateTo(ContentPage page)
        {
            CoreSettings.AppNav.PushAsync(page).ConfigureAwait(false);
        }

        public void NavigateBack(bool animate = true)
        {
            CoreSettings.AppNav.PopAsync(animate).ConfigureAwait(false);
        }

#if __IOS__

		/// <summary>
		/// Override default settings for back button and removes the chevron images leaving just text.
		/// </summary>
		public static readonly BindableProperty OverrideBackButtonProperty =
            BindableProperty.Create("OverrideBackButton", typeof(bool), typeof(BasePages), false);

        /// <summary>
        /// Override default settings for back button and removes the chevron images leaving just text.
        /// </summary>
        /// <value><c>true</c> if override back button; otherwise, <c>false</c>.</value>
        public bool OverrideBackButton
        {
            get { return (bool)GetValue(OverrideBackButtonProperty); }
            set { SetValue(OverrideBackButtonProperty, value); }
        }

        /// <summary>
        /// The override back text property.
        /// </summary>
		public static readonly BindableProperty OverrideBackTextProperty =
		    BindableProperty.Create("OverrideBackText", typeof(string), typeof(BasePages), "Back");

        /// <summary>
        /// Gets or sets the override back text.
        /// </summary>
        /// <value>The override back text.</value>
		public string OverrideBackText
		{
			get { return (string)GetValue(OverrideBackTextProperty); }
			set { SetValue(OverrideBackTextProperty, value); }
		}

#endif

		protected override void OnAppearing()
		{
			appearingUTC = DateTime.UtcNow.Ticks;

			if (Navigation != null)
				CoreSettings.AppNav = Navigation;
			base.OnAppearing();
		}

		protected override void OnDisappearing()
		{
			if (AnalyticsEnabled)
			{
				Log.LogAnalytics(this.GetType().FullName, new TrackingMetatData()
				{
					StartUtc = appearingUTC,
					EndUtc = DateTime.UtcNow.Ticks
				});
			}
			base.OnDisappearing();
		}

    }

}
