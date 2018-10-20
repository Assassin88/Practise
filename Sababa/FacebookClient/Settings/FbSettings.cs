namespace FacebookClient.Settings
{
    public class FbSettings
    {
        /// <summary>
        /// Return id current user.
        /// </summary>
        public static string UserId { get; } = "936346953231113";

        /// <summary>
        /// Return base uri facebook graph api.
        /// </summary>
        public static string BaseUri { get; } = "https://graph.facebook.com/v3.1/";

        /// <summary>
        /// Return permissions for facebook query.
        /// </summary>
        public static string[] Permissions { get; } = { "public_profile", "email", "user_friends", "user_likes" };

        /// <summary>
        /// Return endpoint for facebook query.
        /// </summary>
        public static string EndPoint { get; } = "me/friends";

        /// <summary>
        /// Return arguments for facebook query.
        /// </summary>
        public static string Args { get; } = "&fields=id,name,picture{url},music{id,name}";
    }
}