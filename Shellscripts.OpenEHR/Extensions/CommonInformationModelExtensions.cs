namespace Shellscripts.OpenEHR.Extensions
{
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataTypes;

    public static class  CommonInformationModelExtensions
    {

        #region 3.2.1 - Pathable Class (https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_pathable_class)

        public static Pathable Parent(this Pathable obj) => throw new NotImplementedException();

        public static object ItemAtPath(this Pathable obj, string path) => throw new NotImplementedException();

        public static object[] ItemsAtPath(this Pathable obj, string path) => throw new NotImplementedException();

        public static Boolean PathExists(this Pathable obj, string path) => throw new NotImplementedException();

        public static Boolean PathUnique(this Pathable obj, string path) => throw new NotImplementedException();

        public static string PathOfItem(this Pathable obj) => throw new NotImplementedException();

        #endregion


        #region 3.2.2 - Locatable Class (https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_locatable_class)

        public static DvText Concept(this Locatable obj)
        {
            throw new NotImplementedException();
        }

        public static Boolean IsArchetypeRoot(this Locatable obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
