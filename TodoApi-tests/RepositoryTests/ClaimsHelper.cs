using System.Security.Claims;

namespace TodoApi_tests.RepositoryTests
{
    public class ClaimsHelper
    {
        public class TestPrincipal : ClaimsPrincipal
        {
            public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims))
            {
            }
        }

        public class TestIdentity : ClaimsIdentity
        {
            public TestIdentity(params Claim[] claims) : base(claims)
            {
            }
        }
    }
}
