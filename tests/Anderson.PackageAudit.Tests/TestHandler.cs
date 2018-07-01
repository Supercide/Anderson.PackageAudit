using System.Collections.Generic;
using Anderson.PackageAudit.Core.Errors;
using Anderson.PackageAudit.Errors;
using Anderson.Pipelines.Definitions;
using Anderson.Pipelines.Responses;

namespace Anderson.PackageAudit.Tests
{
    public class TestHandler : PipelineDefinition<IList<TestObject>, Response<IList<TestObject>, Error>>
    {
        public List<TestObject> Input = new List<TestObject>();

        public override Response<IList<TestObject>, Error> Handle(IList<TestObject> request)
        {
            Input.AddRange(request);

            return Input;
        }
    }
}