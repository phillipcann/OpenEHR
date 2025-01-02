namespace Shellscripts.OpenEHR.Tests.Context
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Models.Ehr;


    public class EhrClientPostData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // string url, object? data, Type expectedException, string expectedExceptionMessage
            
            // 1. NullReferenceException
            yield return new object[]
            {
                string.Empty,
                null,
                typeof(NullReferenceException),
                "data cannot be null"
            };

            // 2. HttpRequestException - NotFound
            yield return new object[]
            {
                string.Empty,
                new Ehr() { },
                typeof(HttpRequestException),
                $"{(int)HttpStatusCode.NotFound}"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
