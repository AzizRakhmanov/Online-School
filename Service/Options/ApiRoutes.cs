namespace Service.Options
{
    public static class ApiRoutes
    {
        private const string Root = "api";

        private const string Version = "v1";

        private const string Base = Root + "/" + Version;

        public static class Users
        {
            public const string GetAll = Base + "/" + "users";

            public const string Get = Base + "/" + "users/{id}";

            public const string Create = Base + "/" + "users";

            public const string Delete = Base + "/" + "users";

            public const string Update = Base + "/" + "users";
        }

        public static class Courses
        {
            public const string GetAll = Base + "/" + "courses";

            public const string Get = Base + "/" + "courses/{id}";

            public const string Create = Base + "/" + "courses";

            public const string Delete = Base + "/" + "courses";

            public const string Update = Base + "/" + "courses";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/register";

            public const string Refresh = Base + "/identity/refresh";

        }
    }
}
