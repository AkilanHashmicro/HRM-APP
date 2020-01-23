using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMAPP.Model
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        private const string UserNameKey = "username_key";
        private static readonly string UserNameDefault = string.Empty;

        public static string UserName
        {
            get { return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserNameKey, value); }
        }


        private const string UserPasswordKey = "userpassword_key";
        private static readonly string UserPasswordDefault = string.Empty;

        public static string UserPassword
        {
            get { return AppSettings.GetValueOrDefault<string>(UserPasswordKey, UserPasswordDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserPasswordKey, value); }
        }


        // storing checkIn and checkOut 
        private const string UserCheckIN_OutKey = "checkinout";
        private static readonly string Check_In_OutDefault = string.Empty;

        public static string CheckIn_Out
        {
            get { return AppSettings.GetValueOrDefault<string>(UserCheckIN_OutKey, Check_In_OutDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserCheckIN_OutKey, value); }
        }

        // storing check In ID
        private const string UserCheckIN_ID = "checkIn_Id";
        private static readonly string Check_ID_Default = string.Empty;

        public static string CheckIn_ID
        {
            get { return AppSettings.GetValueOrDefault<string>(UserCheckIN_ID, Check_ID_Default); }
            set { AppSettings.AddOrUpdateValue<string>(UserCheckIN_ID, value); }
        }


        private const string UrlKey = "url_key";
        private static readonly string UrlDefault = string.Empty;

        public static string UserUrlName
        {
            get { return AppSettings.GetValueOrDefault<string>(UrlKey, UrlDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UrlKey, value); }
        }

        private const string UserDbKey = "db_key";
        private static readonly string DbDefault = string.Empty;

        public static string UserDbName
        {
            get { return AppSettings.GetValueOrDefault<string>(UserDbKey, DbDefault); }
            set { AppSettings.AddOrUpdateValue<string>(UserDbKey, value); }
        }

        private const string ColourCodes = "stage_colour_code";
        private static readonly string UserColourCodeDefault = string.Empty;

        public static string StageColourCode
        {
            get { return AppSettings.GetValueOrDefault<string>(ColourCodes, UserColourCodeDefault); }
            set { AppSettings.AddOrUpdateValue<string>(ColourCodes, value); }

        }

        private const string PrefKeyIsLockedkey = "False";
        private static readonly string PrefKeyIsLockedDefault = "False";

        public static string PrefKeyIsLocked
        {
            get { return AppSettings.GetValueOrDefault<string>(PrefKeyIsLockedkey, PrefKeyIsLockedDefault); }
            set { AppSettings.AddOrUpdateValue<string>(PrefKeyIsLockedkey, value); }
        }

        private const string PrefKeyUserDetailsKey = "UserDetails";
        private static readonly string PrefKeyUserDetailsDefault = "UserDefaultDetails";

        public static string PrefKeyUserDetails
        {
            get { return AppSettings.GetValueOrDefault<string>(PrefKeyUserDetailsKey, PrefKeyUserDetailsDefault); }
            set { AppSettings.AddOrUpdateValue<string>(PrefKeyUserDetailsKey, value); }
        }

    }
}
