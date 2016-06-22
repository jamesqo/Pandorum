using Pandorum.Core.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Pandorum.Net.Tests
{
    public class PandoraUriBuilderTests
    {
        [Fact]
        public void DefaultToStringShouldBeEmpty()
        {
            Assert.Equal(string.Empty, default(PandoraUriBuilder).ToString());
        }

        [Fact]
        public void ImmutableSemantics()
        {
            var builder = new PandoraUriBuilder("foo");
            Assert.Equal("foo", builder.Endpoint);

            builder.WithEndpoint("bar");
            Assert.Equal("foo", builder.Endpoint);

            builder = builder.WithEndpoint("bar");
            Assert.Equal("bar", builder.Endpoint);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("!@#$%^&")] // should not call WebUtility.UrlEncode/Decode
        [InlineData("http://somewhere.com/")]
        [InlineData("http://tuner.pandora.com/services/json/")]
        public void OnlyEndpoint(string endpoint)
        {
            var builder = new PandoraUriBuilder(endpoint);
            Assert.Equal(endpoint, builder.Endpoint);
            Assert.Equal(endpoint, builder.ToString());

            builder = new PandoraUriBuilder().WithEndpoint(endpoint);
            Assert.Equal(endpoint, builder.Endpoint);
            Assert.Equal(endpoint, builder.ToString());
        }

        [Theory]
        [MemberData(nameof(SingleParameterData))]
        public void EndpointAndMethod(string endpoint, string method)
        {
            var builder = new PandoraUriBuilder(endpoint).WithMethod(method);
            Assert.Equal(method, builder.Method);

            var encoded = WebUtility.UrlEncode(method);
            Assert.Equal($"{endpoint}?method={encoded}", builder.ToString());
        }

        [Theory]
        [MemberData(nameof(SingleParameterData))]
        public void EndpointAndAuthToken(string endpoint, string authToken)
        {
            var builder = new PandoraUriBuilder(endpoint).WithAuthToken(authToken);
            Assert.Equal(authToken, builder.AuthToken);

            var encoded = WebUtility.UrlEncode(authToken);
            Assert.Equal($"{endpoint}?auth_token={encoded}", builder.ToString());
        }

        [Theory]
        [MemberData(nameof(SingleParameterData))]
        public void EndpointAndPartnerId(string endpoint, string partnerId)
        {
            var builder = new PandoraUriBuilder(endpoint).WithPartnerId(partnerId);
            Assert.Equal(partnerId, builder.PartnerId);

            var encoded = WebUtility.UrlEncode(partnerId);
            Assert.Equal($"{endpoint}?partner_id={encoded}", builder.ToString());
        }

        [Theory]
        [MemberData(nameof(SingleParameterData))]
        public void EndpointAndUserId(string endpoint, string userId)
        {
            var builder = new PandoraUriBuilder(endpoint).WithUserId(userId);
            Assert.Equal(userId, builder.UserId);

            var encoded = WebUtility.UrlEncode(userId);
            Assert.Equal($"{endpoint}?user_id={encoded}", builder.ToString());
        }

        [Theory]
        [MemberData(nameof(TwoParameterData))]
        public void MultipleParameters(string endpoint, string value1, string value2)
        {
            var builder = new PandoraUriBuilder(endpoint);
            var encoded1 = WebUtility.UrlEncode(value1);
            var encoded2 = WebUtility.UrlEncode(value2);

            var builder1 = builder
                .WithAuthToken(value1)
                .WithMethod(value2);

            Assert.Equal(value1, builder1.AuthToken);
            Assert.Equal(value2, builder1.Method);
            Assert.Equal($"{endpoint}?auth_token={encoded1}&method={encoded2}", builder1.ToString());

            var builder2 = builder1
                .WithAuthToken(value2)
                .WithUserId(value1);

            Assert.Equal(value2, builder2.AuthToken);
            Assert.Equal(value2, builder2.Method);
            Assert.Equal(value1, builder2.UserId);
            Assert.Equal($"{endpoint}?auth_token={encoded2}&method={encoded2}&user_id={encoded1}", builder2.ToString());
        }

        public static IEnumerable<object[]> SingleParameterData()
        {
            yield return new object[] { "", "auth.userLogin" };
            yield return new object[] { "Foo.Bar.Baz", "Quux.Blah" };
            yield return new object[] { "This has spaces", "This needs encoding" };
            yield return new object[] { null, ")(*(*&^^%$%#" };
        }

        public static IEnumerable<object[]> TwoParameterData()
        {
            yield return new object[] { null, "FarBooQuuxQuux", "QuuxQuuxBooFar" };
            yield return new object[] { "underscores_and spaces", "$ymbol$", "*&^*&" };
            yield return new object[] { "!@#$%", "23434", "\0\0\0" };
        }
    }
}
