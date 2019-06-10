namespace PenMgmt.Server.Api
{
    public class Constants
    {
        public static readonly string BlankHttpPage = "about:blank";

        public class AppConfiguration
        {
            public static readonly string DataStore = "DataStore";
        }

        public class Http
        {
            public static readonly string ContentType = "application/problem+json";
            public static readonly string ContentLanguage = "en";
        }

        public class DB
        {
            public static readonly int Deleted = 1;
            public static readonly int NotDeleted = 0;
            public static readonly int DeletedDefault = NotDeleted;

            public static readonly int Committed = 1;
            public static readonly int NotCommitted = 0;
            public static readonly int CommittedDefault = NotCommitted;
        }

        public class Message
        {
            public static readonly string TitleGetObjects = "Get all objects";
            public static readonly string TitleGetObjectById = "Get object by Id";
            public static readonly string TitleCreateObject = "Create object";
            public static readonly string TitleUpdateObject = "Update object";
            public static readonly string TitleDeleteObject = "Delete object";

            public static readonly string ValidationFailed = "Object validation failed";
            public static readonly string ValidationFailedIdShouldBeNull = "Id is auto-generated key and should not be sent by creating an object";
            public static readonly string ValidationFailedIdsShouldMatch = "Id in query parameter and body of the request must match";

            public static readonly string CommittedObjectCantBeModified = "Can't modify the committed object";

            public static readonly string ObjectAlreadyDeleted = "Object already deleted";
        }
    }
}