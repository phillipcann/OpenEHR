namespace Shellscripts.OpenEHR.Extensions
{
    using Models.BaseTypes;

    public static class BaseTypeExtensions
    {
        #region 5.4.6 - UID_BASED_ID Class

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

    }
}
