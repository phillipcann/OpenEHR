namespace Shellscripts.OpenEHR.Extensions
{    
    using Models.BaseTypes;

    public static class BaseTypeExtensions
    {
        #region 5.4.6 - Uid Base Id Class (https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class)

        /// <summary>
        /// The identifier of the conceptual namespace in which the object exists, within the identification scheme. Returns the part to the left of the first '::' separator, if any, or else the whole string.
        /// </summary>
        /// <remarks>
        /// <a href="https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class">https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class</a>
        /// </remarks>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static Uid Root(this ObjectId objectId)
        {
            string part = objectId?.Value ?? string.Empty;

            if (objectId?.Value?.Contains("::") ?? false)
            {
                var startIndex = objectId.Value.IndexOf("::");
                part = objectId.Value.Substring(0, startIndex);
            }

            return new Uuid() { Value = part };
        }


        /// <summary>
        /// Optional local identifier of the object within the context of the root identifier. Returns the part to the right of the first '::' separator if any, or else any empty String.
        /// </summary>
        /// <remarks><a href="https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class">https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class</a></remarks>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static string Extension(this ObjectId objectId)
        {
            string part = string.Empty;

            if (objectId?.Value?.Contains("::") ?? false)
            {
                var startIndex = objectId.Value.IndexOf("::")+2;
                var remainingString = objectId.Value.Length - startIndex;

                if (remainingString >= 1)
                    part = objectId.Value.Substring(startIndex, remainingString);
            }

            return part;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks><a href="https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class">https://specifications.openehr.org/releases/BASE/latest/base_types.html#_uid_based_id_class</a></remarks>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public static bool HasExtension(this ObjectId objectId)
        {
            return !string.IsNullOrWhiteSpace(objectId.Extension());
        }

        #endregion

        #region 5.4.8 - Object Version Id Class (https://specifications.openehr.org/releases/BASE/latest/base_types.html#_object_version_id_class)

        public static Uid ObjectId(this ObjectVersionId objectVersionId) => throw new NotImplementedException();

        public static Uid CreatingSystemId(this ObjectVersionId objectVersionId) => throw new NotImplementedException();

        public static VersionTreeId VersionTreeId(this ObjectVersionId objectVersionId) => throw new NotImplementedException();

        public static Boolean IsBranch(this ObjectVersionId objectVersionId) => throw new NotImplementedException();

        #endregion

        #region 5.4.9 - Version Tree Id Class (https://specifications.openehr.org/releases/BASE/latest/base_types.html#_version_tree_id_class)

        public static string TrunkVersion(this VersionTreeId versionTreeId) => throw new NotImplementedException();
        public static Boolean IsBranch(this VersionTreeId versionTreeId) => throw new NotImplementedException();
        public static string BranchNumber(this VersionTreeId versionTreeId) => throw new NotImplementedException();
        public static string BranchVersion(this VersionTreeId versionTreeId) => throw new NotImplementedException();

        #endregion
    }
}